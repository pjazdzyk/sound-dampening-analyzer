using System;

namespace SoundDoc.Core.Extensions
{
    public static class DoubleExtensions
    {
        public static double RoundUp(this double input, int decimals = 1)
        {
            var multiplier = Math.Pow(10, decimals);
            return Math.Ceiling(input * multiplier) / multiplier;
        }

        public static double LogSum(this double firstValue, double secondValue)
        {
            if (firstValue == 0.0 && secondValue == 0.0) return 0.0;
            if (firstValue == 0.0 && secondValue != 0.0) return secondValue;
            if (firstValue != 0.0 && secondValue == 0.0) return firstValue;

            return 10.0 * Math.Log10(Math.Pow(10.0, 0.1 * firstValue) + Math.Pow(10, 0.1 * secondValue));

        }
    }
}