using System;
using SoundDoc.Core.Data.HydraulicModels;
using System.Linq;
using SoundDoc.Core.Data.RoomModels;
using NoiseAnalyzer.Core.Utils;

namespace SoundDoc.Core.Physics
{
    /**
    * THE PHYSICS OF ACOUSTICS EQUATIONS LIBRARY - AIR TERMINALS
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

    public class PhysicsTerminal
    {
        // [1] [Lw, dB] (31) <40> - Terminal total noise sound power
        public static double CalcTerminalTotalLw(HydraulicsTerminal hydraulics)
        {
            
            var lw = 2.7 * Math.Log10(hydraulics.InVolFlow / 1) + 27.9 * Math.Log10(hydraulics.DeltaP / 1) - 5.4;

            if (lw < Defaults.LwMin)
                return Defaults.LwMin;

            return lw;
        }

        // [1] [Lw, dB] (32) <40> - Terminal total noise sound power
        public static double CalcTerminalDeltaLwOkt(HydraulicsTerminal hydraulics, double fm)
        {
            return -(71.72 - (67.37 / (1 + Math.Pow( (((fm * 1) / (hydraulics.InVelocity * hydraulics.DischargeCoefficient)) / 363.74), 1.1 ))));
        }

        // [1] [Lw, dB] (34) <41> - Terminal total noise sound power
        public static double[] CalcTerminalSourceLw(HydraulicsTerminal hydraulics)
        {
            return PhysicsCommon.CalcLwOktArray(hydraulics, CalcTerminalDeltaLwOkt, CalcTerminalTotalLw(hydraulics));
        }

        // [1] [Lw, dB] (43) <50> - Room terminal sound power reduction
        public static double CalcRoomDeltaLw(OutletLocation location, double grossDischArea, double fm)
        {

            if (fm < 0.0 || grossDischArea < 0.0)
                throw new ArgumentException("Negative value of argument." + nameof(location) + " = " + location
                                                                              + nameof(grossDischArea) + grossDischArea);
            double solidAngle;

            switch (location)
            {
                case OutletLocation.MiddleOfTheRoom: solidAngle = 4 * Math.PI; break;
                case OutletLocation.WallOrCeilingCenter: solidAngle = 2 * Math.PI; break;
                case OutletLocation.WallOrCeilingEdgeCenter: solidAngle = 1 * Math.PI; break;
                case OutletLocation.RoomCorner: solidAngle = 0.5 * Math.PI; break;
                default: throw new ArgumentException("Invalid terminal location.");
            }

            var result = 10 * Math.Log10(1 + Math.Pow((Defaults.SoundSpeedC / 4 * Math.PI * fm), 2) * solidAngle / grossDischArea);
            return result > 15 ? 15 : result;
        }

        // [-] [Lw, dB] (-) <-> - Room Terminal sound power reduction array
        public static double[] CalcRoomTerminalSourceDa(OutletLocation location, double grossDischArea)
        {
            return Defaults.OctCenterBand.Select(fm => CalcRoomDeltaLw(location, grossDischArea, fm)).ToArray();
        }
    }
}
