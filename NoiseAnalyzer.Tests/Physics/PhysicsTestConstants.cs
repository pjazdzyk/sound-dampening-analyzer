using NoiseAnalyzer.Core.Utils;

namespace NoiseAnalyzer.Tests.Physics
{
    public class PhysicsTestConstants
    {
        public const double ZeroValue = 0.0;
        public const double NegativeValue = -0.6;                      // generic negative value for all cases
        public const double MaxDimExeeded = Defaults.DimMax + 0.01;    // Value which exceeding maximum dimension size
        public const double Dim1 = 0.6;                                // dimension size [m] (rectangular side A or diameter)
        public const double Dim2 = 0.3;                                // dimension size [m] (rectangular side B)
        public const double Vol1 = 0.50;                               // airflow [m3/s]
        public const double Area1 = 0.25;                              // area [m2]
        public const double FmValue = 500.0;                           // center band octave 500 [Hz]
        public const double HydrDiam1 = 0.15;                          // hydraulic diameter [m]
        public const double Velocity = 2.0;                            // air velocity [m/s]
        public const double SrcDa = 3.5;                               // source dampening   
        public const double SrcLw = Defaults.LwSrc;                    // source noise
        public const double Accuracy = 0.0000001;                      // aceptable accuracy for double equality check
    }
}
