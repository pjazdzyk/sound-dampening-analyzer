using SoundDoc.Core.Data;
using System;
using SoundDoc.Core.Data.HydraulicModels;
using SoundDoc.Core.Physics;
using static SoundDoc.Core.Physics.PhysicsCommon;

namespace SoundDoc.Core.Physics
{
    /**
   * THE PHYSICS OF ACOUSTICS EQUATIONS LIBRARY - BRANCHES
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

    public class PhysicsBranch
    {
        /* ----------------DUCT BRANCH OR BEND ACOUSTIC-RELATED EQUATIONS---------------------- */

        /* --- BRANCH NOISE GENERATION --- */

        // [1] [St,-] (n/a) <38> - Branch or Bend dimensionless frequency: Strouhal Number
        public static double CalcStrouhalNum(double branchHydrDiameter, double branchAirVelocity, double fm)
        {
            if (branchAirVelocity < 0 || fm <= 0 || branchHydrDiameter <= 0)
                throw new ArgumentException(nameof(CalcStrouhalNum) + " Blad wartosci: "
                                           + nameof(branchHydrDiameter) + " = " + branchHydrDiameter + " "
                                           + nameof(fm) + " = " + fm + " "
                                           + nameof(branchAirVelocity) + " = " + branchAirVelocity);

            double strouhalNum = fm * branchHydrDiameter / branchAirVelocity;

            return strouhalNum < Defaults.StrouhalMin
                ? Defaults.StrouhalMin
                : strouhalNum;
        }

        // [1] [Lw*, dB] (29) <38> - Branch or Bend normalized sound power level
        public static double CalcNormalizedLw(double inVelocity, double brVelocity, double strouhalNumber)   
        {
            return 12 - 21.5 * Math.Pow(Math.Log10(strouhalNumber), 1.268) + (32 + 13 * Math.Log10(strouhalNumber)) * Math.Log10(inVelocity / brVelocity);
        }

        // [1] [-] (30) <38> - Branch or Bend correction value (effect of fillet radius on flow noise)
        public static double CalcBranchFiletKFactor(HydraulicsBranch hydraulics, double strouhalNumber)
        {
            return 13.9 * (3.43 - Math.Log10(strouhalNumber)) * (0.15 - hydraulics.BrFiletRad / hydraulics.BrEquivD);
        }

        // [1] [Lw, dB] (28) <38> - Branch or Bend noise sound power for a given fm
        public static double CalcLw(double brEquivD, double brVelocity, double normLw, double factorK, double freqBandWidth)
        {
            if (freqBandWidth < 0)
                throw new ArgumentException();

            double result = normLw + 10 * Math.Log10(freqBandWidth / 1) + 30 * Math.Log10(brEquivD / 1) + 50 * Math.Log10(brVelocity / 1) + factorK;

            return result < Defaults.LwMin
                  ? Defaults.LwMin
                  : result;
        }

        // [1] [Lw[], dB] (28) <38> - Branch or Bend noise sound power spectrum
        public static double[] CalcLwArray<THydraulics> (THydraulics hydraulics, FunctionHydrVal<THydraulics> filetFunction) where THydraulics : HydraulicsBend
        {
            double[] tempSourceLw = Defaults.InputLwMinAray;

            if (hydraulics.BrVelocity == 0 || hydraulics.BrEquivD == 0) 
            {
                return tempSourceLw;
            } 

            for (int i = 0; i < Defaults.NumberOfOctaves; i++)
            {
                var freqBandwth = Defaults.OctBandWidth[i];
                var fm = Defaults.OctCenterBand[i];
                var strouhalNumber = CalcStrouhalNum(hydraulics.BrEquivD, hydraulics.BrVelocity, fm);
                var normLw = CalcNormalizedLw(hydraulics.InVelocity, hydraulics.BrVelocity, strouhalNumber);
                var filetKFactor = filetFunction(hydraulics, strouhalNumber);

                tempSourceLw[i] = CalcLw(hydraulics.BrEquivD, hydraulics.BrVelocity, normLw, filetKFactor, freqBandwth);
            }

            return tempSourceLw;
        }

        // [n/a] [Lw[], dB] (n/a) <n/a> - Branch noise sound power spectrum
        public static double[] CalcBranchSourceLw(HydraulicsBranch hydraulics) 
        {
            return CalcLwArray(hydraulics, CalcBranchFiletKFactor);
        }

        // [n/a] [Lw[], dB] (n/a) <n/a> - Bend noise sound power spectrum
        public static double[] CalcBendSourceLw(HydraulicsBend hydraulics)
        {
            return CalcLwArray(hydraulics, (hydraulics, CalcStrouhalNum) => 0.0 );
        }

        public static double CalcLWBranch(HydraulicsBranch hydraulics)
        {
            return Math.Abs(10 * Math.Log(hydraulics.BrArea / (hydraulics.BrArea + hydraulics.OutArea)));
        }

    }
}
