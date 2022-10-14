using System;
using SoundDoc.Core.Data;
using SoundDoc.Core.Data.HydraulicModels;
using Xunit;
using NoiseAnalyzer.Core.Utils;

namespace SoundDoc.Tests
{
    public class HydraulicsBendTests
    {
        const double Dim1 = 0.2;
        const double Dim2 = 0.5;
        const double MaxAngle = Defaults.AngleMax;
        const double TestAngle = 45;
        const double VolFlow = 0.8;
        const double NegativeValue = -0.5;

        [Fact]
        public void HydraulicsBendConstructorTests()
        {
            // Arrange
            var expectedAnglehDef = Defaults.AngleMax;
            var expectedAngle = TestAngle;

            // Act
            var hydraulicsDef = HydraulicsBend.FromDefaults();
            var hydraulics = HydraulicsBend.Create(VolFlow, TestAngle, Dim1, Dim2);
            var actLenDef = hydraulicsDef.BrAngle;
            var actLen = hydraulics.BrAngle;

            // Assert
            Assert.NotNull(hydraulicsDef);
            Assert.NotNull(hydraulics);
            Assert.Equal(expectedAnglehDef, actLenDef);
            Assert.Equal(expectedAngle, actLen);
        }

        [Theory]
        [InlineData(NegativeValue)]
        [InlineData(MaxAngle+1)]
        public void HydraulicsBendConstructorExceptionTests(double angle)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => HydraulicsBend.Create(VolFlow, angle, Dim1, Dim2));
        }

        [Fact]
        public void HydraulicsBendSetAngleTest() 
        {
            // Arrange
            var expected = TestAngle;
            
            // Act
            var hydraulics = HydraulicsBend.FromDefaults();
            hydraulics.SetBranchAngle(TestAngle);
            var angle = hydraulics.BrAngle;

            // Assert
            Assert.Equal(expected,angle);
        }

        [Theory]
        [InlineData(NegativeValue)]
        [InlineData(MaxAngle + 1)]
        public void HydraulicsSetBendAngleExceptionTest(double angle)
        {
            // Act
            var hydraulics = HydraulicsBend.FromDefaults();

            // Assert
            Assert.Throws<ArgumentException>(() => hydraulics.SetBranchAngle(angle));
        }

    }
}
