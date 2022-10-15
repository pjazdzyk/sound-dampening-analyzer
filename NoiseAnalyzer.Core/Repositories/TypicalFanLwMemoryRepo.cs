using NoiseAnalyzer.Core.Enttities;
using System.Collections.Generic;


namespace NoiseAnalyzer.Core.Repositories
{

    // [1] [Lws, dB] (Table 7, 8) <27,28> - Rectangular or circular duct total flow noise independent from length
    public class TypicalFanLwMemoryRepo
    {
        public static readonly Dictionary<string, DeviceDataFan> TypicalFanSpecificLw = new Dictionary<string, DeviceDataFan>
        {
            { "HP_RG", new DeviceDataFan("HP_RG", "Assembly RG (high pressure side)","Centrifugal fan with spiral casing", 33.0, 0.4) },
            { "LP_RG", new DeviceDataFan("LP_RG", "Assembly RG (low pressur eside)","Centrifugal fan with spiral casing", 31.0, 0.5) },
            { "HP_RF", new DeviceDataFan("HP_RF", "Assembly RF (high pressure side)","Centrifugal fan free-wheeling", 35.0, -0.2) },
            { "LP_RF", new DeviceDataFan("LP_RF", "Assembly RF (low pressure side)","Centrifugal fan free-wheeling", 32.0, 0.1) },
            { "TX", new DeviceDataFan("TX", "Assembly TX","Drum impeller", 36.0, 0.15) },
            { "AO", new DeviceDataFan("AO", "Assembly AO","Axial fan without guide vanes", 40.0, 0.1) },
            { "AN", new DeviceDataFan("AN", "Assembly AN","Axial fan with guide vanes", 40.0, -0.3) },
        };

        public static DeviceDataFan GetFanData(string key) 
        {
            if (!TypicalFanSpecificLw.ContainsKey(key))
                throw new KeyNotFoundException("Nie znaleziono klucza " + nameof(key) + "=" + key);

            return TypicalFanSpecificLw[key];

        }

    }
}
