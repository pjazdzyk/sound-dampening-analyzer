using System;
using Xunit;
using NoiseAnalyzer.Core.Utils;
using NoiseAnalyzer.Core.HydraulicModels;
using NoiseAnalyzer.Core.Physics;

namespace NoiseAnalyzer.Tests.HydraulicsModels
{
    public class HydraulicsBranchTests
    {
        const double InDim1 = 0.4;
        const double InDim2 = 0.4;
        const double InVolFlow = 0.8;
        const double BranchDim1 = 0.2;
        const double BranchDim2 = 0.1;
        const double BranchFiletRad = 0.05;
        const double BranchAngle = 45;
        const double BranchFlow = 0.2;
        const double OutDim1 = 0.4;
        const double OutDim2 = 0.2;
        const double NewDim1 = 1.5;
        const double NewDim2 = 2.5;
        const double NewVolFlow = 2.8;
        const double NewBranchFlow = 0.6;
        const double NewFiletRad = 0.1;
        const double NewAngle = 30;
        const double ZeroValue = 0.0;
        const double NegativeValue = -0.5;
        
        [Fact]
        public void HydraulicsBranchDefaultConstructorTests()
        {
            // Arrange
            var expectedInDim1 = Defaults.DimSize;
            var expectedInDim2 = Defaults.DimSize;
            var expectedInVolFlow = Defaults.FlowIn;
            var expectedBranchDim1 = Defaults.DimSize;
            var expectedBranchDim2 = Defaults.DimSize;
            var expectedBranchFiletRadius = Defaults.FiletRad;
            var expectedBranchAngle = Defaults.AngleMax;
            var expectedBranchFlow = Defaults.FlowBr;
            var expectedOutDim1 = Defaults.DimSize;
            var expectedOutDim2 = Defaults.DimSize;
            var expectedOutFlow = expectedInVolFlow - expectedBranchFlow;
            var expectedBrHydrD = PhysicsCommon.CalcHydrDiameter(expectedBranchDim1, expectedBranchDim2);
            var expectedOutHydrD = PhysicsCommon.CalcHydrDiameter(expectedOutDim1, expectedOutDim2);
            var expectedBrEquivD = PhysicsCommon.CalcEquivDiameter(expectedBranchDim1, expectedBranchDim2);
            var expectedOutEquivD = PhysicsCommon.CalcEquivDiameter(expectedOutDim1, expectedOutDim2);
            var expectedBrDuctArea = PhysicsCommon.CalcDuctSecArea(expectedBranchDim1, expectedBranchDim2);
            var expectedBrDuctVelocity = PhysicsCommon.CalcAirVelocity(expectedBrDuctArea, expectedBranchFlow);
            var expectedOutDuctArea = PhysicsCommon.CalcDuctSecArea(expectedOutDim1, expectedOutDim2);
            var expectedOutDuctVelocity = PhysicsCommon.CalcAirVelocity(expectedOutDuctArea, expectedOutFlow);

            // Act
            var hydraulics = HydraulicsBranch.FromDefaults();
            var actInDim1 = hydraulics.InDim1;
            var actInDim2 = hydraulics.InDim2;
            var actInVolFlow = hydraulics.InVolFlow;
            var actBranchDim1 = hydraulics.BrDim1;
            var actBranchDim2 = hydraulics.BrDim2;
            var actBranchFiletRadius = hydraulics.BrFiletRad;
            var actBranchAngle = hydraulics.BrAngle;
            var actBranchFlow = hydraulics.BrVolFlow;
            var actOutDim1 = hydraulics.OutDim1;
            var actOutDim2 = hydraulics.OutDim2;
            var actOutFlow = hydraulics.OutVolFlow;
            var actBrHydrD = hydraulics.BrHydrD;
            var actOutHydrD = hydraulics.OutHydrD;
            var actBrEquivD = hydraulics.BrEquivD;
            var actOutEquivD = hydraulics.OutEquivD;
            var actBrDuctArea = hydraulics.BrArea;
            var actBrDuctVelocity = hydraulics.BrVelocity;
            var actOutDuctArea = hydraulics.OutArea;
            var actOutDuctVelocity = hydraulics.OutVelocity;
 
            // Assert
            Assert.NotNull(hydraulics);
            Assert.Equal(expectedInDim1, actInDim1);
            Assert.Equal(expectedInDim2, actInDim2);
            Assert.Equal(expectedInVolFlow, actInVolFlow);
            Assert.Equal(expectedBranchDim1, actBranchDim1);
            Assert.Equal(expectedBranchDim2, actBranchDim2);
            Assert.Equal(expectedBranchFiletRadius, actBranchFiletRadius);
            Assert.Equal(expectedBranchAngle, actBranchAngle);
            Assert.Equal(expectedBranchFlow, actBranchFlow);
            Assert.Equal(expectedOutDim1, actOutDim1);
            Assert.Equal(expectedOutDim2, actOutDim2);
            Assert.Equal(expectedOutFlow, actOutFlow);
            Assert.Equal(expectedBrHydrD, actBrHydrD);
            Assert.Equal(expectedOutHydrD, actOutHydrD);
            Assert.Equal(expectedBrEquivD, actBrEquivD);
            Assert.Equal(expectedOutEquivD, actOutEquivD);
            Assert.Equal(expectedBrDuctArea, actBrDuctArea);
            Assert.Equal(expectedBrDuctVelocity, actBrDuctVelocity);
            Assert.Equal(expectedOutDuctArea, actOutDuctArea);
            Assert.Equal(expectedOutDuctVelocity, actOutDuctVelocity);
        }

        [Theory]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, BranchFiletRad, InDim1, BranchDim1, OutDim1, InDim2, BranchDim2, OutDim2)]
        [InlineData(InVolFlow, ZeroValue, BranchAngle, BranchFiletRad, InDim1, BranchDim1, OutDim1, InDim2, BranchDim2, OutDim2)]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, BranchFiletRad, InDim1, BranchDim1, OutDim1, ZeroValue, BranchDim2, OutDim2)]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, BranchFiletRad, InDim1, BranchDim1, OutDim1, InDim2, ZeroValue, OutDim2)]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, BranchFiletRad, InDim1, BranchDim1, OutDim1, InDim2, BranchDim2, ZeroValue)]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, BranchFiletRad, InDim1, BranchDim1, OutDim1, ZeroValue, ZeroValue, ZeroValue)]
        public void HydraulicsBranchConstructorTests(double inFlow, double brFlow, double brAngle, double brFiletRadius,
                             double inDim1, double brDim1, double outDim1, double inDim2, double brDim2, double outDim2)
        {
            // Arrange
            var expectedOutFlow = inFlow - brFlow;
            var expectedBrHydrD = PhysicsCommon.CalcHydrDiameter(brDim1, brDim2);
            var expectedOutHydrD = PhysicsCommon.CalcHydrDiameter(outDim1, outDim2);
            var expectedBrEquivD = PhysicsCommon.CalcEquivDiameter(brDim1, brDim2);
            var expectedOutEquivD = PhysicsCommon.CalcEquivDiameter(outDim1, outDim2);
            var expectedBrDuctArea = PhysicsCommon.CalcDuctSecArea(brDim1, brDim2);
            var expectedBrDuctVelocity = PhysicsCommon.CalcAirVelocity(expectedBrDuctArea, brFlow);
            var expectedOutDuctArea = PhysicsCommon.CalcDuctSecArea(outDim1, outDim2);
            var expectedOutDuctVelocity = PhysicsCommon.CalcAirVelocity(expectedOutDuctArea, expectedOutFlow);

            // Act
            var hydraulics = HydraulicsBranch.Create(inFlow, brFlow, brAngle, brFiletRadius, inDim1, brDim1, outDim1, inDim2, brDim2, outDim2);
            var actInDim1 = hydraulics.InDim1;
            var actInDim2 = hydraulics.InDim2;
            var actInVolFlow = hydraulics.InVolFlow;
            var actBranchDim1 = hydraulics.BrDim1;
            var actBranchDim2 = hydraulics.BrDim2;
            var actBranchFiletRadius = hydraulics.BrFiletRad;
            var actBranchAngle = hydraulics.BrAngle;
            var actBranchFlow = hydraulics.BrVolFlow;
            var actOutDim1 = hydraulics.OutDim1;
            var actOutDim2 = hydraulics.OutDim2;
            var actOutFlow = hydraulics.OutVolFlow;
            var actBrHydrD = hydraulics.BrHydrD;
            var actOutHydrD = hydraulics.OutHydrD;
            var actBrEquivD = hydraulics.BrEquivD;
            var actOutEquivD = hydraulics.OutEquivD;
            var actBrDuctArea = hydraulics.BrArea;
            var actBrDuctVelocity = hydraulics.BrVelocity;
            var actOutDuctArea = hydraulics.OutArea;
            var actOutDuctVelocity = hydraulics.OutVelocity;

            // Assert
            Assert.NotNull(hydraulics);
            Assert.Equal(inDim1, actInDim1);
            Assert.Equal(inDim2, actInDim2);
            Assert.Equal(inFlow, actInVolFlow);
            Assert.Equal(brDim1, actBranchDim1);
            Assert.Equal(brDim2, actBranchDim2);
            Assert.Equal(brFiletRadius, actBranchFiletRadius);
            Assert.Equal(brAngle, actBranchAngle);
            Assert.Equal(brFlow, actBranchFlow);
            Assert.Equal(outDim1, actOutDim1);
            Assert.Equal(outDim2, actOutDim2);
            Assert.Equal(expectedOutFlow, actOutFlow);
            Assert.Equal(expectedBrHydrD, actBrHydrD);
            Assert.Equal(expectedOutHydrD, actOutHydrD);
            Assert.Equal(expectedBrEquivD, actBrEquivD);
            Assert.Equal(expectedOutEquivD, actOutEquivD);
            Assert.Equal(expectedBrDuctArea, actBrDuctArea);
            Assert.Equal(expectedBrDuctVelocity, actBrDuctVelocity);
            Assert.Equal(expectedOutDuctArea, actOutDuctArea);
            Assert.Equal(expectedOutDuctVelocity, actOutDuctVelocity);
        }

        [Theory]
        [InlineData(ZeroValue, BranchFlow, BranchAngle, BranchFiletRad, InDim1, BranchDim1, OutDim1, InDim2, BranchDim2, OutDim2)]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, BranchFiletRad, ZeroValue, BranchDim1, OutDim1, InDim2, BranchDim2, OutDim2)]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, BranchFiletRad, InDim1, ZeroValue, OutDim1, InDim2, BranchDim2, OutDim2)]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, BranchFiletRad, InDim1, BranchDim1, ZeroValue, InDim2, BranchDim2, OutDim2)]
        [InlineData(NegativeValue, BranchFlow, BranchAngle, BranchFiletRad, InDim1, BranchDim1, OutDim1, InDim2, BranchDim2, OutDim2)]
        [InlineData(InVolFlow, NegativeValue, BranchAngle, BranchFiletRad, InDim1, BranchDim1, OutDim1, InDim2, BranchDim2, OutDim2)]
        [InlineData(InVolFlow, BranchFlow, NegativeValue, BranchFiletRad, InDim1, BranchDim1, OutDim1, InDim2, BranchDim2, OutDim2)]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, NegativeValue, InDim1, BranchDim1, OutDim1, InDim2, BranchDim2, OutDim2)]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, BranchFiletRad, NegativeValue, BranchDim1, OutDim1, InDim2, BranchDim2, OutDim2)]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, BranchFiletRad, InDim1, NegativeValue, OutDim1, InDim2, BranchDim2, OutDim2)]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, BranchFiletRad, InDim1, BranchDim1, NegativeValue, InDim2, BranchDim2, OutDim2)]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, BranchFiletRad, InDim1, BranchDim1, OutDim1, NegativeValue, BranchDim2, OutDim2)]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, BranchFiletRad, InDim1, BranchDim1, OutDim1, InDim2, NegativeValue, OutDim2)]
        [InlineData(InVolFlow, BranchFlow, BranchAngle, BranchFiletRad, InDim1, BranchDim1, OutDim1, InDim2, BranchDim2, NegativeValue)]
        [InlineData(NegativeValue, NegativeValue, NegativeValue, NegativeValue, NegativeValue, NegativeValue, NegativeValue, NegativeValue, NegativeValue, NegativeValue)]
        [InlineData(ZeroValue, ZeroValue, ZeroValue, ZeroValue, ZeroValue, ZeroValue, ZeroValue, ZeroValue, ZeroValue, ZeroValue)]
        public void HydraulicsBranchConstructorExceptionTests(double inFlow, double brFlow, double brAngle, double brFiletRadius,
                             double inDim1, double brDim1, double outDim1, double inDim2, double brDim2, double outDim2)
        {
            Assert.ThrowsAny<Exception>(() => HydraulicsBranch.Create(inFlow, brFlow, brAngle, brFiletRadius, inDim1, brDim1, outDim1, inDim2, brDim2, outDim2));
        }
        
        [Fact]
        public void HydraulicsBranchSetFlowsTests()
        {
            // Act
            var hydraulics = HydraulicsBranch.FromDefaults();

            // Arrange
            var expectedInFlow = NewVolFlow;
            var expectedBranchFlow = NewBranchFlow;
            var expectedOutFlow = NewVolFlow - NewBranchFlow;
            var expectedInDuctVelocity = PhysicsCommon.CalcAirVelocity(hydraulics.InArea, expectedInFlow);
            var expectedBrDuctVelocity = PhysicsCommon.CalcAirVelocity(hydraulics.BrArea, expectedBranchFlow);
            var expectedOutDuctVelocity = PhysicsCommon.CalcAirVelocity(hydraulics.OutArea, expectedOutFlow);

            // Act
            hydraulics.SetUpStreamFlow(NewVolFlow);
            hydraulics.SetBranchFlow(NewBranchFlow);

            var actInFLow = hydraulics.InVolFlow;
            var actBranchFlow = hydraulics.BrVolFlow;
            var actOutFlow = hydraulics.OutVolFlow;
            var actInVelocity = hydraulics.InVelocity;
            var actBrVelocity = hydraulics.BrVelocity;
            var actOutVelocity = hydraulics.OutVelocity;

            // Assert
            Assert.Equal(expectedInFlow, actInFLow);
            Assert.Equal(expectedBranchFlow, actBranchFlow);
            Assert.Equal(expectedOutFlow, actOutFlow);
            Assert.Equal(expectedInDuctVelocity, actInVelocity);
            Assert.Equal(expectedBrDuctVelocity, actBrVelocity);
            Assert.Equal(expectedOutDuctVelocity, actOutVelocity);
        }

        [Theory]
        [InlineData(Defaults.FlowBr-0.01)]
        [InlineData(NegativeValue)]
        public void HydraulicsBranchSetUpStreamFlowExceptionsTests(double inFlow)
        {
            // Arrange
            var hydraulics = HydraulicsBranch.FromDefaults();

            // Assert
            Assert.ThrowsAny<Exception>(() => hydraulics.SetUpStreamFlow(inFlow));
        }

        [Theory]
        [InlineData(Defaults.FlowIn+0.01)]
        [InlineData(NegativeValue)]
        public void HydraulicsBranchSetBranchFlowsExceptionsTests(double branchFLow)
        {
            // Arrange
            var hydraulics = HydraulicsBranch.FromDefaults();

            // Assert
            Assert.ThrowsAny<Exception>(() => hydraulics.SetBranchFlow(branchFLow));
        }

        [Theory]
        [InlineData(NewAngle, NewFiletRad, NewDim1, NewDim2)]
        [InlineData(NewAngle, NewFiletRad, NewDim1, ZeroValue)]
        [InlineData(NewAngle, ZeroValue, NewDim1, NewDim2)]
        public void HydraulicsSetBranchGeometryTest(double angle, double filetRad, double dim1, double dim2) 
        {
            // Act
            var hydraulics = HydraulicsBranch.FromDefaults();
            hydraulics.SetBranchGeometry(angle, filetRad, dim1, dim2);

            // Arrange
            var expectedBrDuctVelocity = PhysicsCommon.CalcAirVelocity(hydraulics.BrArea, hydraulics.BrVolFlow);

            // Act
            var actAngle = hydraulics.BrAngle;
            var actFiletRad = hydraulics.BrFiletRad;
            var actDim1 = hydraulics.BrDim1;
            var actDim2 = hydraulics.BrDim2;
            var actBrVel = hydraulics.BrVelocity;

            // Assert
            Assert.Equal(angle,actAngle);
            Assert.Equal(filetRad,actFiletRad);
            Assert.Equal(dim1,actDim1);
            Assert.Equal(dim2,actDim2);
            Assert.Equal(expectedBrDuctVelocity, actBrVel);
        }

        [Theory]
        [InlineData(ZeroValue, InDim2)]
        [InlineData(ZeroValue, ZeroValue)]
        [InlineData(InDim1, NegativeValue)]
        [InlineData(NegativeValue, InDim2)]
        [InlineData(NegativeValue, NegativeValue)]
        public void HydraulicsSetBranchGeometryExceptionsTest(double newDim1, double newDim2)
        {
            // Act
            var hydraulics = HydraulicsBranch.FromDefaults();

            // Assert
            Assert.ThrowsAny<Exception>(() => hydraulics.SetBranchGeometry(BranchAngle, BranchFiletRad, newDim1, newDim2));
        }

        [Theory]
        [InlineData(NewDim1, NewDim2)]
        [InlineData(NewDim1, ZeroValue)]
        public void HydraulicsSetDownStreamGeometryTest(double dim1, double dim2)
        {
            // Act
            var hydraulics = HydraulicsBranch.FromDefaults();
            hydraulics.SetDownStreamGeometry(dim1, dim2);

            // Arrange
            var expectedOutDuctVelocity = PhysicsCommon.CalcAirVelocity(hydraulics.OutArea, hydraulics.OutVolFlow);

            // Act
            var actDim1 = hydraulics.OutDim1;
            var actDim2 = hydraulics.OutDim2;
            var actOutVel = hydraulics.OutVelocity;

            // Assert
            Assert.Equal(dim1, actDim1);
            Assert.Equal(dim2, actDim2);
            Assert.Equal(expectedOutDuctVelocity, actOutVel);
        }

        [Theory]
        [InlineData(ZeroValue, InDim2)]
        [InlineData(ZeroValue, ZeroValue)]
        [InlineData(InDim1, NegativeValue)]
        [InlineData(NegativeValue, InDim2)]
        [InlineData(NegativeValue, NegativeValue)]
        public void HydraulicsSetDownStreamGeometryExceptionsTest(double newDim1, double newDim2)
        {
            // Act
            var hydraulics = HydraulicsBranch.FromDefaults();

            // Assert
            Assert.ThrowsAny<Exception>(() => hydraulics.SetDownStreamGeometry(newDim1, newDim2));
        }
    }
}
