using System;
using NoiseAnalyzer.Core.Utils;
using SoundDoc.Core.Data;
using SoundDoc.Core.Data.HydraulicModels;
using SoundDoc.Core.Extensions;
using SoundDoc.Core.Physics;
using Xunit;

namespace SoundDoc.Tests.Physics
{
    public class PhysicsCommonTests
    {
        // Arrange (Shared)
        const double ZeroValue = 0.0;            // constant 0.0 value for exception tests
        const double NegativeValue = -0.6;       // generic negative value for all cases [-]
        const double MaxDimExceed = Defaults.DimMax + 0.01; // Exceeded maximum dimension [m]
        const double DimA = 0.6;                 // dimension size A [m]
        const double DimB = 0.3;                 // dimension size B [m]
        const double Vol1 = 0.50;                // airflow [m3/s]
        const double Area1 = 0.25;               // area [m2]
        const double SrcDa = 3.5;                // source dampening   
        const double SrcLw = Defaults.LwSrc;     // source noise
        const double Accuracy = 0.0000001;       // aceptable accuracy for double equality check
        static double Func(HydraulicsTerminal hydraulics, double fm) => -(71.72 - (67.37 / (1 + Math.Pow(fm * 1 / (hydraulics.InVelocity * hydraulics.DischargeCoefficient) / 363.74, 1.1))));

        [Fact]
        public void CalcCalcDuctSecAreaTest() 
        {
            // Arrange
            double expected1 = Math.PI * DimA * DimA / 4;
            double expected2 = DimA * DimB;

            // Act
            double result1 = PhysicsCommon.CalcDuctSecArea(DimA);
            double result2 = PhysicsCommon.CalcDuctSecArea(DimA,DimB);

            // Assert
            Assert.Equal(expected1,result1);
            Assert.Equal(expected2,result2);
        }

        [Theory]
        [InlineData(ZeroValue, DimA)]
        [InlineData(NegativeValue, DimA)]
        [InlineData(DimA, NegativeValue)]
        [InlineData(DimA, MaxDimExceed)]
        [InlineData(MaxDimExceed, DimA)]
        public void CalcCalcDuctSecAreaExceptionTest(double value1, double value2) 
        {
            // Assert
            Assert.ThrowsAny<Exception>( () => PhysicsCommon.CalcDuctSecArea(value1,value2) );
        }

        [Fact]
        public void CalcCalcDuctSecPerimeterTest()
        {
            // Arrange
            double expected1 = Math.PI * DimA;
            double expected2 = 2*DimA + 2*DimB;

            // Act
            double result1 = PhysicsCommon.CalcDuctSecPerimeter(DimA);
            double result2 = PhysicsCommon.CalcDuctSecPerimeter(DimA, DimB);

            // Assert
            Assert.Equal(expected1, result1);
            Assert.Equal(expected2, result2);

        }

        [Theory]
        [InlineData(ZeroValue, DimA)]
        [InlineData(NegativeValue, DimA)]
        [InlineData(DimA, NegativeValue)]
        [InlineData(DimA, MaxDimExceed)]
        [InlineData(MaxDimExceed, DimA)]
        public void CalcCalcDuctSecPerimeterExceptionTest(double value1, double value2)
        {
            // Assert
            Assert.ThrowsAny<Exception>(() => PhysicsCommon.CalcDuctSecPerimeter(value1, value2));
        }

        [Fact]
        public void CalcAirVelocityTest()
        {
            // Act
            var result1 = PhysicsCommon.CalcAirVelocity(Area1, Vol1);
            var result2 = PhysicsCommon.CalcAirVelocity(Area1, ZeroValue);

            // Assert
            Assert.Equal(2.0, result1);
            Assert.Equal(0.0, result2);
        }

        [Theory]
        [InlineData(ZeroValue, Vol1)]
        [InlineData(ZeroValue, ZeroValue)]
        [InlineData(NegativeValue, Vol1)]
        [InlineData(Area1, NegativeValue)]
        [InlineData(NegativeValue, NegativeValue)]
        public void CalcAirVelocityExceptionsTest(double value1, double value2)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => PhysicsCommon.CalcAirVelocity(value1, value2));
        }

        [Fact]
        public void CalcRectDuctEquivDiameterTest()
        {
            // Arrange
            var expected = Math.Sqrt(4 / Math.PI * DimA * DimB);

            // Act
            var actual1 = PhysicsCommon.CalcEquivDiameter(DimA, DimB);
            var actual2 = PhysicsCommon.CalcEquivDiameter(DimA, ZeroValue);

            // Assert
            Assert.Equal(expected, actual1);
        }

        [Theory]
        [InlineData(ZeroValue, DimA)]
        [InlineData(ZeroValue, ZeroValue)]
        [InlineData(DimA, NegativeValue)]
        [InlineData(NegativeValue, DimA)]
        [InlineData(NegativeValue, NegativeValue)]
        [InlineData(DimA, MaxDimExceed)]
        [InlineData(MaxDimExceed, DimA)]
        public void CalcRectDuctEquivDiameterRxceptionsTest(double value1, double value2)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => PhysicsCommon.CalcEquivDiameter(value1, value2));
        }

        [Fact]
        public void CalcHydrDiameterTest()
        {
            // Arrange
            var expected1 = 4 * (DimA * DimB) / (2 * DimA + 2 * DimB);
            var expected2 = DimA;

            // Act
            var actual1 = PhysicsCommon.CalcHydrDiameter(DimA, DimB);
            var actual2 = PhysicsCommon.CalcHydrDiameter(DimA, ZeroValue);

            // Assert
            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
        }

        [Theory]
        [InlineData(ZeroValue, DimA)]
        [InlineData(ZeroValue, ZeroValue)]
        [InlineData(DimA, NegativeValue)]
        [InlineData(NegativeValue, DimA)]
        [InlineData(NegativeValue, NegativeValue)]
        [InlineData(DimA, MaxDimExceed)]
        [InlineData(MaxDimExceed, DimA)]
        public void CalcHydrDiameterExceptionsTest(double value1, double value2)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => PhysicsCommon.CalcHydrDiameter(value1, value2));
        }

        [Fact]
        public void CalcHydrRadiusTest()
        {
            // Arrange
            var expected1 = (DimA * DimB) / (2*DimA + 2*DimB);
            var expected2 = DimA/4;     
            //It may be surprising, but yes: hydraulic radius is half of geometrical radius. This is why: d/4.
            var expected3 = PhysicsCommon.CalcHydrDiameter(DimA, DimB) / 4;
            var expected4 = PhysicsCommon.CalcHydrDiameter(DimA, ZeroValue) / 4;

            // Act
            var actual1 = PhysicsCommon.CalcHydrRadius(DimA, DimB);
            var actual2 = PhysicsCommon.CalcHydrRadius(DimA, ZeroValue);

            // Assert
            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
            Assert.Equal(expected3, actual1);
            Assert.Equal(expected4, actual2);
        }

        [Theory]
        [InlineData(ZeroValue, DimA)]
        [InlineData(ZeroValue, ZeroValue)]
        [InlineData(DimA, NegativeValue)]
        [InlineData(NegativeValue, DimA)]
        [InlineData(NegativeValue, NegativeValue)]
        [InlineData(DimA, MaxDimExceed)]
        [InlineData(MaxDimExceed, DimA)]
        public void CalcHydrRadiusExceptionsTest(double value1, double value2)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => PhysicsCommon.CalcHydrRadius(value1, value2));
        }

        [Fact]
        public void CalcOutLwFromSourceDaTest() {

            // Arrange
            var inLw = 95;
            var srcDa1 = 12;
            var srcDa2 = 98;
            var expected1 = 83;
            var expected2 = Defaults.LwMin;

            // Act
            var result1 = PhysicsCommon.CalcOutLwFromSourceDa(inLw, srcDa1);
            var result2 = PhysicsCommon.CalcOutLwFromSourceDa(inLw, srcDa2);

            // Assert
            Assert.Equal(result1,expected1);
            Assert.Equal(result2,expected2);
        }

        [Theory]
        [InlineData(NegativeValue, SrcLw)]
        [InlineData(SrcDa, NegativeValue)]
        public void CalcOutLwFromSourceDaExceptionTest(double inLw, double inDa) 
        {
            // Assert
            Assert.ThrowsAny<Exception>(()=> PhysicsCommon.CalcOutLwFromSourceDa(inLw,inDa));
        }

        [Fact]
        public void CalcuOutLwFromSourceLwDaTest()
        {
            // Arrange
            double[] inLwArr = { 92.0, 90.0, 88.0, 84.0, 79.0, 74.0, 67.0, 60.0 };
            double[] srcLwArr = { 58.0, 54.0, 49.0, 45.0, 41.0, 37.0, 34.0, 31.0 };
            double[] srcDaArr = { 6.0, 17.0, 34.0, 36.0, 38.0, 29.0, 19.0, 15.0 };
            var expected = new[] { 86.00687765, 73.05433314, 55.19331048, 49.76434862, 44.01029996, 45.63892034, 48.16954289, 45.16954289 };
            var minAccuracy = 0.00000001;

            // Act
            var result = PhysicsCommon.CalcuOutLwFromSourceLwDa(inLwArr, srcLwArr, srcDaArr);
            Assert.True(result.EqualsToPrecision(expected, minAccuracy));
            Assert.DoesNotContain(result, x => x < 0);
        }

        [Fact]
        public void CalcOutLwFromSourceLwDaExceptionsTest() 
        {
            // Arrange
            double[] inLwArr = { 92.0, 90.0, 88.0, 84.0, 79.0, 74.0, 67.0, 60.0 };
            double[] srcLwArr = { 58.0, 54.0, 49.0, 45.0, 41.0, 37.0, 34.0, 31.0 };
            double[] srcDaArr = { 6.0, 17.0, 34.0, 36.0, 38.0, 29.0, 19.0, 15.0 };

            Assert.ThrowsAny<Exception>(() => PhysicsCommon.CalcuOutLwFromSourceLwDa(null, srcLwArr, srcDaArr));
            Assert.ThrowsAny<Exception>(() => PhysicsCommon.CalcuOutLwFromSourceLwDa(inLwArr, null, srcDaArr));
            Assert.ThrowsAny<Exception>(() => PhysicsCommon.CalcuOutLwFromSourceLwDa(inLwArr, srcLwArr, null));
        }

        [Fact]
        public void CalcTotalLwOktGessamntTests() 
        {
            // Arrange
            var terminalHydr = HydraulicsTerminal.Create(0.25, 10.0, 20.0, 0.3);
            var expected = 1.6283078; 
            var highRange = expected + Accuracy;
            var lowRange = expected - Accuracy;

            // Act
            var result = PhysicsCommon.CalcTotalLwOktGessamnt(terminalHydr, Func);

            // Assert
            Assert.InRange(result, lowRange, highRange);
        }

        [Fact]
        public void CalcLwOktTests() 
        {
            // Arrange
            var terminalHydr = HydraulicsTerminal.Create(0.25, 10.0, 20.0, 0.3);
            var expected63 = 23.10160133;
            var expected8000 = 0.0;
            var lw = 29.2731749;

            // Act
            var resut63 = PhysicsCommon.CalcLwOkt(terminalHydr, Func, lw, 63);
            var resut8000 = PhysicsCommon.CalcLwOkt(terminalHydr, Func, lw, 8000);

            // Assert
            Assert.InRange(resut63, expected63 - Accuracy, expected63 + Accuracy);
            Assert.Equal(resut8000, expected8000);
        }

        [Fact]
        public void CalcLwOktArrayTests()
        {
            // Arrange
            var terminalHydr = HydraulicsTerminal.Create(0.25, 10.0, 20.0, 0.3);
            var lw = 29.2731749;
            var expected = new[] { 23.10160133, 22.88552897, 22.42348610, 21.45424554, 19.46894159, 15.59392862, 8.69587612, 0.0 };

            // Act
            var result = PhysicsCommon.CalcLwOktArray(terminalHydr,Func, lw);

            // Assert
            Assert.True(result.EqualsToPrecision(expected, Accuracy));
        }

        [Fact]
        public void CalcLwOktArrayExceptionsTests()
        {
            // Arrange
            var terminalHydr = HydraulicsTerminal.Create(0.25, 10.0, 20.0, 0.3);
            HydraulicsTerminal hydrNull = null;
            var lw = 29.2731749;

            // Assert
            Assert.ThrowsAny<Exception>(() => PhysicsCommon.CalcLwOktArray(terminalHydr, Func, NegativeValue));
            Assert.ThrowsAny<Exception>(() => PhysicsCommon.CalcLwOktArray(terminalHydr, null, lw));
            Assert.ThrowsAny<Exception>(() => PhysicsCommon.CalcLwOktArray(hydrNull, Func, lw));
        }

        [Fact]
        public void CalcLogSumTests() 
        {
            // Arrange
            var expected = 34.7712125471966;

            // Act
            var actual = PhysicsCommon.CalcLogSum(30.0,30.0,30.0);

            // Assert
            Assert.Equal(expected, actual, 2);

        }

    }
}

