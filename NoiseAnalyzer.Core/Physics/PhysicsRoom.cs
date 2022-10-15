using NoiseAnalyzer.Core.AcousticModels;
using NoiseAnalyzer.Core.Extensions;
using NoiseAnalyzer.Core.RoomModels;
using NoiseAnalyzer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoiseAnalyzer.Core.Physics
{
    /**
    * THE PHYSICS OF ACOUSTICS EQUATIONS LIBRARY - SOUND PROPAGATION IN REAL CUBIC ROOMS
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

    public class PhysicsRoom
    {

        /* -----------------------PROPERTIES BASED ON REVERBERATION TIME--------------------- */

        // [1] [rH,m] (-) <55> - Radius from which reverberant area begins, based on reverberation time T
        public static double CalcRoomRvrbHemisphereRadiusFromT(double roomVol, double reverbT)
        {
            if (roomVol < 0 || reverbT < 0)
                throw new ArgumentException("Negative value exception. " + nameof(roomVol) + " = " + roomVol
                                                                         + nameof(reverbT) + " = " + reverbT);
            return 0.08 * Math.Sqrt(roomVol / reverbT);
        }

        // [1] [A,m2] (49) <55> - Room equivalent sound absorpton area based on reverberation time T
        public static double CalcRoomRvrbEquivSoundAbsAreaA(double roomVol, double reverbT)
        {
            if (roomVol < 0 || reverbT < 0)
                throw new ArgumentException("Negative value exception. " + nameof(roomVol) + " = " + roomVol
                                                                         + nameof(reverbT) + " = " + reverbT);

            return (24 * Math.Log(10) / Defaults.SoundSpeedC) * roomVol / reverbT;
        }

        // [1] [Lp,dB] (50) <55> - Far field room sound pressure based on reverberation time T
        public static double CalcRoomRvrbFarFieldSoundPressLp(double terminalLw, double roomVol, double reverbT)
        {
            if (terminalLw < 0 || roomVol < 0 || reverbT < 0)
                throw new ArgumentException("Negative value exception. " + nameof(terminalLw) + " = " + terminalLw
                                                                         + nameof(roomVol) + " = " + roomVol
                                                                         + nameof(reverbT) + " = " + reverbT);

            return terminalLw - 10 * Math.Log10(roomVol / 1) + 10 * Math.Log10(reverbT / 1) + 14;
        }


        /* -------------------PROPERTIESD BASED ON REAL ROOM MATERIAL FINISHES------------------- */

        // [1] [rH,m] (-) <55> - Radius from which reverberant area begins, based on equiv absorbption area A
        public static double CalcRoomHemisphereRadiusFromA(double equivAbsArea)
        {
            if (equivAbsArea < 0)
                throw new ArgumentException("Negative value exception. " + nameof(equivAbsArea) + " = " + equivAbsArea);

            return 0.2 * Math.Sqrt(equivAbsArea);
        }

        // [1] [Ai,m2] (1st part of 51) <56> - Equivalent sound absorption area of room, based on material proprties
        public static double CalcRoomEquivSoundAbsArea(double alphaCoef, double elementArea)
        {
            if (alphaCoef < 0 || elementArea < 0)
                throw new ArgumentException("Negative value exception. " + nameof(alphaCoef) + " = " + alphaCoef
                                                                         + nameof(elementArea) + " = " + elementArea);
            return alphaCoef * elementArea;
        }

        // [1] [-] (-) <54> - Table 15. Coeficients for calculating the directivity factor Q - [B1, B2, x0, p].
        public static double[] GetDirIndexCoeficients(OutletLocation location, double angle)
        {

            if (angle < 0.0 || angle > 90)
                throw new ArgumentException("Invalid outlet angle. " + nameof(location) + " = " + location
                                                                     + nameof(angle) + " = " + angle);
            if (angle >= 0.0 && angle < 45.0)
            {

                switch (location)
                {
                    case OutletLocation.MiddleOfTheRoom: return new double[] { 0.73, 7.62, 158.51, 1.29 };
                    case OutletLocation.WallOrCeilingCenter: return new double[] { 1.70, 7.88, 121.19, 1.28 };
                    case OutletLocation.WallOrCeilingEdgeCenter: return new double[] { 3.90, 8.28, 133.97, 1.27 };
                    case OutletLocation.RoomCorner: return new double[] { 7.28, 9.42, 298.46, 0.37 };
                }
            }

            if (angle >= 45)
            {

                switch (location)
                {
                    case OutletLocation.MiddleOfTheRoom: return new double[] { 0.84, 4.01, 213.89, 1.10 };
                    case OutletLocation.WallOrCeilingCenter: return new double[] { 1.90, 4.16, 221.72, 1.25 };
                    case OutletLocation.WallOrCeilingEdgeCenter: return new double[] { 3.78, 5.23, 395.71, 42.28 };
                    case OutletLocation.RoomCorner: return new double[] { 8.35, 4.42, 42.28, 1.75 };
                }
            }

            throw new ArgumentException("Invalid location exception");
        }

        // [1] [-] (46) <54> - Directivity index Q as function of the frequency in Hz and gross sound outlet area.
        public static double CalcDirectivityIndex(OutletLocation location, double angle, double grossDischArea, double fm)
        {

            if (fm < 0.0 || angle < 0.0 || fm < 0.0)
                throw new ArgumentException("Negative value of argumentexception" + nameof(location) + " = " + location
                                            + nameof(angle) + " = " + angle + nameof(grossDischArea) + grossDischArea);

            double[] coeficients = GetDirIndexCoeficients(location, angle);
            var B1 = coeficients[0];
            var B2 = coeficients[1];
            var x0 = coeficients[2];
            var p = coeficients[3];

            return B2 + (B1 - B2) / (1 + Math.Pow((fm * Math.Sqrt(grossDischArea) / x0), p));
        }

        // [1] [Lp.ges, dB] (52) <56> - Sound pressure level for a given fm (based on sound absobrtion area)
        public static double CalcRoomSoundPressFromTerminal(IRoomTerminal terminal, double equivAbsAreaForFm, double minDist, double fm)
        {

            if (terminal == null)
                throw new ArgumentNullException(nameof(terminal) + " Null passed for terminal argument");
            
            if (minDist < 0 || equivAbsAreaForFm < 0 || fm < 0)
                throw new ArgumentException( " Negative values are not acceptable");

            var fmIndex = Array.IndexOf(Defaults.OctCenterBand, fm);
            var termLw = terminal.GetRoomOutlet().OutputLw[fmIndex];
            var dirQ = CalcDirectivityIndex(terminal.Location, terminal.MountAngle, terminal.GrossDischArea, fm);

            if (minDist == 0.0)
                return termLw;

            var resultLp = termLw + 10.0 * Math.Log10((dirQ * 1.0 * 1.0) / (4.0 * Math.PI * minDist * minDist) + (4.0 * 1.0) / equivAbsAreaForFm);

            if (resultLp > termLw || equivAbsAreaForFm < 1)
                return termLw;

            return resultLp;
        }

        // [1] [Lp, dB] (44) <51> - Sound pressure level in the diffuse sound field
        public static double CalcRoomSoundPressAsDiffuseField(double lw, double equivAbsArea) 
        {
            if (lw < 0 || equivAbsArea < 0)
                throw new ArgumentException("Negative values are not acceptable");

            if (lw == 0)
                return 0.0;

            if (equivAbsArea == 0)
                return lw;

            return lw + 10 * Math.Log10(4 * 1 / equivAbsArea);
        }

        public static double[] CalcRoomSourceLpFromSourceLwArray(double[] sourceLw, double[] equivAbsArea)
        {
            if (sourceLw == null || equivAbsArea == null)
                throw new ArgumentNullException(nameof(sourceLw) + " Null passed for sourceLw argument");

            var sourceLp = Defaults.EmptyOctaveArray;

            for (int i = 0; i < sourceLw.Length; i++) 
            {
                var lw = sourceLw[i];
                var absArea = equivAbsArea[i];
                sourceLp[i] = CalcRoomSoundPressAsDiffuseField(lw, absArea);
            }

            return sourceLp;
        }

        // [1] [Lp.ges, dB] (52) <56> - Overall sound pressure level for each octave (based on sound absobrtion area)
        public static double[] CalcRoomSoundPressureArray(List<IRoomTerminal> terminalList, double[] equivAbsArea, double[] sourceLw, double[] sourceLp, double minDist) 
        {

            if (terminalList == null || equivAbsArea==null)
                throw new ArgumentNullException(nameof(terminalList) + " Null passed for room argument");

            if (terminalList.Count() == 0)
                return Defaults.EmptyOctaveArray;

            var soundPressArr = new double[8];
            var sourceLpFromLw = CalcRoomSourceLpFromSourceLwArray(sourceLw, equivAbsArea);
           
            for (int i = 0; i < soundPressArr.Length; i++) 
            {
                var fm = Defaults.OctCenterBand[i];
                var equivAbsAreaForFm = equivAbsArea[i];
                var termLp = terminalList.Select(terminal => CalcRoomSoundPressFromTerminal(terminal, equivAbsAreaForFm, minDist, fm)).ToArray().LogSum();
                var srcLp = sourceLp[i];
                var srcLpFromSrcLw = sourceLpFromLw[i];
                soundPressArr[i] = PhysicsCommon.CalcLogSum(termLp,srcLp,srcLpFromSrcLw);
            }

            return soundPressArr;
        }
    }
}
