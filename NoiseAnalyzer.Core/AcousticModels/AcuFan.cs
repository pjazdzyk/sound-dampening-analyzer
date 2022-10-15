using NoiseAnalyzer.Core.Utils;
using NoiseAnalyzer.Core.HydraulicModels;
using NoiseAnalyzer.Core.Physics;

namespace NoiseAnalyzer.Core.AcousticModels
{
    public class AcuFan : AcuBasic<HydraulicsFan>
    {
        private AcuFan() : this(HydraulicsFan.FromDefaults()) { }

        public static AcuFan FromDefaults()
        {
            return new();
        }

        private AcuFan(HydraulicsFan hydraulics)
            : base(hydraulics)
        {
            UpdateAcousticProperties();
        }

        public static AcuFan FromHydraulics(HydraulicsFan hydraulics)
        {
            return new(hydraulics);
        }

        public AcuFan WithName(string name)
        {
            Name = name;
            return this;
        }

        public AcuFan WithInputLw(double[] inputLw)
        {
            SetInputLw(inputLw);
            return this;
        }

        protected override void CalculateSourceDa()
        {
            SourceDa = Defaults.InputLwMinAray;
        }

        protected override void CalculateSourceLw()
        {
            SourceLw = PhysicsFan.CalcFanSourceLw(Hydraulics);
        }

        public void SetDeltaP(double deltaP)
        {
            Hydraulics.SetDeltaP(deltaP);
            UpdateAcousticProperties();
        }

        public void SetFanRPM(double fanRPM)
        {
            Hydraulics.SetFanRPM(fanRPM);
            UpdateAcousticProperties();
        }

        public void SetFanType(string key)
        {
            Hydraulics.SetFanType(key);
            UpdateAcousticProperties();
        }

    }
}


