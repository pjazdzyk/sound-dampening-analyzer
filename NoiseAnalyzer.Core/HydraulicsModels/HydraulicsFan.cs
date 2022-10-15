using System;
using System.Text;
using NoiseAnalyzer.Core.Utils;
using NoiseAnalyzer.Core.Extensions;
using NoiseAnalyzer.Core.Enttities;
using NoiseAnalyzer.Core.Repositories;

namespace NoiseAnalyzer.Core.HydraulicModels

{
    public class HydraulicsFan : Hydraulics
    {
        public double DeltaP { get; private set; }  
        public double FanRPM { get; private set; }
        public DeviceDataFan FanType { get; private set; }

        private HydraulicsFan() : this(Defaults.FlowIn, Defaults.FanDeltaP, Defaults.FanTypeKey, Defaults.FanRPM, Defaults.DimSize, Defaults.DimSize) { }

        public static HydraulicsFan FromDefaults() 
        {
            return new();
        }

        private HydraulicsFan(double inFlow, double deltaP, string fanTypeKey, double fanRPM, double inDim1, double inDim2)
            :base(inFlow, inDim1, inDim2)
        {
            SetDeltaP(deltaP);
            SetFanRPM(fanRPM);
            SetFanType(fanTypeKey);
        }

        public static HydraulicsFan Create(double inFlow, double deltaP, string fanTypeKey, double fanRPM, double inDim1, double inDim2) 
        {
            return new(inFlow, deltaP, fanTypeKey, fanRPM, inDim1, inDim2);
        }

        public void SetDeltaP(double deltaP)
        {
            if (deltaP < 0)
            {
                throw new ArgumentException("deltaPT can't be less than 0");
            }

            DeltaP = deltaP;
        }

        public void SetFanRPM(double fanRPM)
        {
            if (fanRPM < 0)
            {
                throw new ArgumentException("deltaPT can't be less than 0");
            }

            FanRPM = fanRPM;
        }

        public void SetFanType(string key) 
        {
            FanType = TypicalFanLwMemoryRepo.GetFanData(key);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("\nFan properties:");
            builder.Append($"deltaPT= {DeltaP.RoundUp(2)}");
            builder.Append($"  fanRPM= {FanRPM.RoundUp(0)}");

            return "\n--HYDRAULICS FAN--" + base.ToString() + builder.ToString();
        }

    }
}