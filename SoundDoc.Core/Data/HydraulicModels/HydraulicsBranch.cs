using System;
using System.Text;
using SoundDoc.Core.Extensions;
using SoundDoc.Core.Physics;

namespace SoundDoc.Core.Data.HydraulicModels
{
    public class HydraulicsBranch : HydraulicsBend
    {
        // BRANCH GEOMETRY: INPUT PROPERTY
        public double BrFiletRad { get; private set; }              // promień zaokrąglenia krawędzi kolana lub trójnika

        // BRANCH FLOW: INPUT PROPERTY
        public double BrVolFlow { get; private set; }               // objetosciowy strumien przeplywu w odejściu

        // DOWNSTREAM GEOMETRY: INPUT PROPERTY
        public double OutDim1 { get; private set; }                  // bok A kanału lub średnica hydrauliczna wylotu
        public double OutDim2 { get; private set; }                  // bok b kanału wylotu

        // DOWNSTREAM GEOMETRY: CALCULATION PROPERTY
        public double OutEquivD { get; private set; }                // średnica ekwiwalentna wylotu
        public double OutHydrD { get; private set; }                 // średnica hydrauliczna wylotu
        public double OutArea { get; private set; }                  // powierzchnia przekroju poprzecznego wylotu

        // UPSTREAM FLOW: INPUT PROPERTY
        public double OutVolFlow { get; private set; }               // objetosciowy strumien przeplywu na wylocie

        // DOWNSTREAM FLOW: CALCULATION PROPERTY
        public double OutVelocity { get; private set; }              // predkosc przeplywu w kroccu wylotowym

        private HydraulicsBranch() : this(Defaults.FlowIn, Defaults.FlowBr, Defaults.AngleMax, Defaults.FiletRad, Defaults.DimSize, Defaults.DimSize, Defaults.DimSize, Defaults.DimSize, Defaults.DimSize, Defaults.DimSize) { }

        public static new HydraulicsBranch FromDefaults() 
        {
            return new();
        }

        private HydraulicsBranch(double inVolFlow, double brVolFlow, double brAngle, double brFiletRadius, double inDim1, double brDim1, double outDim1, double inDim2 = 0, double brDim2 = 0, double outDim2 = 0)
            : base(inVolFlow, brAngle, inDim1, brDim1, inDim2, brDim2)

        {
            InitializeBranchGeometry(brAngle,brFiletRadius,brDim1,brDim2);
            InitializeDownStreamGeometry(outDim1,outDim2);
            CheckFlowBalance(inVolFlow, brVolFlow);
            InitializeBranchFlow(brVolFlow);
            InitializeOutFlow(inVolFlow,brVolFlow);
            UpdateUpStreamFlowProperties();
            UpdateBranchFlowProperties();
            UpdateDownStreamFlowProperties();
        }

        public static HydraulicsBranch Create(double inVolFlow, double brVolFlow, double brAngle, double brFiletRadius, double inDim1, double brDim1, double outDim1, double inDim2 = 0, double brDim2 = 0, double outDim2 = 0) 
        {
            return new(inVolFlow, brVolFlow, brAngle, brFiletRadius, inDim1, brDim1, outDim1, inDim2, brDim2, outDim2);
        }


        // INITIALIZERS
        private void InitializeBranchGeometry(double brAngle, double brFiletRad, double brDim1, double brDim2 = 0) 
        {
            if (brFiletRad < 0)
                throw new ArgumentException();
            InitializeBendGeometry(brAngle, brDim1, brDim2);
            BrFiletRad = brFiletRad;
        }

        private void InitializeDownStreamGeometry(double dim1, double dim2 = 0) 
        {
            if (dim1 < 0 || dim2 < 0) 
                throw new ArgumentException(nameof(dim1) + "=" + dim1 + " " + nameof(dim2) + "=" + dim2 + " Dimensions cannot be nevative value");

            OutDim1 = dim1;
            OutDim2 = dim2;
            OutHydrD = PhysicsCommon.CalcHydrDiameter(dim1, dim2);
            OutEquivD = PhysicsCommon.CalcEquivDiameter(dim1, dim2);
            OutArea = PhysicsCommon.CalcDuctSecArea(dim1, dim2);
        }

        private void InitializeBranchFlow(double brVolFlow)
        {
            if (brVolFlow < 0)
                throw new ArgumentException(nameof(brVolFlow) + "=" + brVolFlow + " Flow in branch cannot be nevative value.");
            
            BrVolFlow = brVolFlow;
        }

        private void InitializeOutFlow(double inVolFlow, double brVolFlow) 
        {
            if (inVolFlow < 0 || brVolFlow < 0)
                throw new ArgumentException(nameof(inVolFlow) + "=" + inVolFlow + " " + nameof(brVolFlow) + "=" + brVolFlow + " " +
                                            "Flow canot be negative value.");
            OutVolFlow = inVolFlow - brVolFlow;
        }


        // SETTERS WITH CALCULATION PROPERTY AUTO-UPDATE
        private void CheckFlowBalance(double inVolFlow, double branchVolFlow) 
        {
            if (BrVolFlow > 0.0 && inVolFlow < branchVolFlow)
                throw new ArgumentException(nameof(inVolFlow) + "=" + inVolFlow + " " + nameof(branchVolFlow) + "=" + branchVolFlow + " " +
                                            "Flow in a branch cannot be greater than upstream flow.");
         }

        public override void SetUpStreamFlow(double inVolFlow)
        {
            CheckFlowBalance(inVolFlow,BrVolFlow);
            base.SetUpStreamFlow(inVolFlow);
            InitializeOutFlow(inVolFlow,BrVolFlow);
            UpdateDownStreamFlowProperties();
        }

        public void SetBranchGeometry(double brAngle, double brFiletRad, double brDim1, double brDim2 = 0)
        {
            InitializeBranchGeometry(brAngle,brFiletRad,brDim1,brDim2);
            UpdateBranchFlowProperties();
            UpdateDownStreamFlowProperties();
        }

        public void SetBranchFlow(double brVolFlow)
        {
            CheckFlowBalance(InVolFlow, brVolFlow);
            InitializeBranchFlow(brVolFlow);
            InitializeOutFlow(InVolFlow,brVolFlow);
            UpdateBranchFlowProperties();
            UpdateDownStreamFlowProperties();
        }

        public void SetDownStreamGeometry(double dim1, double dim2 = 0)
        {
            InitializeDownStreamGeometry(dim1, dim2);
            UpdateDownStreamFlowProperties();
        }

        protected override void UpdateBranchFlowProperties()
        {
            BrVelocity = PhysicsCommon.CalcAirVelocity(BrArea, BrVolFlow);
        }

        private void UpdateDownStreamFlowProperties() 
        {
            OutVelocity = PhysicsCommon.CalcAirVelocity(OutArea, OutVolFlow);
        }

        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("\nUpStream properties:");
            builder.Append($"inDim1= {InDim1.RoundUp(2)}");
            builder.Append($"  inDim2= {InDim2.RoundUp(2)}");
            builder.Append($"  inArea= {InArea.RoundUp(2)}");
            builder.Append($"  inVolFlow= {InVolFlow.RoundUp(2)}");
            builder.Append($"  inVelocity= {InVelocity.RoundUp(2)}");
            builder.Append($"  inEquivD= {InEquivD.RoundUp(2)}");
            builder.Append($"  inHydrD= {InHydrD.RoundUp(2)}");
            builder.Append($"  inHydrR= {InHydrR.RoundUp(2)}");
            builder.AppendLine("\nBranch properties:");
            builder.Append($"brDim1= {BrDim1.RoundUp(2)}");
            builder.Append($"  brDim2= {BrDim2.RoundUp(2)}");
            builder.Append($"  brArea= {BrArea.RoundUp(2)}");
            builder.Append($"  brVolFlow= {BrVolFlow.RoundUp(2)}");
            builder.Append($"  brVelocity= {BrVelocity.RoundUp(2)}");
            builder.Append($"  brEquivD= {BrEquivD.RoundUp(2)}");
            builder.Append($"  brHydrD= {BrHydrD.RoundUp(2)}");
            builder.AppendLine("\nDownStream properties:");
            builder.Append($"outDim1= {OutDim1.RoundUp(2)}");
            builder.Append($"  otDim2= {OutDim2.RoundUp(2)}");
            builder.Append($"  otArea= {OutArea.RoundUp(2)}");
            builder.Append($"  otVolFlow= {OutVolFlow.RoundUp(2)}");
            builder.Append($"  otVelocity= {OutVelocity.RoundUp(2)}");
            builder.Append($"  otEquivD= {OutEquivD.RoundUp(2)}");
            builder.Append($"  otHydrD= {OutHydrD.RoundUp(2)}");

            return "\n--HYDRAULICS BRANCH--" + builder.ToString();

        }

    }
}
