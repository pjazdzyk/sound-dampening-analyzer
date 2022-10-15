using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseAnalyzer.Core.AcousticModels
{
    public enum OutletLocation
    {
        MiddleOfTheRoom = 1,
        WallOrCeilingCenter = 2,
        WallOrCeilingEdgeCenter = 3,
        RoomCorner = 4
    }
}