using System.Collections.Generic;
using SoundDoc.Abstractions.Models;

namespace SoundDoc.Core.Data
{
    public class MaterialBendsLists : MaterialAbstractList
    {
        protected override List<Material> Materials { get; set; } = new List<Material>
        {
            new Material { Name = "roundDuct", Description = "Kolano o przekroju kołowym", SizeFrom = 0, SizeTo = 0.2, ValuesArray = new[] { 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 2.0, 3.0 } },
            new Material { Name = "roundDuct", Description = "Kolano o przekroju kołowym", SizeFrom = 0.2, SizeTo = 0.4, ValuesArray = new[] { 0.0, 0.0, 0.0, 0.0, 1.0, 2.0, 3.0, 3.0 } },
            new Material { Name = "roundDuct", Description = "Kolano o przekroju kołowym", SizeFrom = 0.4, SizeTo = 0.8, ValuesArray = new[] { 0.0, 0.0, 0.0, 1.0, 2.0, 3.0, 3.0, 3.0 } },
            new Material { Name = "roundDuct", Description = "Kolano o przekroju kołowym", SizeFrom = 0.8, SizeTo = 100, ValuesArray = new[] { 0.0, 0.0, 1.0, 2.0, 3.0, 3.0, 3.0, 3.0 } },

            new Material { Name = "rectDuct", Description = "Kolano o przekroju prostokątnym bez wykładziny dzwiękochłonnej", SizeFrom = 0, SizeTo = 0.2, ValuesArray = new[] { 0.0, 0.0, 0.0, 0.0, 6.0, 8.0, 4.0, 3.0 } },
            new Material { Name = "rectDuct", Description = "Kolano o przekroju prostokątnym bez wykładziny dzwiękochłonnej", SizeFrom = 0.2, SizeTo = 0.4, ValuesArray = new[] { 0.0, 0.0, 0.0, 6.0, 8.0, 4.0, 3.0, 3.0 } },
            new Material { Name = "rectDuct", Description = "Kolano o przekroju prostokątnym bez wykładziny dzwiękochłonnej", SizeFrom = 0.4, SizeTo = 0.8, ValuesArray = new[] { 0.0, 0.0, 6.0, 8.0, 4.0, 3.0, 3.0, 3.0 } },
            new Material { Name = "rectDuct", Description = "Kolano o przekroju prostokątnym bez wykładziny dzwiękochłonnej", SizeFrom = 0.8, SizeTo = 100, ValuesArray = new[] { 0.0, 6.0, 8.0, 4.0, 3.0, 3.0, 3.0, 3.0 } },

            new Material { Name = "coatRectDuct", Description = "Kolano o przekroju prostokątnym z wykładziną przed kształtką", SizeFrom = 0, SizeTo = 0.2, ValuesArray = new[] { 0.0, 0.0, 0.0, 0.0, 6.0, 8.0, 6.0, 8.0 } },
            new Material { Name = "coatRectDuct", Description = "Kolano o przekroju prostokątnym z wykładziną przed kształtką", SizeFrom = 0.2, SizeTo = 0.4, ValuesArray = new[] { 0.0, 0.0, 0.0, 6.0, 8.0, 6.0, 8.0, 11.0 } },
            new Material { Name = "coatRectDuct", Description = "Kolano o przekroju prostokątnym z wykładziną przed kształtką", SizeFrom = 0.4, SizeTo = 0.8, ValuesArray = new[] { 0.0, 0.0, 6.0, 8.0, 6.0, 8.0, 11.0, 11.0 } },
            new Material { Name = "coatRectDuct", Description = "Kolano o przekroju prostokątnym z wykładziną przed kształtką", SizeFrom = 0.8, SizeTo = 100, ValuesArray = new[] { 0.0, 6.0, 8.0, 6.0, 8.0, 11.0, 11.0, 11.0 } },

            new Material { Name = "rectDuctCoat", Description = "Kolano o przekroju prostokątnym z wykładziną za kształtką", SizeFrom = 0, SizeTo = 0.2, ValuesArray = new[] { 0.0, 0.0, 0.0, 0.0, 7.0, 11.0, 10.0, 10.0 } },
            new Material { Name = "rectDuctCoat", Description = "Kolano o przekroju prostokątnym z wykładziną za kształtką", SizeFrom = 0.2, SizeTo = 0.4, ValuesArray = new[] { 0.0, 0.0, 0.0, 7.0, 11.0, 10.0, 10.0, 10.0 } },
            new Material { Name = "rectDuctCoat", Description = "Kolano o przekroju prostokątnym z wykładziną za kształtką", SizeFrom = 0.4, SizeTo = 0.8, ValuesArray = new[] { 0.0, 0.0, 7.0, 11.0, 10.0, 10.0, 10.0, 10.0 } },
            new Material { Name = "rectDuctCoat", Description = "Kolano o przekroju prostokątnym z wykładziną za kształtką", SizeFrom = 0.8, SizeTo = 100, ValuesArray = new[] { 0.0, 7.0, 11.0, 10.0, 10.0, 10.0, 10.0, 10.0 } },

            new Material { Name = "coatRectDuctCoat", Description = "Kolano o przekroju prostokątnym z wykładziną przed i za kształtką", SizeFrom = 0, SizeTo = 0.2, ValuesArray = new[] { 0.0, 0.0, 0.0, 0.0, 7.0, 12.0, 14.0, 16.0 } },
            new Material { Name = "coatRectDuctCoat", Description = "Kolano o przekroju prostokątnym z wykładziną przed i za kształtką", SizeFrom = 0.2, SizeTo = 0.4, ValuesArray = new[] { 0.0, 0.0, 0.0, 7.0, 12.0, 14.0, 16.0, 18.0 } },
            new Material { Name = "coatRectDuctCoat", Description = "Kolano o przekroju prostokątnym z wykładziną przed i za kształtką", SizeFrom = 0.4, SizeTo = 0.8, ValuesArray = new[] { 0.0, 0.0, 7.0, 12.0, 14.0, 16.0, 18.0, 18.0 } },
            new Material { Name = "coatRectDuctCoat", Description = "Kolano o przekroju prostokątnym z wykładziną przed i za kształtką", SizeFrom = 0.8, SizeTo = 100, ValuesArray = new[] { 0.0, 7.0, 12.0, 14.0, 16.0, 18.0, 18.0, 18.0 } },
        };
    }
}