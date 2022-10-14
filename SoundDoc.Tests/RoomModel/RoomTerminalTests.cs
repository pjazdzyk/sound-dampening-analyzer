using SoundDoc.Core;
using SoundDoc.Core.Data.HydraulicModels;
using SoundDoc.Core.Data.RoomModels;
using Xunit;

namespace SoundDoc.Tests.RoomModel
{
    public class RoomTerminalTests
    {
        [Fact]
        public void TerminalWrapperTest()
        {

            // Arrange
            var flow = 0.2;     // m3/s
            var length = 1.0;   // m
            var dimA = 0.2;     // m
            var dimB = 0.3;     // m    
            var inputLw = new double[] { 51, 63, 65, 62, 75, 81, 84, 78 };
            var grossDiscArea = 0.048;  // m2

            var hydraulics = HydraulicsDuct.Create(flow, length, dimA, dimB);
            var duct = AcuDuct.FromHydraulics(hydraulics).WithName("DuctAsTerminal").WithInputLw(inputLw);
            var location = OutletLocation.RoomCorner;
            var angle = 0.0;    // deg
            var ductAsRoomTerminal = TerminalWrapper.FromAcuItem(duct).WithLocation(location).WithMountAngle(angle)
                                                                             .WithGrossDischargeArea(grossDiscArea);

            // Assert
            Assert.NotNull(ductAsRoomTerminal);
            Assert.Equal(ductAsRoomTerminal.Location, location);
            Assert.Equal(ductAsRoomTerminal.MountAngle, angle);
            Assert.Equal(ductAsRoomTerminal.GetRoomOutlet().InputLw, inputLw);

        }

    }
}