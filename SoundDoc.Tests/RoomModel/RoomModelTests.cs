using SoundDoc.Core;
using SoundDoc.Core.Data;
using SoundDoc.Core.Data.HydraulicModels;
using SoundDoc.Core.Data.RoomModels;
using SoundDoc.Core.Extensions;
using Xunit;

namespace SoundDoc.Tests.RoomModel
{
    public class RoomModelTests
    {
        public const double MathAccuracy = 0.000000000001;

        [Fact]
        public void RoomModelTest()
        {
            // Arrange Absorbing Items
            AbsorbingMaterials materialList = new AbsorbingMaterials();
            var coating = materialList.GetMaterialByName("wykladzina");
            var glassPanel = materialList.GetMaterialByName("elementySzklane");
            var coatingPart = AbsorbingPart.FromMaterialAndArea(coating, 266.3);
            var glassPart = AbsorbingPart.FromMaterialAndArea(glassPanel, 100).WithName("szklo");
            var absItem = AbsorbingItemFromMaterial.FromParts(coatingPart, glassPart);

            // Arrange Terminals
            var flow = 0.2;     // m3/s
            var length = 1.0;   // m
            var dimA = 0.2;     // m
            var dimB = 0.3;     // m    
            var inputLw = new double[] { 51, 63, 65, 62, 75, 81, 84, 78 };
            var grossDiscArea = 0.048;  // m2
            var zetaCoef = 2.0; // -     
            var deltaP = 23;    // Pa    
            var location = OutletLocation.RoomCorner;
            var mountAngle = 0.0;    // deg
            var hydraulics = HydraulicsDuct.Create(flow, length, dimA, dimB);
            var hydraulicsTerminal = HydraulicsTerminal.Create(flow, zetaCoef, deltaP, dimA, dimB);
            var duct = AcuDuct.FromHydraulics(hydraulics).WithName("DuctAsTerminal").WithInputLw(inputLw);
            var ductAsRoomTerminal = TerminalWrapper.FromAcuItem(duct).WithLocation(location).WithMountAngle(mountAngle).WithGrossDischargeArea(grossDiscArea);
            var acuTerminal = AcuTerminal.FromHydraulics(hydraulicsTerminal).WithInputLw(inputLw).WithLocation(location).WithGrossDischargeArea(grossDiscArea);
            var office = Core.Data.RoomModels.RoomModel.Create().WithName("Office");
            var expectedTermDuctOutLw = new[] { 51.051136242385965, 62.78693271550494, 64.78277976530185, 61.71697296333068, 74.67300432076078, 80.67300024435974, 83.67300002734734, 77.673 };
            var expectedTermAcuOutlw = new[] { 36.52547742025116, 48.02609134880121, 50.00900752065636, 47.00572176577775, 60.00004173003829, 66.0, 69.0, 63.0 };

            // 1-CASE: Single room terminal, no absorbtion area (empty zero-filled arrays)
            office.AddRoomTerminal(ductAsRoomTerminal);
            var expectedLp = expectedTermDuctOutLw;
            var expectedSumLp = 86.4748101950047;
            var actualLp = office.RoomOutLp;
            var sumLp = office.RoomSumOutLp;
            Assert.True(ArrayExtensions.EqualsToPrecision(expectedLp, actualLp, MathAccuracy));
            Assert.Equal(expectedSumLp, sumLp, 12);

            // 2-CASE: Single room terminal, with absorbing Item
            office.AddAbsorbingItem(absItem);
            expectedLp = new[] {51.0511362423859, 56.0332514769924, 60.5681317198801, 57.3410093271209, 67.8596469088027, 72.5208842869117, 75.5545174310633, 69.4757928613930};
            expectedSumLp = 78.51138389263640000;
            actualLp = office.RoomOutLp;
            sumLp = office.RoomSumOutLp;
            Assert.True(ArrayExtensions.EqualsToPrecision(expectedLp, actualLp, MathAccuracy));
            Assert.Equal(expectedSumLp,sumLp, 12);

            // 3-CASE: Single room terminal, removed absorbing Item
            office.RemoveAbsorbingItem();
            expectedLp = expectedTermDuctOutLw;
            expectedSumLp = 86.4748101950047;
            actualLp = office.RoomOutLp;
            sumLp = office.RoomSumOutLp;
            Assert.True(ArrayExtensions.EqualsToPrecision(expectedLp, actualLp, MathAccuracy));
            Assert.Equal(expectedSumLp, sumLp, 12);

            // 4-CASE: Removed terminal, with absorbing item
            office.AddAbsorbingItem(absItem);
            office.RemoveTerminal();
            expectedLp = Defaults.EmptyOctaveArray;
            expectedSumLp = 0.0;
            actualLp = office.RoomOutLp;
            sumLp = office.RoomSumOutLp;
            Assert.True(ArrayExtensions.EqualsToPrecision(expectedLp, actualLp, MathAccuracy));
            Assert.Equal(expectedSumLp, sumLp, 12);

            // 5-CASE: Removed terminal, removed absorbing item
            office.RemoveAbsorbingItem();
            expectedLp = Defaults.EmptyOctaveArray;
            expectedSumLp = 0.0;
            actualLp = office.RoomOutLp;
            sumLp = office.RoomSumOutLp;
            Assert.True(ArrayExtensions.EqualsToPrecision(expectedLp, actualLp, MathAccuracy));
            Assert.Equal(expectedSumLp, sumLp, 12);

            // 6-CASE: Two terminals, with absorbing item
            office.AddAbsorbingItem(absItem);
            office.AddRoomTerminal(ductAsRoomTerminal);
            office.AddRoomTerminal(acuTerminal);
            expectedLp = new[] { 51.2016822983685, 56.1759908092165, 60.7104535636848, 57.4853609330515, 68.0052555928478, 72.6664917294532, 75.7001248807599, 69.6214003119914 };
            expectedSumLp = 78.6569217810137;
            actualLp = office.RoomOutLp;
            sumLp = office.RoomSumOutLp;
            Assert.True(ArrayExtensions.EqualsToPrecision(expectedLp, actualLp, MathAccuracy));
            Assert.Equal(expectedSumLp, sumLp, 12);

            // 7-CASE: Two temerminals, with absorbing item, exposure distance change
            office.SetExposureDistance(1.8);
            expectedLp = new[] { 51.2016822983685, 58.1858201873018, 61.9584786669848, 58.7921128324023, 70.1152361401687, 75.3743749160367, 78.4243620934942, 72.4129353923431 };
            expectedSumLp = 81.2987717673637;
            actualLp = office.RoomOutLp;
            sumLp = office.RoomSumOutLp;
            Assert.True(ArrayExtensions.EqualsToPrecision(expectedLp, actualLp, MathAccuracy));
            Assert.Equal(expectedSumLp, sumLp, 12);

            // 8-CASE: Two temerminals, with absorbing item, exposure distance = 0
            office.SetExposureDistance(0.0);
            expectedLp = new[] { 51.2016822983685, 62.929672047729, 64.9251016091065, 61.8613245692611, 74.8186130048058, 80.8186076869012, 83.818607477044, 77.8186074505984};
            expectedSumLp = 86.6203804324628;
            actualLp = office.RoomOutLp;
            sumLp = office.RoomSumOutLp;
            Assert.True(ArrayExtensions.EqualsToPrecision(expectedLp, actualLp, MathAccuracy));
            Assert.Equal(expectedSumLp, sumLp, 12);

            // 9-CASE: Two terminals, with absorbing item, exposure distance =3, with souceLp
            var sourceLp = new[] { 48.0, 41.0, 46.0, 47.0, 41.0, 41.0, 78.0, 50.0 };
            office.SetExposureDistance(3.0);
            office.SetSourceLp(sourceLp);
            expectedLp = new[] { 52.8997332174259, 56.30590993228, 60.8548312480718, 57.8573383746375, 68.0139018145878, 72.6694496549687, 80.0108579233483, 69.668529039386 };
            expectedSumLp = 81.36041241221400000;
            actualLp = office.RoomOutLp;
            sumLp = office.RoomSumOutLp;
            Assert.True(ArrayExtensions.EqualsToPrecision(expectedLp, actualLp, MathAccuracy));
            Assert.Equal(expectedSumLp, sumLp, 12);

            // 10-CASE: Two terminals, with absorbing item, exposure distance =3, with souceLp, with sourceLw
            var sourceLw = new[] { 58.0, 51.0, 56.0, 57.0, 51.0, 51.0, 88.0, 60.0 };
            office.SetSourceLw(sourceLw);
            expectedLp = new[] { 59.1694315024522, 56.4830631214199, 61.2714950036479, 58.7933347418732, 68.0255468305665, 72.6717711912282, 81.7551558532058, 69.7033714050443 };
            expectedSumLp = 82.7266887234606;
            actualLp = office.RoomOutLp;
            sumLp = office.RoomSumOutLp;
            Assert.True(ArrayExtensions.EqualsToPrecision(expectedLp, actualLp, MathAccuracy));
            Assert.Equal(expectedSumLp, sumLp, 12);
        }
    }
}
