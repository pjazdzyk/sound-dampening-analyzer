using SoundDoc.Abstractions.Models;
using SoundDoc.Core.Data;
using System;
using System.Linq;
using SoundDoc.Core.Data.HydraulicModels;

namespace SoundDoc.Core.Physics
{
    /**
     * THE PHYSICS OF ACOUSTICS EQUATIONS LIBRARY - DUCTS
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

   public class PhysicsDuct
    {
        /* ----------------STRAIGHT DUCT ACOUSTIC-RELATED EQUATIONS---------------------- */

        /* --- DUCT NOISE GENERATION --- */

        // [1] [Lw, dB] (24) <36> - Rectangular or circular duct total flow noise independent from length
        public static double CalcDuctTotalLw(HydraulicsDuct hydraulics)
        {
            return 16.5 + 48.2 * Math.Log10(hydraulics.InVelocity / 1) + 10 * Math.Log10(hydraulics.InArea / 1);
        }

        // [1] [∆LwOkt, dB] (25) <37> - Rectangular or circular duct octave band correction
        public static double CalcDuctDeltaLwOkt(HydraulicsDuct hydraulics, double fm)
        {
            return -6.24 - 21.75 * Math.Log10(0.228 + 0.094 * (fm / hydraulics.InVelocity) * 1 / 1);
        }

        // [1] [LwOkt[], dB] (27) <37> - Rectangular or circular duct sound power spectrum
        public static double[] CalcDuctSourceLw(HydraulicsDuct hydraulics)
        {
            return PhysicsCommon.CalcLwOktArray(hydraulics, CalcDuctDeltaLwOkt, CalcDuctTotalLw(hydraulics));
        }

        /* --- DUCT NOISE DAMPENING --- */

        // [3] [∆L1, dB] (13.7) <717> - Rectangular or circular duct sound dampening
        public static double CalcDuctDeltaL1(HydraulicsDuct hydraulics, double absorptionCoef)
        {
            return 1.09 * absorptionCoef * (hydraulics.InLength / hydraulics.InHydrR);
        }

        // [3] [∆L1[], dB] (13.7) <717> - Rectangular or circular duct sound dampening spectrum
        public static double[] CalcDuctSourceDa(HydraulicsDuct hydraulics, Material ductMaterial) 
        { 
            return ductMaterial.ValuesArray.Select(dampCoef => CalcDuctDeltaL1(hydraulics, dampCoef)).ToArray();
        }
    }
}
