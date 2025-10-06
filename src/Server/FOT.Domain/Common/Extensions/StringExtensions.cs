namespace FOT.Domain.Common.Extensions;

internal static class StringExtensions
{
    public static void ThrowIfLengthOutOfRange(this string str, int min, int max)
    {
        if (str.Length < min || str.Length > max)
        {
            throw new ArgumentException($"{nameof(str)} length must be between {min} and {max}.");
        }
    }
}