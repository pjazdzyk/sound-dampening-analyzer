using Xunit;
using NoiseAnalyzer.Core.Physics;

namespace NoiseAnalyzer.Tests.Physics
{
    public class PhysicsBranchTests : PhysicsTestConstants
    {
        // Arrange (Shared)

        [Fact]
        public void CalculateStNumberTest()
        {
            // Act
            var actual = PhysicsBranch.CalcStrouhalNum(HydrDiam1, Velocity, FmValue);

            // Assert
            Assert.Equal(37.5, actual);
        }
    }
}
