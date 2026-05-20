namespace ArturRios.Extensions.Tests;

public class IntExtensionsTests
{
    [Theory]
    [InlineData(-5, false)]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    [InlineData(17, true)]
    [InlineData(18, false)]
    [InlineData(7919, true)]
    public void GivenInt_WhenCheckingIsPrime_ThenReturnsExpectedResult(int input, bool expected) => Assert.Equal(expected, input.IsPrime());

    [Theory]
    [InlineData(2L, true)]
    [InlineData(3L, true)]
    [InlineData(4L, false)]
    [InlineData(1000000007L, true)]
    [InlineData(1000000008L, false)]
    public void GivenLong_WhenCheckingIsPrime_ThenReturnsExpectedResult(long input, bool expected) => Assert.Equal(expected, input.IsPrime());

    [Theory]
    [InlineData((short)-3, false)]
    [InlineData((short)2, true)]
    [InlineData((short)4, false)]
    public void GivenShort_WhenCheckingIsPrime_ThenReturnsExpectedResult(short input, bool expected) => Assert.Equal(expected, input.IsPrime());

    [Theory]
    [InlineData((byte)0, false)]
    [InlineData((byte)2, true)]
    [InlineData((byte)9, false)]
    public void GivenByte_WhenCheckingIsPrime_ThenReturnsExpectedResult(byte input, bool expected) => Assert.Equal(expected, input.IsPrime());

    [Theory]
    [InlineData((sbyte)-7, false)]
    [InlineData((sbyte)3, true)]
    public void GivenSByte_WhenCheckingIsPrime_ThenReturnsExpectedResult(sbyte input, bool expected) => Assert.Equal(expected, input.IsPrime());

    [Theory]
    [InlineData((ushort)2, true)]
    [InlineData((ushort)100, false)]
    public void GivenUShort_WhenCheckingIsPrime_ThenReturnsExpectedResult(ushort input, bool expected) => Assert.Equal(expected, input.IsPrime());

    [Theory]
    [InlineData(65537u, true)]
    [InlineData(65536u, false)]
    public void GivenUInt_WhenCheckingIsPrime_ThenReturnsExpectedResult(uint input, bool expected) => Assert.Equal(expected, input.IsPrime());

    [Theory]
    [InlineData(1000000007UL, true)]
    [InlineData(1000000008UL, false)]
    public void GivenULong_WhenCheckingIsPrime_ThenReturnsExpectedResult(ulong input, bool expected) => Assert.Equal(expected, input.IsPrime());
}

