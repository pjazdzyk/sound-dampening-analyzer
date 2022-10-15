using System;
using System.Collections.Generic;
using System.Linq;
using NoiseAnalyzer.Core.HydraulicModels;
using NoiseAnalyzer.Core.Physics;
using NoiseAnalyzer.Core.Utils;

namespace NoiseAnalyzer.Core.AcousticModels
{
    public class AcuDamper : AcuBasic<HydraulicsDamper>
    {
        private AcuDamper() : this(HydraulicsDamper.FromDefaults()) { }

        public static AcuDamper FromDefaults()
        {
            return new();
        }

        private AcuDamper(HydraulicsDamper hydraulics)
            : base(hydraulics)
        {
            UpdateAcousticProperties();
        }

        public static AcuDamper FromHydraulics(HydraulicsDamper hydraulics)
        {
            return new(hydraulics);
        }

        public AcuDamper WithName(string name)
        {
            Name = name;
            return this;
        }

        public AcuDamper WithInputLw(double[] inputLw)
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
            SourceLw = PhysicsDamper.CalcDamperSourceLw(Hydraulics);
        }

        public void SetDischargeCoeficient(double dischargeCoeficient)
        {
            Hydraulics.SetDischargeCoeficient(dischargeCoeficient);
            UpdateAcousticProperties();
        }

        public void SetDamperType(bool damperType)
        {
            Hydraulics.SetDamperType(damperType);
            UpdateAcousticProperties();
        }

        public void SetBladeHeight(double bladeHeight)
        {
            Hydraulics.SetBladeHeight(bladeHeight);
            UpdateAcousticProperties();
        }
    }
}