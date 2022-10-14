using System;
using System.Text;
using SoundDoc.Core.Extensions;

namespace SoundDoc.Core.Data.HydraulicModels
{
    public class HydraulicsDamper : Hydraulics
    {
        public const bool TypeOpposedBlades = true;
        public const bool TypeParallelBlades = false;
        public double DischargeCoefficient { get; private set; }        
        public double BladeHeight { get; private set; }                 
        public bool DamperType { get; private set; }

        private HydraulicsDamper() : this(Defaults.FlowIn, Defaults.DamperDischargeCoef, Defaults.DamperBladeHeight, TypeOpposedBlades, Defaults.DimSize, Defaults.DimSize) { }

        public static HydraulicsDamper FromDefaults() 
        {
            return new();
        }

        private HydraulicsDamper(double Flow, double dischargeCoeficient, double bladeHeight, bool damperType, double inDim1, double inDim2)
            :base(Flow, inDim1, inDim2) 
        {
            SetDischargeCoeficient(dischargeCoeficient);
            SetDamperType(damperType);
            SetBladeHeight(bladeHeight);
        }

        public static HydraulicsDamper Create(double Flow, double dischargeCoeficient, double bladeHeight, bool damperType, double inDim1, double inDim2) 
        {
            return new(Flow, dischargeCoeficient, bladeHeight, damperType, inDim1, inDim2);
        }

        public void SetDischargeCoeficient(double dischargeCoeficient)
        {
            if(dischargeCoeficient < 0)
            {
                throw new ArgumentException();
            }

            DischargeCoefficient = dischargeCoeficient;
        }

        public void SetDamperType(bool damperType) 
        {
            DamperType = damperType;
        }

        public void SetBladeHeight(double bladeHeight) 
        {

            if (bladeHeight <= 0)
            {
                throw new ArgumentException();
            }

            BladeHeight = bladeHeight;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("\nDamper properties:");
            builder.Append($"dischargeCoef= {DischargeCoefficient.RoundUp(2)}");
            builder.Append($"  bladeHeight= {BladeHeight.RoundUp(2)}");
            builder.Append($"  opposedBlades= {DamperType}");

            return "\n--HYDRAULICS DAMPER--" + base.ToString() + builder.ToString(); 
        }

    }
}