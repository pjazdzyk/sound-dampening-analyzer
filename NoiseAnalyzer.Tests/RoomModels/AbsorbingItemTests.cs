using Xunit;
using NoiseAnalyzer.Core.RoomModels;
using NoiseAnalyzer.Core.Extensions;
using NoiseAnalyzer.Core.Repositories;

namespace NoiseAnalyzer.Tests.RoomModels
{
    public class AbsorbingItemTests
    {
        double mathAccuracy = 0.00001;

        [Fact]
        public void AbsorbingPartTest() 
        {
            AbsorbingMaterialsMemoryRepo materialList = new AbsorbingMaterialsMemoryRepo();
            var coating = materialList.GetMaterialByName("wykladzina");
            var glassPanel = materialList.GetMaterialByName("elementySzklane"); 

            // Arrange
            AbsorbingPart part1 = AbsorbingPart.FromMaterialAndArea(coating,266.3);
            var expected = new double[] { 0.000, 13.3150, 7.9890, 10.6520, 26.6300, 47.9340, 47.9340, 50.5970 };

            // Act
            var actual = part1.PartEquivArea;

            // Assert
            Assert.True(expected.EqualsToPrecision(actual, mathAccuracy));

            // Material change
            part1.SetPartMaterial(glassPanel);
            expected = new double[] { 0.000, 39.9450, 13.3150, 7.9890,  7.9890,  7.9890,  7.9890,  7.9890 };
            actual = part1.PartEquivArea;

            // Assert
            Assert.True(expected.EqualsToPrecision(actual, mathAccuracy));

            // Area change
            part1.SetPartArea(100);
            expected = new double[] { 0.0, 15.0, 5.0, 3.0, 3.0, 3.0, 3.0, 3.0 };
            actual = part1.PartEquivArea;

            // Assert
            Assert.True(expected.EqualsToPrecision(actual, mathAccuracy));
        }

        [Fact]
        public void AbsorbingItemTest() 
        {
            
            // Arrange
            AbsorbingMaterialsMemoryRepo materialList = new AbsorbingMaterialsMemoryRepo();
            var coating = materialList.GetMaterialByName("wykladzina");
            var glassPanel = materialList.GetMaterialByName("elementySzklane");
            var part1 = AbsorbingPart.FromMaterialAndArea(coating, 266.3);
            var part2 = AbsorbingPart.FromMaterialAndArea(glassPanel, 100).WithName("szklo");
            var absItem = AbsorbingItemFromMaterial.FromParts(part1,part2);
            var expected = new double[] { 0.0000, 28.3150, 12.9890, 13.6520, 29.6300, 50.9340, 50.9340, 53.5970 };

            // Act
            var actual = absItem.ItemAbsEquivArea;

            // Assert
            Assert.True(expected.EqualsToPrecision(actual,mathAccuracy));

            // Change: add new part
            var czlowiekSiedz = materialList.GetMaterialByName("czlowiekSiedz");
            var part3 = AbsorbingPart.FromMaterialAndArea(czlowiekSiedz,12.0);
            absItem.AddAbsorbingPart(part3);
            expected = new double[] { 3.0000, 31.3150, 17.3090, 18.6920, 35.1500, 57.1740, 57.6540, 60.3170 };
            actual = absItem.ItemAbsEquivArea;

            // Assert
            Assert.True(expected.EqualsToPrecision(actual, mathAccuracy));

            // Change: change area for one of the internal parts, by index
            absItem.EditPartAtIndex(0,coating,150);
            expected = new double[] { 3.0000, 25.5000, 13.8200, 14.0400, 23.5200, 36.2400, 36.7200, 38.2200 };
            actual = absItem.ItemAbsEquivArea;

            // Assert
            Assert.True(expected.EqualsToPrecision(actual, mathAccuracy));

            // Change: change area for one of the internal parts, by name
            absItem.EditPartOfName("szklo", glassPanel, 200);
            expected = new double[] { 3.0000, 40.5000, 18.8200, 17.0400, 26.5200, 39.2400, 39.7200, 41.2200 };
            actual = absItem.ItemAbsEquivArea;

            // Assert
            Assert.True(expected.EqualsToPrecision(actual, mathAccuracy));

            // Change: removing parts
            absItem.RemoveAbsorbingPart(1);
            expected = new double[] { 3.0000, 10.5000, 8.8200, 11.0400, 20.5200, 33.2400, 33.7200, 35.2200 };
            actual = absItem.ItemAbsEquivArea;

            // Assert
            Assert.True(expected.EqualsToPrecision(actual, mathAccuracy));

        }

    }
}
