using FluentAssertions;

namespace Helpers.Tests;

public class BinaryHelperTests
{
    #region Convert to binary string tests

    [Fact]
    public void ToBinary_LongValue_ShouldReturn64BitBinaryString()
    {
        long value = 5;
        value.ToBinary().Should().Be("0000000000000000000000000000000000000000000000000000000000000101");
    }

    [Fact]
    public void ToBinary_IntValue_ShouldReturn32BitBinaryString()
    {
        int value = 5;
        value.ToBinary().Should().Be("00000000000000000000000000000101");
    }

    [Fact]
    public void ToBinary_ShortValue_ShouldReturn16BitBinaryString()
    {
        short value = 5;
        value.ToBinary().Should().Be("0000000000000101");
    }

    #endregion

    #region Get Value from numeric word tests

    [Fact]
    public void GetBitValue_LongValue_ValidPosition_ShouldReturnCorrectBitValue()
    {
        long value = 5; // 000...0101 (64-bit)
        value.GetBitValue(0).Should().BeTrue();  // Bit at position 0 is 1
        value.GetBitValue(2).Should().BeTrue();  // Bit at position 2 is 1
        value.GetBitValue(1).Should().BeFalse(); // Bit at position 1 is 0
    }

    [Fact]
    public void GetBitValue_LongValue_InvalidPosition_ShouldThrowArgumentOutOfRangeException()
    {
        long value = 5;
        Action act = () => value.GetBitValue(64);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GetBitValue_IntValue_ValidPosition_ShouldReturnCorrectBitValue()
    {
        int value = 5; // 000...0101 (32-bit)
        value.GetBitValue(0).Should().BeTrue();  // Bit at position 0 is 1
        value.GetBitValue(2).Should().BeTrue();  // Bit at position 2 is 1
        value.GetBitValue(1).Should().BeFalse(); // Bit at position 1 is 0
    }

    [Fact]
    public void GetBitValue_ShortValue_ValidPosition_ShouldReturnCorrectBitValue()
    {
        short value = 5; // 0000...0101 (16-bit)
        value.GetBitValue(0).Should().BeTrue();  // Bit at position 0 is 1
        value.GetBitValue(2).Should().BeTrue();  // Bit at position 2 is 1
        value.GetBitValue(1).Should().BeFalse(); // Bit at position 1 is 0
    }

    [Fact]
    public void GetBitsValue_LongValue_ValidRange_ShouldReturnCorrectValue()
    {
        long value = 29; // 000...00011101 (64-bit)
        value.GetBitsValue(0, 3).Should().Be(5);  // Bits 0-3 are 101 (5)
        value.GetBitsValue(3, 5).Should().Be(3);  // Bits 3-5 are 11 (3)
    }

    [Fact]
    public void GetBitsValue_LongValue_InvalidRange_ShouldThrowArgumentOutOfRangeException()
    {
        long value = 29;
        Action act = () => value.GetBitsValue(5, 65);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region Set and Toggle Bit Value tests

    [Fact]
    public void SetBitValue_LongValue_ShouldSetBitToOne()
    {
        long value = 0; // All bits are 0
        value.SetBitValue(3, true).Should().Be(8); // Set bit 3 to 1 -> 1000 (8)
    }

    [Fact]
    public void SetBitValue_LongValue_ShouldSetBitToZero()
    {
        long value = 15; // All bits are 1 (1111)
        value.SetBitValue(1, false).Should().Be(13); // Set bit 1 to 0 -> 1101 (13)
    }

    [Fact]
    public void ToggleBitValue_LongValue_ShouldToggleBit()
    {
        long value = 0; // All bits are 0
        value.ToggleBitValue(3).Should().Be(8); // Toggle bit 3 -> 1000 (8)
        value = 8;
        value.ToggleBitValue(3).Should().Be(0); // Toggle bit 3 again -> 0000 (0)
    }

    [Fact]
    public void SetBitValue_LongValue_InvalidPosition_ShouldThrowArgumentOutOfRangeException()
    {
        long value = 5;
        Action act = () => value.SetBitValue(64, true);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion
}