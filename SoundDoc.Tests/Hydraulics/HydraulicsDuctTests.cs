using System;
using SoundDoc.Core.Data;
using SoundDoc.Core.Data.HydraulicModels;
using Xunit;
using NoiseAnalyzer.Core.Utils;

namespace SoundDoc.Tests
{
    public class HydraulicsDuctTests
    {
        const double Dim1 = 0.2;
        const double Dim2 = 0.5;
        const double Len = 2.5;
        const double VolFlow = 0.8;
        const double ZeroValue = 0.0;
        const double NegativeValue = -0.5;

        [Fact]
        public void HydraulicsDuctConstructorTests()
        {
            // Arrange
            var expectedLengthDef = Defaults.Lentgh;
            var expectedLength = Len;

            // Act
            var hydraulicsDef = HydraulicsDuct.FromDefaults();
            var hydraulics = HydraulicsDuct.Create(VolFlow,Len,Dim1,Dim2);
            var actLenDef = hydraulicsDef.InLength;
            var actLen = hydraulics.InLength;

            // Assert
            Assert.NotNull(hydraulicsDef);
            Assert.NotNull(hydraulics);
            Assert.Equal(expectedLengthDef, actLenDef);
            Assert.Equal(expectedLength, actLen);
        }

        [Theory]
        [InlineData(ZeroValue)]
        [InlineData(NegativeValue)]
        public void HydraulicsDuctConstructorExceptionTests(double length) 
        {
            // Assert
            Assert.Throws<ArgumentException>(()=> HydraulicsDuct.Create(VolFlow, length, Dim1, Dim2));
        }

        [Fact]
        public void HydraulicsDuctSetDuctLengthTests()
        {
            // Arrange
            var expected = Len;

            // Act
            var hydraulics = HydraulicsDuct.FromDefaults();
            hydraulics.SetDuctLength(Len);
            var angle = hydraulics.InLength;

            // Assert
            Assert.Equal(expected, angle);
        }

        [Theory]
        [InlineData(ZeroValue)]
        [InlineData(NegativeValue)]
        public void HydraulicsDuctSetDuctLengthExceptionTests(double length) 
        {
            // Act
            var hydraulics = HydraulicsDuct.FromDefaults();

            // Assert
            Assert.Throws<ArgumentException>(()=>hydraulics.SetDuctLength(length));
        }

        }
}
