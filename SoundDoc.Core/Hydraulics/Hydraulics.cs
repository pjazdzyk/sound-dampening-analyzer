using System;
using System.Text;
using NoiseAnalyzer.Core.Utils;
using SoundDoc.Core.Extensions;
using SoundDoc.Core.Physics;

namespace SoundDoc.Core.Data.HydraulicModels
{
    public class Hydraulics
    {
        // UPSTREAM GEOMETRY: INPUT PROPERTY
        public double InDim1 { get; private set; }                   // bok A kanału lub średnica w przypadku kanałów okrągłych
        public double InDim2 { get; private set; }                   // bok B odejścia

        // UPSTREAM GEOMETRY: CALCULATION PROPERTY
        public double InEquivD { get; private set; }                 // srednica ekwiwalentna kanału
        public double InHydrD { get; private set; }                  // średnica hydrauliczna kanału
        public double InHydrR { get; private set; }                  // promień hydrauliczny kanału
        public double InArea { get; private set; }                   // powierzchnia przekroju poprzecznego

        // UPSTREAM FLOW: INPUT PROPERTY
        public double InVolFlow { get; private set; }                // napływajacy objetosciowy strumien przeplywu

        // UPSTREAM FLOW: CALCULATION PROPERTY
        public double InVelocity { get; private set; }               // predkosc przeplywu w kroccu wlotowym
        //public double[] InStrouhalAr { get; private set; }         // tablica z liczbami Stouhala dla danej geometri

        public Hydraulics() : this(Defaults.FlowIn, Defaults.DimSize, Defaults.DimSize) { }

        public Hydraulics(double inFlow, double inDim1, double inDim2 = 0.0) 
        {
            InitializeUpStreamGeometry(inDim1, inDim2);
            InitializeUpStreamFlow(inFlow);
            UpdateUpStreamFlowProperties();
        }

        protected void InitializeUpStreamGeometry(double dim1, double dim2 = 0) 
        {
            if (dim1 <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dim1));
            }

            if (dim2 < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dim2));
            }

            InDim1 = dim1;
            InDim2 = dim2;
            InHydrD = PhysicsCommon.CalcHydrDiameter(dim1, dim2);
            InHydrR = PhysicsCommon.CalcHydrRadius(dim1, dim2);
            InEquivD = PhysicsCommon.CalcEquivDiameter(dim1, dim2);
            InArea = PhysicsCommon.CalcDuctSecArea(dim1, dim2);
           
        }

        protected void InitializeUpStreamFlow(double volFlow) 
        {
            if (volFlow < 0)
                throw new ArgumentException();
            InVolFlow = volFlow;
        }
        
        // SETTERS WITH CALCULATION PROPERTY AUTO-UPDATE
        public virtual void SetUpStreamGeometry(double dim1, double dim2 = 0) 
        {
            InitializeUpStreamGeometry(dim1, dim2);
            UpdateUpStreamFlowProperties();
        }

        public virtual void SetUpStreamFlow(double volFlow) 
        {
            InitializeUpStreamFlow(volFlow);
            UpdateUpStreamFlowProperties();
        }

        // VALUE UPDATER
        public virtual void UpdateUpStreamFlowProperties() 
        {
            InVelocity = PhysicsCommon.CalcAirVelocity(InArea, InVolFlow);
        }

        public override string ToString() 
        {
            var builder = new StringBuilder();
            builder.AppendLine("\nUpStream properties:");
            builder.Append($"inDim1= {InDim1.RoundUp(2)}");
            builder.Append($"  inDim2= {InDim2.RoundUp(2)}");
            builder.Append($"  inArea= {InArea.RoundUp(2)}");
            builder.Append($"  inVolFlow= {InVolFlow.RoundUp(2)}");
            builder.Append($"  inVelocity= {InVelocity.RoundUp(2)}");
            builder.Append($"  inEquivD= {InEquivD.RoundUp(2)}");
            builder.Append($"  inHydrD= {InHydrD.RoundUp(2)}");
            builder.Append($"  inHydrR= {InHydrR.RoundUp(2)}");

            return builder.ToString();
        }
    }
}