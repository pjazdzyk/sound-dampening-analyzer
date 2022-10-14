using SoundDoc.Core.Extensions;
using Xunit;

namespace NoiseAnalyzer.Tests.Extensions
{
    public class ArrayExtensionsTests
    {

        public const double MathAccuracy = 0.00000001;

        [Fact]
        public void LogSumTest()
        {
            // Arrange
            var value1 = 20.0;
            var value2 = 23.0;
            var value3 = 0.0;
            var value4 = 0.0;
            var expected1 = 24.7643486243649;
            var expected2 = 0.0;

            // Act
            var actual1 = value1.LogSum(value2);
            var actual2 = value3.LogSum(value4);
            var actual4 = value1.LogSum(value3);
            var actual5 = value3.LogSum(value1);

            // Assert
            Assert.Equal(expected1, actual1, 13);
            Assert.Equal(expected2, actual2, 13);
            Assert.Equal(value1, actual4);
            Assert.Equal(value1, actual5);

        }

        [Fact]
        public void LogSumArrayTest()
        {
            // Arrange
            var input1 = new[] { 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0 };
            var expected1 = 19.0308998699194;
            var input2 = new[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            var expected2 = 0.0;
            var input3 = new[] { 10.0, 10.0, 10.0, 10.0, 0.0, 0.0, 0.0, 0.0 };
            var expected3 = 16.0205999132796;

            // Act
            var actual1 = input1.LogSum();
            var actual2 = input2.LogSum();
            var actual3 = input3.LogSum();

            // Assert
            Assert.Equal(expected1, actual1, 13);
            Assert.Equal(expected2, actual2, 13);
            Assert.Equal(expected3, actual3, 13);

        }

        [Fact]
        public void CompareWithPrecisionTests()
        {
            // Arrange   
            var array1 = new[] { 86.00687765, 73.05433314, 55.19331048, 49.76434862, 44.01029996, 45.63892034, 48.16954289, 45.16954289 };
            var array2 = new[] { 86.00687766, 73.05433313, 55.19331049, 49.76434863, 44.01029995, 45.63892033, 48.16954287, 45.16954290 };
            var minAccuracy = 0.0000001;

            // Act
            var result1 = array1 == array2;
            var result2 = array1.EqualsToPrecision(array2, minAccuracy);

            // Assert
            Assert.False(result1);
            Assert.True(result2);

        }

        [Fact]
        public void LogSumTwoArraysTests()
        {
            // Arrange
            var input1 = new[] { 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0 };
            var input2 = new[] { 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0 };
            var input3 = new[] { 10.0, 0.0, 10.0, 0.0, 10.0, 0.0, 10.0, 0.0 };
            var input4 = new[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };

            var expected1 = new[] { 13.01029995, 13.01029995, 13.01029995, 13.01029995, 13.01029995, 13.01029995, 13.01029995, 13.01029995 }; //input1 + input2
            var expected2 = new[] { 13.01029995, 10.0, 13.01029995, 10.0, 13.01029995, 10.0, 13.01029995, 10.0 };  //input1 + input3 
            var expected3 = new[] { 10.0, 0.0, 10.0, 0.0, 10.0, 0.0, 10.0, 0.0 };  //input3 + input4 
            var expected4 = new[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };  //input4 + input4 

            // Act
            var actual1 = input1.LogSumTwoArrays(input2);
            var actual2 = input1.LogSumTwoArrays(input3);
            var actual3 = input3.LogSumTwoArrays(input4);
            var actual4 = input4.LogSumTwoArrays(input4);

            // Assert
            Assert.True(expected1.EqualsToPrecision(actual1, MathAccuracy));
            Assert.True(expected2.EqualsToPrecision(actual2, MathAccuracy));
            Assert.True(expected3.EqualsToPrecision(actual3, MathAccuracy));
            Assert.True(expected4.EqualsToPrecision(actual4, MathAccuracy));

        }


    }




}