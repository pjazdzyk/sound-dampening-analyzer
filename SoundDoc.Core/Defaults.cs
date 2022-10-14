
using SoundDoc.Abstractions.Models;

namespace SoundDoc.Core
{
    public static class Defaults
    {
        public const double LwMin = 0.0;                // minimum generated noise level [dB]
        public const double LwBackr = 10.0;             // default acoustic bacground noise level [dB]
        public const double LwSrc = 90.0;               // default noise level [dB]
        public const double LwMax = 220.0;              // maximum noise level limit[dB]
        public const double FlowIn = 0.1;               // default volumetric flowrate [m3/s]
        public const double FlowBr = FlowIn / 2.0;      // default flow in branch [m3/s]
        public const double DimSize = 0.5;              // default duct size [m]
        public const double DimMax = 20.0;              // maximum duct size [m]
        public const double AngleMin = 0.0;             // minimum branch / bend angle [deg] 
        public const double AngleMax = 90.0;            // maximum branch / bend anglea [deg]
        public const double Lentgh = 1.5;               // default duct length [m]
        public const double FiletRad = 0.01;            // default filet radius [m]
        public const double StrouhalMin = 1.0;          // minimum acceptable Strouhal number value [-]
        public const double DamperDischargeCoef = 10.0; // default damper discharge coeficient (Dzeta) [-]
        public const double DamperBladeHeight = 0.08;   // default damper blade height [m]
        public const double TerminalDeltaP = 25.0;      // default terminal pressure drop [Pa]
        public const double FanDeltaP = 10000.0;        // external static pressure for a typical fan [Pa]
        public const double FanRPM = 1500.0;            // default fan revolution per minute [r/min]
        public const double FanSpecLw = 35.0;           // default specific fan sound power level [dB]
        public const double SoundSpeedC = 340.0;        // default speed of sound in air, [m/s]

        public static double[] EmptyOctaveArray => new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
        public static double[] InputLwArray => new double[] { LwSrc, LwSrc, LwSrc, LwSrc, LwSrc, LwSrc, LwSrc, LwSrc };     // default sound power spectrum [dB]
        public static double[] InputLwMinAray => new double[] { LwMin, LwMin, LwMin, LwMin, LwMin, LwMin, LwMin, LwMin };   // minimum sound power spectrum [dB]
        public static double[] TestSourceDa => new double[] { 6.0, 17.0, 34.0, 36.0, 38.0, 29.0, 19.0, 15.0 };              // test damperning array [dB] 
        public static double[] TestSourceLw => new double[] { 58.0, 54.0, 49.0, 45.0, 41.0, 37.0, 34.0, 31.0};              // test source noise array [dB]
        public static double[] OctCenterBand => new double[] { 63.0, 125.0, 250.0, 500.0, 1000.0, 2000.0, 4000.0, 8000.0 }; // octave center band array (fm) [Hz]
        public static double[] OctBandWidth => new double[] { 45.0, 88.0, 177.0, 354.0, 707.0, 1414.0, 2828.0, 5657.0 };    // octave wide band array (delta_f) [Hz]
        
        public static int NumberOfOctaves = OctCenterBand.Length;       // octave arrays length
       
        public static readonly string Name = "New Unnmamed Item";       // default element name
        public static readonly string Material = "blachaOcynk";         // default duct material DB key
        public static readonly string BendKey = "rectDuct";             // default bend type DB key
        public const string FanTypeKey = "HP_RG";                       // default fan type DB key
        public static Material MaterialDef => new Material { Name = "murBetonSur", Description = "Beton surowy", ValuesArray = new[] { 0.010, 0.010, 0.012, 0.016, 0.019, 0.023, 0.035, 0.035 } };
    }
}
