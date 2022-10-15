using System;
using System.Text;
using NoiseAnalyzer.Core.Utils;
using NoiseAnalyzer.Core.Extensions;

namespace NoiseAnalyzer.Core.HydraulicModels
{
    public class HydraulicsTerminal : Hydraulics
    {
        public double DeltaP { get; private set; }
        public double DischargeCoefficient { get; private set; }

        private HydraulicsTerminal() : this(Defaults.FlowIn, Defaults.DamperDischargeCoef, Defaults.TerminalDeltaP, Defaults.DimSize, Defaults.DimSize) { }

        public static HydraulicsTerminal FromDefaults()
        {
            return new();
        }

        private HydraulicsTerminal(double Flow, double dischargeCoeficient, double deltaP, double inDim1, double inDim2 = 0)
            : base(Flow, inDim1, inDim2)
        {
            SetDischargeCoeficient(dischargeCoeficient);
            SetDeltaPT(deltaP);
        }

        public static HydraulicsTerminal Create(double Flow, double dischargeCoeficient, double deltaP, double inDim1, double inDim2 = 0)
        {
            return new(Flow, dischargeCoeficient, deltaP, inDim1, inDim2);
        }

        private void SetDischargeCoeficient(double dischargeCoeficient)
        {
            if (dischargeCoeficient < 0)
            {
                throw new ArgumentException(nameof(dischargeCoeficient) + " Discharge Coefficient can't be negative.");
            }

            DischargeCoefficient = dischargeCoeficient;
        }

        private void SetDeltaPT(double deltaPT)
        {
            if (deltaPT < 0)
            {
                throw new ArgumentException("deltaPT can't be less than 0");
            }

            DeltaP = deltaPT;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("\nTerminal properties:");
            builder.Append($"dischargeCoef= {DischargeCoefficient.RoundUp(2)}");
            builder.Append($" deltaP= {DeltaP.RoundUp(2)}");

            return "\n--HYDRAULICS TERMINAL--" + base.ToString() + builder.ToString();
        }

    }
}