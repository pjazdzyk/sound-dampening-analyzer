using SoundDoc.Core.Data;
using System;
using SoundDoc.Core.Data.HydraulicModels;

namespace SoundDoc.Core.Physics
{
    /**
    * THE PHYSICS OF ACOUSTICS EQUATIONS LIBRARY - FAN
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

    public class PhysicsFan
    {
        // [1] [St,_] (20) <28> - Fan dimensionles frequencu (Strouhal number)
        public static double CalcFanStrouhalNum(HydraulicsFan hydraulics, double fm)
        {
            double stNum = fm * 60 / (Math.PI * hydraulics.FanRPM);

            return stNum < Defaults.StrouhalMin
                ? Defaults.StrouhalMin
                : stNum;
        }

        // [1] [ΔLw.Okt, dB] (21) <28> - Fan octave band correction
        public static double CalcFanDeltaLwOkt(HydraulicsFan hydraulics, double fm) 
        {
            double st = CalcFanStrouhalNum(hydraulics,fm);
            double calcFactor = hydraulics.FanType.CalculationFactor;

            return -5 - 5 * Math.Pow((Math.Log10(st) + calcFactor),2);
        }

        // [1] [Lw, dB] (19) <26> - Fan noise total sound power
        public static double CalcFanTotalLw(HydraulicsFan hydraulics)
        {
            double lwsSpecPwr = hydraulics.FanType.Lws;

            return lwsSpecPwr + 10 * Math.Log10(hydraulics.InVolFlow / 1)
                + 20 * Math.Log10(hydraulics.DeltaP / 1);
        }

        // [1] [Lw, dB] (23) <29> - Fan noise sound power spectrum
        public static double[] CalcFanSourceLw(HydraulicsFan hydraulics) 
        {
            return PhysicsCommon.CalcLwOktArray(hydraulics, CalcFanDeltaLwOkt, CalcFanTotalLw(hydraulics));
        }
    }
}
