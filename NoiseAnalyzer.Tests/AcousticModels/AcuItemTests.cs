using Xunit;
using System;
using NoiseAnalyzer.Core.Physics;
using NoiseAnalyzer.Core.AcousticModels;
using NoiseAnalyzer.Core.Utils;

namespace NoiseAnalyzer.Test.AcousticModels
{
    public class AcuItemTests
    {
        const double MinAccuracy = 0.00000001;
        static readonly double[] InputLw = new[] { 92.0, 90.0, 88.0, 84.0, 79.0, 74.0, 67.0, 60.0 };
        static readonly double[] SourceLw = new[] { 58.0, 54.0, 49.0, 45.0, 41.0, 37.0, 34.0, 31.0 };
        static readonly double[] SourceDa = new[] { 6.0, 17.0, 34.0, 36.0, 38.0, 29.0, 19.0, 15.0 };
        readonly double[] OutputLw = PhysicsCommon.CalcuOutLwFromSourceLwDa(InputLw, SourceLw, SourceDa);
        readonly string Name = "Test!@#$%^&*()ęążćźół";

        [Fact]
        public void AcuItemDefaultConstructorTests()
        {
            // Arrange
            var expectedName = Defaults.Name;
            var expectedInLw = Defaults.InputLwArray;
            var expectedSrcDa = Defaults.InputLwMinAray;
            var expectedSrcLw = Defaults.InputLwMinAray;
            var expectedOutLw = PhysicsCommon.CalcuOutLwFromSourceLwDa(expectedInLw, expectedSrcLw, expectedSrcDa);

            // Act
            var acuItem = new AcuItem();
            var actName = acuItem.Name;
            var actInLw = acuItem.InputLw;
            var actSrcDa = acuItem.SourceDa;
            var actSrcLw = acuItem.SourceLw;
            var actOutLw = acuItem.OutputLw;

            // Assert
            Assert.NotNull(acuItem);
            Assert.Equal(expectedName, actName);
            Assert.Equal(expectedInLw, actInLw);
            Assert.Equal(expectedSrcDa, actSrcDa);
            Assert.Equal(expectedSrcLw, actSrcLw);
            Assert.Equal(expectedOutLw, actOutLw);
        }

        [Fact]
        public void AcuItemConstructorTests() 
        {
            // Arrange
            var expectedName = Name;
            var expectedInLw = InputLw;
            var expectedSrcDa = SourceDa;
            var expectedSrcLw = SourceLw;
            var expectedOutLw = OutputLw;

            // Act
            var acuItem = new AcuItem(Name, InputLw, SourceLw, SourceDa);
            var actName = acuItem.Name;
            var actInLw = acuItem.InputLw;
            var actSrcDa = acuItem.SourceDa;
            var actSrcLw = acuItem.SourceLw;
            var actOutLw = acuItem.OutputLw;

            // Assert
            Assert.NotNull(acuItem);
            Assert.Equal(expectedName, actName);
            Assert.Equal(expectedInLw, actInLw);
            Assert.Equal(expectedSrcDa, actSrcDa);
            Assert.Equal(expectedSrcLw, actSrcLw);
            Assert.Equal(expectedOutLw, actOutLw);   
        }
        
        [Fact]
        public void AcuItemEnsureMinLwTest()
        {
            // Arrange
            var inputLw = new[] { 300.0, 90.0, 250.0, 84.0, 0.0, 2.0, 10.0, 500.0 };
            var expectedInLw = new[] { Defaults.LwMax, 90.0, Defaults.LwMax, 84.0, Defaults.LwBackr, Defaults.LwBackr, Defaults.LwBackr, Defaults.LwMax };
            var expectedSrcDa = SourceDa;
            var expectedSrcLw = SourceLw;
            var expectedOutLw = PhysicsCommon.CalcuOutLwFromSourceLwDa(expectedInLw, expectedSrcLw, expectedSrcDa);

            // Act
            var acuItem = new AcuItem(Name, inputLw, expectedSrcLw, expectedSrcDa);
            var actInLw = acuItem.InputLw;
            var actOutLw = acuItem.OutputLw;

            // Assert
            Assert.NotNull(acuItem);
            Assert.DoesNotContain(actOutLw, x => x < 0);
            Assert.Equal(expectedInLw, actInLw);
            Assert.Equal(expectedOutLw, actOutLw);
        }

        [Fact]
        public void AcuItemEnsureMinLwExceptionsTest() {

            // Arrange
            var inputLw = new[] { 300.0, 90.0, 250.0, 84.0, 0.0, 2.0, -10.0, 500.0 };
            var expectedInLw = new[] { Defaults.LwMax, 90.0, Defaults.LwMax, 84.0, Defaults.LwBackr, Defaults.LwBackr, Defaults.LwBackr, Defaults.LwMax };
            var expectedSrcDa = SourceDa;
            var expectedSrcLw = SourceLw;

            // Assert
            Assert.Throws<ArgumentException>(() => new AcuItem(Name, inputLw, expectedSrcLw, expectedSrcDa));
        }

        [Fact]
        public void AcuItemSettersTest()
        {
            // Arrange
            var expectedName = Name;
            var expectedInLw = InputLw;
            var expectedSrcDa = SourceDa;
            var expectedSrcLw = SourceLw;
            var expectedOutLw = OutputLw;

            // Act
            var acuItem = new AcuItem();
            acuItem.SetInputLw(expectedInLw);
            acuItem.SetSourceLw(expectedSrcLw);
            acuItem.SetSourceDa(expectedSrcDa);
            var actInLw = acuItem.InputLw;
            var actSrcLw = acuItem.SourceLw;
            var actSrcDa = acuItem.SourceDa;
            var actOutLw = acuItem.OutputLw;

            // Assert
            Assert.Equal(expectedInLw, actInLw);
            Assert.Equal(expectedSrcLw, actSrcLw);
            Assert.Equal(expectedSrcDa, actSrcDa);
            Assert.DoesNotContain(actOutLw, x => x < 0);
            Assert.Equal(expectedOutLw, actOutLw);
        }

        [Fact]
        public void AcuItemSettersExceptionsTest() 
        {
            // Arrange
            var testArray1 = new[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0};
            var testArray2 = new[] {1.0, 2.0, 3.0};

            // Act
            var acuItem = new AcuItem();

            // Assert
            Assert.ThrowsAny<Exception>(() => acuItem.SetInputLw(null));
            Assert.ThrowsAny<Exception>(() => acuItem.SetSourceDa(null));
            Assert.ThrowsAny<Exception>(() => acuItem.SetSourceLw(null));

            Assert.ThrowsAny<Exception>(() => acuItem.SetInputLw(testArray1));
            Assert.ThrowsAny<Exception>(() => acuItem.SetSourceDa(testArray1));
            Assert.ThrowsAny<Exception>(() => acuItem.SetSourceLw(testArray1));

            Assert.ThrowsAny<Exception>(() => acuItem.SetInputLw(testArray2));
            Assert.ThrowsAny<Exception>(() => acuItem.SetSourceDa(testArray2));
            Assert.ThrowsAny<Exception>(() => acuItem.SetSourceLw(testArray2));
        }
    }
}
