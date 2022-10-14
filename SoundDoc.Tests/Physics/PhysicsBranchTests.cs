using NoiseAnalyzer.Core.Utils;
using SoundDoc.Core.Physics;
using Xunit;

namespace SoundDoc.Tests.Physics
{
    public class PhysicsBranchTests
    {
        // Arrange (Shared)
        const double ZeroValue = 0.0;
        const double NegativeValue = -0.6;       // generic negative value for all cases
        const double MaxDimExeeded = Defaults.DimMax + 0.01;
        const double Dim1 = 0.6;                 // dimension size [m]
        const double Dim2 = 0.3;
        const double Vol1 = 0.50;                // airflow [m3/s]
        const double Area1 = 0.25;               // area [m2]
        const double FmValue = 500.0;            // center band octave 500 [Hz]
        const double HydrDiam1 = 0.15;           // hydraulic diameter [m]
        const double Velocity = 2.0;             // air velocity [m/s]

        [Fact]
        public void CalculateStNumberTest()
        {
            // Act
            var actual = PhysicsBranch.CalcStrouhalNum(HydrDiam1, Velocity, FmValue);

            // Assert
            Assert.Equal(37.5, actual);
        }
    }
}
