
namespace NoiseAnalyzer.Core.Enttities
{
    public class DeviceDataBasic
    {
        public string DeviceID { get; private set; }
        public string DescriptionPrimary { get; private set; }
        public DeviceDataBasic(string deviceID, string descPrim) 
        {
            DeviceID = deviceID;
            DescriptionPrimary = descPrim;
        }

    }
}
