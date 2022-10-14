
namespace SoundDoc.Core.Data.RoomModels
{
   public interface IRoomTerminal
    {
        public void SetLocation(OutletLocation location);
        public void SetGrossDischargeArea(double discArea);
        public void SetMountAngle(double mountAngle);
        public OutletLocation Location { get; }
        public double GrossDischArea { get; }
        public double MountAngle { get; }
        public AcuItem GetRoomOutlet();
    }
}
