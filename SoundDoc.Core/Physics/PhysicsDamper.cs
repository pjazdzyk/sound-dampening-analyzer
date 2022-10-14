using MathNet.Numerics;
using SoundDoc.Core.Data;
using System;
using SoundDoc.Core.Data.HydraulicModels;


namespace SoundDoc.Core.Physics
{
     /**
     * THE PHYSICS OF ACOUSTICS EQUATIONS LIBRARY - DAMPER
     * MAIN REFERENCE STANDARD: VDI 2081 Part 1 / 2019: "Noise generation and noise reduction"
     * STANDARD LEGAL COPY ORDER NUMBER: W2228997
     * LIBRARY FIRST ISSUE DATE: 2021.03
     * 
     * SOURCE PUBLICATIONS:
     * [1] - VDI 2081 Part 1 / 2019: "Noise generation and noise reduction"
     * [2] - https://www.engineeringtoolbox.com/hydraulic-diameter-rectangular-ducts-d_1004.html
     * [3] - A.Pełech. Wentylacja i klimatyzacja - podstawy. ISBN: 978-83-7493-780-1.
     * 
     * LEGEND KEY:
     * [reference no] [value symbology in standard, unit] (euquation number) <page> - Description
     */

    public class PhysicsDamper
    {
        // [1] [K,_] (Table 12) <44> - Polynominal coefficients for blade heights
        public static double[] GetPolyCoefOfBladeHeights(double bladeHeight)
        {
            return (bladeHeight >= 0.130 && bladeHeight <= 0.200)
                ? new double[] { 23.2, -46.08, 41.82, -17.03, 3.26, -0.182 }
                : new double[] { 15.7, -35.48, 43.76, -26.01, 7.937, -0.907 };
        }

        // [1] [Z,-] (37) <43> - Dimensionles frequency coeficient "Z"
        public static double CalcDamperZCoef(HydraulicsDamper hydraulics, double fm)
        {
            return fm / hydraulics.InVelocity * Math.Pow(hydraulics.DischargeCoefficient, 0.3);
        }

        // [1] [Z,-] (35,36) <43> - Total sound power level of multi-blade dampers
        public static double CalcDamperTotalLw(HydraulicsDamper hydraulics)
        {
            double bladeFactor = (hydraulics.DamperType == HydraulicsDamper.TypeOpposedBlades) ? 22 : 28;
                        
            return 10 + 60 * Math.Log10(hydraulics.InVelocity)
                + bladeFactor * Math.Log10(hydraulics.DischargeCoefficient + 1)
                + 10 * Math.Log10(hydraulics.InArea / 1);
        }

        // [1] [ΔLwOkt,dB] (37) <43> - Damper Octave band correction
        public static double CalcDeltaLwOkt(HydraulicsDamper hydraulics, double fm)
        {
            double z = CalcDamperZCoef(hydraulics, fm);
            double[] k = GetPolyCoefOfBladeHeights(hydraulics.BladeHeight);
            return -(k[0]
                   + k[1] * Math.Log10(z)
                   + k[2] * Math.Pow((Math.Log10(z)), 2)
                   + k[3] * Math.Pow((Math.Log10(z)), 3)
                   + k[4] * Math.Pow((Math.Log10(z)), 4)
                   + k[5] * Math.Pow((Math.Log10(z)), 5));
        }

        // [1] [LwOkt,dB] (39) <44> - Damper noise sound power spectrum
        public static double[] CalcDamperSourceLw(HydraulicsDamper hydraulics) 
        {
            return PhysicsCommon.CalcLwOktArray(hydraulics, CalcDeltaLwOkt, CalcDamperTotalLw(hydraulics));
        }

        public static class ChartsData
        {
            /*OPPOSED BLADES */
            //Part1: 3rd order polynomial regression: LIMITS:<0.0, 20.0>
            private static double[] XData1 = { 0.0, 5.0, 7.5, 10.0, 15.0, 20.0 };
            private static double[] YData1 = { 0.2, 0.25, 0.29, 0.37, 0.6, 1.0 };
            private static Func<double, double> Function1 = Fit.PolynomialFunc(XData1, YData1, 3);

            //Part2:  power regression, LIMITS: <20.0, 60.0>
            private static double[] XData2 = { 20.0, 30.0, 40.0, 60 };
            private static double[] YData2 = { 1.0, 3.2, 10.0, 100 };
            private static Func<double, double> Function2 = Fit.PowerFunc(XData2, YData2);

            //Part3:  power regression, LIMITS: <60.0, 80.0>
            private static double[] XData3 = { 60.0, 65.0, 70.0, 75.0, 80.0 };
            private static double[] YData3 = { 100.0, 200.0, 400.0, 900.0, 1850.0 };
            private static Func<double, double> Function3 = Fit.PowerFunc(XData3, YData3);

            //Part4: 3rd order polynomial regression, LIMITS: <80.0, 84.0>
            private static double[] XData4 = { 80.0, 81.0, 82.0, 83.0, 84.0, 85.0 };
            private static double[] YData4 = { 1850.0, 2000.0, 2400.0, 3000.0, 3800.0, 5000.0 };
            private static Func<double, double> Function4 = Fit.PolynomialFunc(XData4, YData4, 2);

            // [1] [Z,-] (F12) <43> - Terminal opposed blades zeta coeficient calculation 
            public static double CalcOpposedBladesZetaCoef(double openingAngle)
            {
                if (openingAngle < 0)
                    throw new ArgumentException(nameof(openingAngle) + "Damper blades angle cannot be negative value");

                if (openingAngle <= 20)
                    return Function1.Invoke(openingAngle);
                if (openingAngle > 20 && openingAngle <= 60)
                    return Function2.Invoke(openingAngle);
                if (openingAngle > 60 && openingAngle <= 80)
                    return Function3.Invoke(openingAngle);
                if (openingAngle > 80 && openingAngle <= 85)
                    return Function4.Invoke(openingAngle);

                throw new ArgumentException(nameof(openingAngle) + " = " + openingAngle + " Damper blades angle exeeds 85deg.");

            }
        }
    }
}
