using System;
using System.Text;
using NoiseAnalyzer.Core.Utils;
using SoundDoc.Core.Extensions;
using SoundDoc.Core.Physics;

namespace SoundDoc.Core.Data.HydraulicModels
{
    public class HydraulicsBend : Hydraulics
    {
        // GEOMETRY: INPUT PROPERTY
        public double BrDim1 { get; protected set; }                  // bok A odejścia kolana lub średnica ekwiwalentna
        public double BrDim2 { get; protected set; }                  // bok B odejscia kolana
        public double BrAngle { get; protected set; }                 // kąt odejścia kolana

        // BRANCH GEOMETRY: CALCULATION PROPERTY
        public double BrEquivD { get; protected set; }                // średnica ekwiwalentna odejścia
        public double BrHydrD { get; protected set; }                 // średnica hydrauliczna odejścia
        public double BrArea { get; protected set; }                  // powierzchnia przekroju poprzecznego odejścia

        // BRANCH FLOW: CALCULATION PROPERTY
        public double BrVelocity { get; protected set; }              // predkosc przeplywu w odejściu
        
        protected HydraulicsBend() : this(Defaults.FlowIn, Defaults.AngleMax, Defaults.DimSize, Defaults.DimSize) { }

        public static HydraulicsBend FromDefaults() 
        {
            return new();
        }

        protected HydraulicsBend(double inFlow, double brAngle, double inDim1, double brDim1, double inDim2 = 0.0, double brDim2=0.0) 
            : base(inFlow, inDim1, inDim2)
        {
            InitializeBendGeometry(brAngle, brDim1, brDim2);
            UpdateBranchFlowProperties();
        }

        public static HydraulicsBend Create(double inFlow, double brAngle, double inDim1, double brDim1, double inDim2 = 0.0, double brDim2 = 0.0) 
        {
            return new(inFlow, brAngle, inDim1, brDim1, inDim2, brDim2);
        }

        // INITIALIZERS
        protected void InitializeBendGeometry(double brAngle, double brDim1, double brDim2)
        {
            if (brDim1 < 0 || brDim2 < 0 || brAngle < 0 || brAngle>Defaults.AngleMax)
                throw new ArgumentException();

            BrDim1 = brDim1;
            BrDim2 = brDim2;
            BrAngle = brAngle;
            BrHydrD = PhysicsCommon.CalcHydrDiameter(brDim1, brDim2);
            BrEquivD = PhysicsCommon.CalcEquivDiameter(brDim1, brDim2);
            BrArea = PhysicsCommon.CalcDuctSecArea(brDim1, brDim2);
        }

        public override void SetUpStreamFlow(double inVolFlow)
        {
            base.SetUpStreamFlow(inVolFlow);
            UpdateBranchFlowProperties();
        }

        public void SetBranchGeometry(double brAngle, double brDim1, double brDim2 = 0)
        {
            InitializeBendGeometry(brAngle, brDim1, brDim2);
            UpdateBranchFlowProperties();
        }

        protected virtual void UpdateBranchFlowProperties()
        {
            BrVelocity = PhysicsCommon.CalcAirVelocity(BrArea, InVolFlow);
        }

        public void SetBranchAngle(double brAngle)
        {
            if (brAngle < 0)
                throw new ArgumentException(nameof(brAngle) + ": Bend angle canot be negative.");
            if (brAngle > Defaults.AngleMax)
                throw new ArgumentException(nameof(brAngle) + ": Bend angle exceeds maximum value (" + Defaults.AngleMax +")");

            BrAngle = brAngle;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("\nBend-out properties:");
            builder.Append($"brDim1= {BrDim1.RoundUp(2)}");
            builder.Append($"  brDim2= {BrDim2.RoundUp(2)}");
            builder.Append($"  brArea= {BrArea.RoundUp(2)}");
            builder.Append($"  brVelocity= {BrVelocity.RoundUp(4)}");
            builder.Append($"  brEquivD= {BrEquivD.RoundUp(2)}");
            builder.Append($"  brHydrD= {BrHydrD.RoundUp(2)}");

            return "\n--HYDRAULICS BEND--" + base.ToString() + builder.ToString();

        }
    }
}