using System;
using System.Linq;
using SoundDoc.Core.Data;
using SoundDoc.Core.Data.HydraulicModels;
using SoundDoc.Core.Extensions;

namespace SoundDoc.Core.Physics
{
    /**
   * THE PHYSICS OF ACOUSTICS EQUATIONS LIBRARY - COMMON
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

    public class PhysicsCommon
    {
        public static readonly string Version = "1.00";
        
        /* ---COMMON GEOMETRY EQUATIONS--- */

        // Checks for valid dimensions or throws exception
        public static void CheckDimensions(double dim1, double dim2)
        {
            if (dim1 <= 0 || dim2 < 0 || dim1 > Defaults.DimMax || dim2 > Defaults.DimMax)
                throw new ArgumentException(nameof(CheckDimensions) + " Blad obliczen. Niepoprawne wymiary. " + nameof(dim1) + "=" + dim1 + " "
                                                                                                                   + nameof(dim2) + "=" + dim2);
        }

        // [n/a] [S, m2] (n/a) <n/a> - Rectangular or circular duct crossection area
        public static double CalcDuctSecArea(double dim1, double dim2 = 0)
        {
            CheckDimensions(dim1, dim2);

            return dim2 == 0
                ? Math.PI * dim1 * dim1 / 4
                : dim1 * dim2;
        }

        // [n/a] [P, m] (n/a) <n/a> - Rectangular or circular duct perimeter
        public static double CalcDuctSecPerimeter(double dim1, double dim2 = 0)
        {
            CheckDimensions(dim1, dim2);

            return dim2 == 0
                ? 2 * Math.PI * dim1 / 2
                : 2 * dim1 + 2 * dim2;
        }

        // [n/a] [v, m/s] (n/a) <n/a> - Flow velocity inside ventilation duct
        public static double CalcAirVelocity(double ductArea, double volFlow)
        {
            if (ductArea <= 0 || volFlow < 0)
                throw new ArgumentException(nameof(CalcAirVelocity) + "Blad wartosci: " + nameof(ductArea) + "=" + ductArea + " "
                                                                                          + nameof(volFlow) + "=" + volFlow);
            return volFlow / ductArea;
        }

        // [1] [dg,m] (n/a) <n/a> - Rectangular duct equivalent diameter
        public static double CalcEquivDiameter(double dim1, double dim2 = 0)
        {
            double ductSecArea = CalcDuctSecArea(dim1, dim2);

            return dim2 == 0
                ? dim1
                : Math.Sqrt(4 / Math.PI * ductSecArea);
        }

        // [2] [dh,m] (n/a) <40> - Rectangular or circular duct hydraulic diameter equation       
        public static double CalcHydrDiameter(double dim1, double dim2 = 0)
        {
            double ductSecArea = CalcDuctSecArea(dim1, dim2);
            double ductSecPerimeter = CalcDuctSecPerimeter(dim1, dim2);

            return 4 * ductSecArea / ductSecPerimeter;
        }

        // [n/a] [rh,m] (n/a) <n/a> - Rectangular or circular duct hydraulic radius
        public static double CalcHydrRadius(double dim1, double dim2 = 0)
        {
            double ductSecArea = CalcDuctSecArea(dim1, dim2);
            double ductSecPerimeter = CalcDuctSecPerimeter(dim1, dim2);

            return ductSecArea / ductSecPerimeter;
        }

        /* ---COMMON ACOUSTIC EQUATIONS--- */

        // Logharitmic sum of any number of values
        public static double CalcLogSum(params double[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values) + " Null passed for values argument");

            var logSumResult = 0.0;

            foreach (double value in values)
                logSumResult = logSumResult.LogSum(value);

            return logSumResult;
        }

        // Calculates resulting sound power level for a single value including sound dampening only
        public static double CalcOutLwFromSourceDa(double inputLwValue, double sourceDaValue)
        {
            if (inputLwValue < 0 || sourceDaValue < 0)
                throw new ArgumentException(nameof(inputLwValue) + "= " + inputLwValue + nameof(sourceDaValue) + 
                                            "=" + sourceDaValue + " Input values cannot be negative.");

            return sourceDaValue > inputLwValue
                ? Defaults.LwMin
                : inputLwValue - sourceDaValue;
        }

        // Calculates resulting sound power level including provided element internal noise and dampening effect (returns array)
        public static double[] CalcuOutLwFromSourceLwDa(double[] inputLw, double[] sourceLw, double[] sourceDa)
        {
            if (inputLw == null)
                throw new ArgumentNullException(nameof(inputLw));
            if (sourceLw == null)
                throw new ArgumentNullException(nameof(sourceLw));
            if (sourceDa == null)
                throw new ArgumentNullException(nameof(sourceDa));

            var outLw = Defaults.EmptyOctaveArray;

            for (int i = 0; i < inputLw.Length; i++) 
                outLw[i] = (inputLw[i] - sourceDa[i]).LogSum(sourceLw[i]);

            return outLw;

        }

        /* ---COMMON FUNCTIONAL / GENERIC METHODS FOR IMPLEMENTATION OF EQUATIONS NO 33 AND 34--- */

        // Delegate function: takes Hydraulics and double value, returns double
        public delegate double FunctionHydrVal<THydraulics>(THydraulics hydraulics, double value) where THydraulics : Hydraulics;

        // Delegate function: takes Hydraulics, return double
        public delegate double FunctionHydr<THydraulics>(THydraulics hydraulics) where THydraulics : Hydraulics;

        // [1] [∆LwOkt.Gessamt, dB] (26) <37> - Total correction of noise sound power
        public static double CalcTotalLwOktGessamnt<THydraulics>(THydraulics hydraulics, FunctionHydrVal<THydraulics> deltaLwOktFun) where THydraulics : Hydraulics
        {
            double tempSum = Defaults.OctCenterBand.Select(fm => Math.Pow(10, 0.1 * deltaLwOktFun(hydraulics, fm))).Sum();
            return 10 * Math.Log10(tempSum);
        }

        // [1] [LwOkt, dB] (34) <41> - Noise sound power spectrum
        public static double CalcLwOkt<THydraulics>(THydraulics hydraulics, FunctionHydrVal<THydraulics> deltaLwOktFun, double lw, double fm) where THydraulics : Hydraulics
        {
            double deltaLwOktGessamt = CalcTotalLwOktGessamnt(hydraulics, deltaLwOktFun);
            double LwOkt = lw + deltaLwOktFun(hydraulics, fm) - deltaLwOktGessamt;
           
            return LwOkt < Defaults.LwMin
                ? Defaults.LwMin
                : LwOkt;
        }

        // [1] [LwOkt[], dB] (34) <41> - Noise sound power spectrum array
        public static double[] CalcLwOktArray<THydraulics>(THydraulics hydraulics, FunctionHydrVal<THydraulics> deltaLwOktFun, double lw) where THydraulics : Hydraulics
        {
            if (hydraulics == null)
                throw new ArgumentNullException(nameof(hydraulics));
            if (hydraulics == null)
                throw new ArgumentNullException(nameof(deltaLwOktFun));
            if (lw < 0)
                throw new ArgumentException(nameof(lw) + "=" + lw + "Is negative");

            return Defaults.OctCenterBand.Select(fm => CalcLwOkt(hydraulics, deltaLwOktFun, lw, fm)).ToArray();
        }
    }
}
