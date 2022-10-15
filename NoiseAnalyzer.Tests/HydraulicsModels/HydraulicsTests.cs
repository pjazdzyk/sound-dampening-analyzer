using System;
using Xunit;
using NoiseAnalyzer.Core.Physics;
using NoiseAnalyzer.Core.Utils;
using NoiseAnalyzer.Core.HydraulicModels;

namespace NoiseAnalyzer.Tests.HydraulicsModels
{
    public class HydraulicsTests
    {
        //Arrange
        const double Dim1 = 0.2;
        const double Dim2 = 0.5;
        const double VolFlow = 0.8;
        const double NewDim1 = 1.5;
        const double NewDim2 = 2.5;
        const double NewVolFlow = 2.8;
        const double ZeroValue = 0.0;
        const double NegativeValue = -0.5;

        [Fact]
        public void HydraulicsDefaultConstructorTests() {

            // ARRANGE
            var expectedDim1 = Defaults.DimSize;
            var expectedDim2 = Defaults.DimSize;
            var expectedFlow = Defaults.FlowIn;
            var expectedArea = PhysicsCommon.CalcDuctSecArea(expectedDim1, expectedDim2);
            var expectedVel = PhysicsCommon.CalcAirVelocity(expectedArea,expectedFlow);
            var expectedEqvD = PhysicsCommon.CalcEquivDiameter(expectedDim1, expectedDim2);
            var expectedHdrD = PhysicsCommon.CalcHydrDiameter(expectedDim1, expectedDim2);

            // ACT
            var hydraulics = new Hydraulics();
            var actDim1 = hydraulics.InDim1;
            var actDim2 = hydraulics.InDim2;
            var actArea = hydraulics.InArea;
            var actVolFlow = hydraulics.InVolFlow;
            var actInVel = hydraulics.InVelocity;
            var actInEqvD = hydraulics.InEquivD;
            var actInHydrD = hydraulics.InHydrD;

            // Assert
            Assert.NotNull(hydraulics);
            Assert.Equal(expectedDim1, actDim1);
            Assert.Equal(expectedDim2, actDim2);
            Assert.Equal(expectedArea, actArea);
            Assert.Equal(expectedFlow, actVolFlow);
            Assert.Equal(expectedVel, actInVel);
            Assert.Equal(expectedEqvD, actInEqvD);
            Assert.Equal(expectedHdrD, actInHydrD);
            
        }

        [Theory]
        [InlineData(Dim1, Dim2, VolFlow, NewDim1, NewDim2, NewVolFlow)]
        [InlineData(Dim1, ZeroValue, VolFlow, NewDim1, NewDim2, NewVolFlow)]
        [InlineData(Dim1, Dim2, VolFlow, NewDim1, ZeroValue, NewVolFlow)]
        public void HydraulicsConstructorAndSettersTests(Double dim1, Double dim2, Double volFlow, Double newDim1, Double newDim2, Double newFlow) 
        {        
            //1 - INITIALIZATION TESTS

            // Arrange
            var expectedDim1 = dim1;
            var expectedDim2 = dim2;
            var expectedFlow = volFlow;
            var expectedArea = PhysicsCommon.CalcDuctSecArea(expectedDim1, expectedDim2);
            var expectedVel = PhysicsCommon.CalcAirVelocity(expectedArea, expectedFlow);
            var expectedEqvD = PhysicsCommon.CalcEquivDiameter(expectedDim1, expectedDim2);
            var expectedHdrD = PhysicsCommon.CalcHydrDiameter(expectedDim1, expectedDim2);

            // Act
            var hydraulics = new Hydraulics(volFlow, dim1, dim2);
            var actDim1 = hydraulics.InDim1;
            var actDim2 = hydraulics.InDim2;
            var actArea = hydraulics.InArea;
            var actVolFlow = hydraulics.InVolFlow;
            var actInVel = hydraulics.InVelocity;
            var actInEqvD = hydraulics.InEquivD;
            var actInHydrD = hydraulics.InHydrD;

            // Assert
            Assert.NotNull(hydraulics);
            Assert.Equal(expectedDim1, actDim1);
            Assert.Equal(expectedDim2, actDim2);
            Assert.Equal(expectedArea, actArea);
            Assert.Equal(expectedFlow, actVolFlow);
            Assert.Equal(expectedVel, actInVel);
            Assert.Equal(expectedEqvD, actInEqvD);
            Assert.Equal(expectedHdrD, actInHydrD);

            //2 - MODIFICATION TESTS

            // Arrange
            expectedDim1 = newDim1;
            expectedDim2 = newDim2;
            expectedFlow = newFlow;
            expectedArea = PhysicsCommon.CalcDuctSecArea(expectedDim1, expectedDim2);
            expectedVel = PhysicsCommon.CalcAirVelocity(expectedArea, expectedFlow);
            expectedEqvD = PhysicsCommon.CalcEquivDiameter(expectedDim1, expectedDim2);
            expectedHdrD = PhysicsCommon.CalcHydrDiameter(expectedDim1, expectedDim2);
           
            // Modification
            hydraulics.SetUpStreamGeometry(expectedDim1, expectedDim2);
            hydraulics.SetUpStreamFlow(expectedFlow);

            // Act
            actDim1 = hydraulics.InDim1;
            actDim2 = hydraulics.InDim2;
            actArea = hydraulics.InArea;
            actVolFlow = hydraulics.InVolFlow;
            actInVel = hydraulics.InVelocity;
            actInEqvD = hydraulics.InEquivD;
            actInHydrD = hydraulics.InHydrD;

            // Assert
            Assert.Equal(expectedDim1, actDim1);
            Assert.Equal(expectedDim2, actDim2);
            Assert.Equal(expectedArea, actArea);
            Assert.Equal(expectedFlow, actVolFlow);
            Assert.Equal(expectedVel, actInVel);
            Assert.Equal(expectedEqvD, actInEqvD);
            Assert.Equal(expectedHdrD, actInHydrD);
  
        }

        [Theory]
        [InlineData(ZeroValue, Dim2, VolFlow)]
        [InlineData(ZeroValue, ZeroValue, VolFlow)]
        [InlineData(ZeroValue, ZeroValue, ZeroValue)]
        [InlineData(NegativeValue, Dim2, VolFlow)]
        [InlineData(NegativeValue, NegativeValue, VolFlow)]
        [InlineData(NegativeValue, NegativeValue, NegativeValue)]
        public void HydraulicConstructorExceptionsTest(Double dim1, Double dim2, Double volFlow) 
        {
            //Assert
            Assert.ThrowsAny<Exception>(() => new Hydraulics(volFlow,dim1,dim2));
        }

        [Theory]
        [InlineData(ZeroValue, Dim2)]
        [InlineData(ZeroValue, ZeroValue)]
        [InlineData(Dim1, NegativeValue)]
        [InlineData(NegativeValue, Dim2)]
        [InlineData(NegativeValue, NegativeValue)]
        public void HydraulicsSetUpStreamGeometryExceptionsTest(Double newDim1, Double newDim2)
        {
            // Act
            var hydraulics = new Hydraulics(VolFlow, Dim1, Dim2);

            // Assert
            Assert.ThrowsAny<Exception>(() => hydraulics.SetUpStreamGeometry(newDim1, newDim2));
        }

        [Fact]
        public void HydraulicsSetUpStreamFlowExceptionsTest()
        {
            // Act
            var hydraulics = new Hydraulics(VolFlow, Dim1, Dim2);

            // Assert
            Assert.ThrowsAny<Exception>(() => hydraulics.SetUpStreamFlow(NegativeValue));
        }



    }
}
