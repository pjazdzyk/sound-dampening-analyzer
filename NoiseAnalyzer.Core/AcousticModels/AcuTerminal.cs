using NoiseAnalyzer.Core.HydraulicModels;
using NoiseAnalyzer.Core.Physics;
using NoiseAnalyzer.Core.RoomModels;

namespace NoiseAnalyzer.Core.AcousticModels
{
    public sealed class AcuTerminal : AcuBasic<HydraulicsTerminal>, IRoomTerminal
    {
        public OutletLocation Location { get; private set; }
        public double GrossDischArea { get; private set; }
        public double MountAngle { get; private set; }
        public bool IncludeGrilles { get; private set; }

        private AcuTerminal() : this(HydraulicsTerminal.FromDefaults()) { }

        private AcuTerminal(HydraulicsTerminal hydraulics)
            : base(hydraulics)
        {
            IncludeGrilles = true;
            Location = OutletLocation.WallOrCeilingCenter;
            MountAngle = 0.0;
            GrossDischArea = hydraulics.InEquivD;
            UpdateAcousticProperties();
        }

        public static AcuTerminal FromDefaults()
        {
            return new();
        }

        public static AcuTerminal FromHydraulics(HydraulicsTerminal hydraulics)
        {
            return new(hydraulics);
        }

        public AcuTerminal WithName(string name)
        {
            Name = name;
            return this;
        }

        public AcuTerminal WithInputLw(double[] inputLw)
        {
            SetInputLw(inputLw);
            return this;
        }

        public AcuTerminal WithGrossDischargeArea(double grossDiscArea)
        {
            SetGrossDischargeArea(grossDiscArea);
            return this;
        }

        public AcuTerminal WithLocation(OutletLocation location)
        {
            SetLocation(location);
            return this;
        }

        public void SetLocation(OutletLocation location)
        {
            Location = location;
            UpdateAcousticProperties();
        }

        public void SetGrossDischargeArea(double discArea)
        {
            GrossDischArea = discArea;
            UpdateAcousticProperties();
        }

        public void SetMountAngle(double mountAngle)
        {
            MountAngle = mountAngle;
        }

        protected override void CalculateSourceDa()
        {
            if (!IncludeGrilles)
                return;

            SourceDa = PhysicsTerminal.CalcRoomTerminalSourceDa(Location, GrossDischArea);
        }

        protected override void CalculateSourceLw()
        {
            SourceLw = PhysicsTerminal.CalcTerminalSourceLw(Hydraulics);
        }

        public AcuItem GetRoomOutlet()
        {
            return this;
        }
    }
}


