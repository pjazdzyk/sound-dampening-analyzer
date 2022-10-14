using System;
using System.Linq;

namespace SoundDoc.Core.Extensions
{
    public static class ArrayExtensions
    {
        public static string Format(this double[] inputArray, int decimals = 1) => $"[{string.Join("  ", inputArray.Select(x => x.RoundUp(decimals)))}]";

        public static double LogSum(this double[] input)
        {
            if (input.Sum() == 0)
                return 0.0;

            return 10.0 * Math.Log10(input.Where(val => val > 0.0).Sum(val => Math.Pow(10.0, 0.1 * val)));
        } 

        public static double[] LogSumTwoArrays(this double[] firstArray, double[] secondArray) 
        {
            if (firstArray.Length != Defaults.NumberOfOctaves && secondArray.Length != Defaults.NumberOfOctaves)
                throw new ArgumentException("Invalid array size");

            if (firstArray == null || secondArray == null)
                throw new ArgumentNullException("Arrays cannot be null");

            double[] result = new double[8];

            for (int i = 0; i < firstArray.Length; i++)
            {
                result[i] = firstArray[i].LogSum(secondArray[i]);
            }
            return result;
        }

        public static bool EqualsToPrecision(this double[] input, double[] compared, double accuracy)
        {
            for (int i = 0; i < input.Length; i++)
            {
                double tempDifference = Math.Abs(input[i] - compared[i]);
                if (tempDifference > accuracy)
                    return false;
            }
            return true;
        }

        public static bool ContainsNegative(this double[] input) => input.Any(value => value < 0);

    }
}