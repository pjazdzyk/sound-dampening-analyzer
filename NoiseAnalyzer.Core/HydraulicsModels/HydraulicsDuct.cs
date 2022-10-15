using System;
using NoiseAnalyzer.Core.Utils;
using NoiseAnalyzer.Core.Extensions;

namespace NoiseAnalyzer.Core.HydraulicModels
{
    public class HydraulicsDuct : Hydraulics
    {
        // UPSTREAM GEOMETRY: INPUT PROPERTY
        public double InLength { get; private set; } // długość kanału

        private HydraulicsDuct() : this(Defaults.FlowIn, Defaults.Lentgh, Defaults.DimSize, Defaults.DimSize) { }

        public static HydraulicsDuct FromDefaults() 
        {
            return new();
        }

        private HydraulicsDuct(double inFlow, double inLength, double inDim1, double inDim2 = 0.0) 
            : base(inFlow, inDim1, inDim2)
        {
            SetDuctLength(inLength);
        }

        public static HydraulicsDuct Create(double inFlow, double inLength, double inDim1, double inDim2 = 0.0) 
        {
            return new(inFlow, inLength, inDim1, inDim2 = 0.0);
        }

        public void SetDuctLength(double length)
        {
            if (length <= 0)
                throw new ArgumentException();

            InLength = length;
        }

        public override string ToString()
        {
            return "\n--HYDRAULICS DUCT--" + base.ToString() + $"  length= {InLength.RoundUp(2)} \n";
        }

    }
}


