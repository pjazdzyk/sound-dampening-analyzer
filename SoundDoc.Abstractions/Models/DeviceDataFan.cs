using SoundDoc.Core.Data;

namespace SoundDoc.Abstractions.Models
{
    public class DeviceDataFan : DeviceDataBasic
    {
        public string DescriptionSecondary { get; private set; }
        public double CalculationFactor { get; private set; }
        public double Lws { get; private set; }

        public DeviceDataFan(string deviceID, string descPrim, string descSec, double specSoundPower, double calcFact) 
            : base(deviceID, descPrim)
        {
            DescriptionSecondary = descSec;
            CalculationFactor = calcFact;
            Lws = specSoundPower;
        }
    }
}
