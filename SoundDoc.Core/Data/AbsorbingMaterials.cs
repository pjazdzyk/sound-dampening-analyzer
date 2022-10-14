using System.Collections.Generic;
using SoundDoc.Abstractions.Models;

namespace SoundDoc.Core.Data
{
   public class AbsorbingMaterials : MaterialAbstractList
    {
        protected override List<Material> Materials { get; set; } = new List<Material>
        {
            new Material { Name = "murBetonSur", Description = "Beton surowy", ValuesArray = new[] { 0.010, 0.010, 0.012, 0.016, 0.019, 0.023, 0.035, 0.035 } },
            new Material { Name = "murCeglaSur", Description = "Mur ceglany surowy", ValuesArray = new[] { 0.024, 0.024, 0.025, 0.031, 0.042, 0.049, 0.070, 0.070 } },
            new Material { Name = "murCeglOtynk", Description = "Mur ceglany otynkowany", ValuesArray = new[] { 0.010, 0.010, 0.010, 0.020, 0.020, 0.030, 0.030, 0.030 } },
            new Material { Name = "murCeglDziur", Description = "Ściana z cegły dziurawki + tynk gipsowy", ValuesArray = new[] { 0.010, 0.010, 0.010, 0.020, 0.030, 0.040, 0.050, 0.050 } },
            new Material { Name = "murChropowaty", Description = "Mur chropowaty na drewnianym oszalowaniu", ValuesArray = new[] { 0.025, 0.025, 0.045, 0.060, 0.085, 0.043, 0.058, 0.058 } },
            new Material { Name = "murBoazSosn", Description = "Boazeria sosnowa na ścianie", ValuesArray = new[] { 0.100, 0.110, 0.100, 0.080, 0.080, 0.043, 0.011, 0.011 } },
            new Material { Name = "mataWelMin100", Description = "Mata z wełny mineralnej o grubości 10cm pokryta tkaniną", ValuesArray = new[] { 0.390, 0.390, 0.450, 0.560, 0.590, 0.610, 0.550, 0.550 } },
            new Material { Name = "mataWelMin250", Description = "Mata z wełny mineralnej o grubości 25cm pokryta tkaniną", ValuesArray = new[] { 0.430, 0.430, 0.540, 0.590, 0.690, 0.700, 0.700, 0.700 } },
            new Material { Name = "mataWelMin25", Description = "Mata z wełny mineralnej 2.5cm w 2 warstwach ze szczeliną 4,5cm", ValuesArray = new[] { 0.500, 0.500, 0.630, 0.700, 0.810, 0.830, 0.830, 0.830 } },
            new Material { Name = "plytPilTwrdPrf", Description = "Płyta pilśniowa twarda perforowana fi4mm, 5cm od ściany", ValuesArray = new[] { 0.060, 0.060, 0.080, 0.550, 0.380, 0.230, 0.270, 0.270 } },
            new Material { Name = "plytPilMiek10", Description = "Płyta pilśniowa miękka o grubości 10cm, na twardym podłożu", ValuesArray = new[] { 0.320, 0.320, 0.240, 0.340, 0.430, 0.450, 0.450, 0.450 } },
            new Material { Name = "plytPilMiekPrf", Description = "Płyta pilśniowa miękka o grubości 10cm, perforowana, na listwach o grubości 60mm", ValuesArray = new[] { 0.713, 0.713, 0.388, 0.388, 0.750, 0.560, 0.710, 0.710 } },
            new Material { Name = "skljkPerfWeln", Description = "Sklejka perforowana o grubości 4mm na macie z wełny mineralnej", ValuesArray = new[] { 0.420, 0.420, 0.450, 0.460, 0.570, 0.570, 0.280, 0.280 } },
            new Material { Name = "plytWiorCem50", Description = "Płyta wiorowo-cementowa o grub. 50mmk na twardym podlozu", ValuesArray = new[] { 0.210, 0.210, 0.200, 0.350, 0.370, 0.410, 0.220, 0.220 } },
            new Material { Name = "styropian40", Description = "Styropian o gęstości 40kg/m3", ValuesArray = new[] { 0.050, 0.050, 0.040, 0.050, 0.280, 0.650, 0.100, 0.100 } },
            new Material { Name = "pianizoll20", Description = "Pianizol o gęstości 20kg/m3", ValuesArray = new[] { 0.170, 0.170, 0.230, 0.380, 0.730, 0.870, 0.600, 0.600 } },
            new Material { Name = "blachaOcynk", Description = "Blacha ocynkowana", ValuesArray = new[] { 0.010, 0.010, 0.010, 0.013, 0.015, 0.015, 0.015, 0.015 } },
            new Material { Name = "czlowiekSiedz", Description = "Czlowiek siedzący na miękkim krześle", ValuesArray = new[] { 0.250, 0.250, 0.360, 0.420, 0.460, 0.520, 0.560, 0.560 } },
            new Material { Name = "wykladzina", Description = "Podłoga, wykładzina (np. flotex)", ValuesArray = new[] { 0.00, 0.05, 0.03, 0.04, 0.10, 0.18, 0.18, 0.19 } },
            new Material { Name = "scianyTynk", Description = "ściany tynkowane lub malowane", ValuesArray = new[] { 0.00, 0.01, 0.01, 0.01, 0.02, 0.02, 0.02, 0.02 } },
            new Material { Name = "sciankiLekkie", Description = "Ścianki lekkie", ValuesArray = new[] { 0.00, 0.15, 0.10, 0.06, 0.04, 0.04, 0.05, 0.05 } },
            new Material { Name = "parapety", Description = "Parapety", ValuesArray = new[] { 0.00, 0.02, 0.03, 0.04, 0.05, 0.05, 0.05, 0.05 } },
            new Material { Name = "oknaZewn", Description = "Okna zewnętrzne", ValuesArray = new[] { 0.00, 0.01, 0.06, 0.04, 0.03, 0.02, 0.02, 0.02 } },
            new Material { Name = "elementySzklane", Description = "Elementy szklane, ścianki i drzwi", ValuesArray = new[] { 0.00, 0.15, 0.05, 0.03, 0.03, 0.03, 0.03, 0.03 } },
            new Material { Name = "sufitPodwieszony", Description = "Sufit podwieszony (np. Focus E)", ValuesArray = new[] { 0.00, 0.50, 0.90, 0.90, 0.90, 1.00, 1.00, 1.00 } },
            new Material { Name = "oswietlenieKasety", Description = "Oświetlenie kasetonowe", ValuesArray = new[] { 0.00, 0.30, 0.30, 0.30, 0.30, 0.30, 0.30, 0.30 } },
            new Material { Name = "szafkiMeble", Description = "Szafki i meble", ValuesArray = new[] { 0.00, 0.29, 0.25, 0.20, 0.17, 0.15, 0.11, 0.10 } }
        };
    }
}