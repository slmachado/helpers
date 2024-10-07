using FluentAssertions;
using Helpers;
using Xunit;

namespace Helpers.Tests
{
    public class FloatingNumberHelperTests
    {
        [Fact]
        public void NearlyEqual_DoubleValuesWithinTolerance_ReturnsTrue()
        {
            // Arrange
            double value = 0.000001;
            double compareTo = 0.000002;

            // Act
            bool result = value.NearlyEqual(compareTo, 0.00001);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void NearlyEqual_DoubleValuesOutsideTolerance_ReturnsFalse()
        {
            // Arrange
            double value = 0.000001;
            double compareTo = 0.01;

            // Act
            bool result = value.NearlyEqual(compareTo, 0.000001);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void NearlyEqual_NullableDoublesBothNull_ReturnsTrue()
        {
            // Arrange
            double? value = null;
            double? compareTo = null;

            // Act
            bool result = value.NearlyEqual(compareTo);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void NearlyEqual_NullableDoubleOneNull_ReturnsFalse()
        {
            // Arrange
            double? value = 0.1;
            double? compareTo = null;

            // Act
            bool result = value.NearlyEqual(compareTo);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void NearlyEqual_NullableDoublesWithinDefaultTolerance_ReturnsTrue()
        {
            // Arrange
            double? value = 0.000001;
            double? compareTo = 0.000002;

            // Act
            bool result = value.NearlyEqual(compareTo);

            // Assert
            result.Should().BeTrue(); // Com o epsilon aumentado, isso deve agora passar
        }

        [Fact]
        public void NearlyZero_DoubleValueWithinTolerance_ReturnsTrue()
        {
            // Arrange
            double value = 0.000001;

            // Act
            bool result = value.NearlyZero();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void NearlyZero_DoubleValueOutsideTolerance_ReturnsFalse()
        {
            // Arrange
            double value = 0.01;

            // Act
            bool result = value.NearlyZero();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void NearlyZero_NullableDoubleValueWithinTolerance_ReturnsTrue()
        {
            // Arrange
            double? value = 0.000001;

            // Act
            bool result = value.NearlyZero();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void NearlyZeroOrNull_NullValue_ReturnsTrue()
        {
            // Arrange
            double? value = null;

            // Act
            bool result = value.NearlyZeroOrNull();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void NearlyZeroOrNull_ValueWithinTolerance_ReturnsTrue()
        {
            // Arrange
            double? value = 0.000001;

            // Act
            bool result = value.NearlyZeroOrNull();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void NearlyEqual_FloatValuesWithinTolerance_ReturnsTrue()
        {
            // Arrange
            float value = 0.000001f;
            float compareTo = 0.000002f;

            // Act
            bool result = value.NearlyEqual(compareTo);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void NearlyZero_FloatValueWithinTolerance_ReturnsTrue()
        {
            // Arrange
            float value = 0.000001f;

            // Act
            bool result = value.NearlyZero();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GreaterThanOrNearlyEqual_ValueGreaterThanCompareTo_ReturnsTrue()
        {
            // Arrange
            double value = 10.000001;
            double compareTo = 10.0;

            // Act
            bool result = value.GreaterThanOrNearlyEqual(compareTo);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GreaterThanOrNearlyEqual_ValueLessThanCompareTo_ReturnsFalse()
        {
            // Arrange
            double value = 9.999;
            double compareTo = 10.0;

            // Act
            bool result = value.GreaterThanOrNearlyEqual(compareTo);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void LessThanOrNearlyEqual_ValueLessThanCompareTo_ReturnsTrue()
        {
            // Arrange
            double value = 9.999999;
            double compareTo = 10.0;

            // Act
            bool result = value.LessThanOrNearlyEqual(compareTo);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void LessThanOrNearlyEqual_ValueGreaterThanCompareTo_ReturnsFalse()
        {
            // Arrange
            double value = 10.001;
            double compareTo = 10.0;

            // Act
            bool result = value.LessThanOrNearlyEqual(compareTo);

            // Assert
            result.Should().BeFalse();
        }
    }
}
