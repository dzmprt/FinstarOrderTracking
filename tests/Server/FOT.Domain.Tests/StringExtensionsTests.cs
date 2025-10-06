using FOT.Domain.Common.Extensions;

namespace FOT.Domain.Tests;

public class StringExtensionsTests
{
    [Fact]
    public void ThrowIfLengthOutOfRange_ValidLength_DoesNotThrow()
    {
        var str = "abc";
        str.ThrowIfLengthOutOfRange(1, 5);
    }

    [Theory]
    [InlineData("")]
    [InlineData("abcdef")]
    public void ThrowIfLengthOutOfRange_InvalidLength_Throws(string str)
    {
        Assert.Throws<ArgumentException>(() => str.ThrowIfLengthOutOfRange(1, 5));
    }
}