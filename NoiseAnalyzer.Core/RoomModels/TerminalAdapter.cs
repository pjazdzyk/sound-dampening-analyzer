using NoiseAnalyzer.Core.AcousticModels;
using System;

namespace NoiseAnalyzer.Core.RoomModels
{

    /// <summary>
    /// Terminal Wrapper can wrap any AcuItem object and use it as a room terminal for room model calculations.
    /// This is required for rare cases where instead of a diffuser an open duct, damper or other element
    /// is used (typical for industrial buildings).
    /// </summary>
    public class TerminalAdapter : IRoomTerminal
    {
        private AcuItem _roomOutlet;
        public OutletLocation Location { get; private set; }
        public double MountAngle { get; private set; }
        public double GrossDischArea { get; private set; }

        private TerminalAdapter(AcuItem acuItem) 
        {
            if (typeof(AcuTerminal).IsInstanceOfType(acuItem))
                throw new ArgumentException("Invalid argument. Do not use TerminalWrapper for AcuTerminal");

            _roomOutlet = acuItem;
            Location = OutletLocation.WallOrCeilingCenter;
            MountAngle = 0.0;
        }

        public static TerminalAdapter FromAcuItem(AcuItem acuItem) 
        {
            return new(acuItem);
        }

        public TerminalAdapter WithLocation(OutletLocation location) 
        {
            Location = location;
            return this;
        }
        
        public TerminalAdapter WithMountAngle(double mountAngle)
        {
            MountAngle = mountAngle;
            return this;
        }

        public TerminalAdapter WithGrossDischargeArea(double grossDischArea)
        {
            GrossDischArea = grossDischArea;
            return this;
        }

        public void SetLocation(OutletLocation location)
        {
            Location = location;
        }

        public void SetGrossDischargeArea(double discArea)
        {
            GrossDischArea = discArea;
        }

        public void SetMountAngle(double mountAngle)
        {
            MountAngle = mountAngle;
        }

        public AcuItem GetRoomOutlet()
        {
            return _roomOutlet;
        }
    }
}
