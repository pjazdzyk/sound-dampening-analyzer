using NoiseAnalyzer.Core.Utils;

namespace SoundDoc.Tests.Physics
{
    public class PhysicsDuctTests
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
    }
}