namespace FOT.Domain.Common.Extensions;

public static class IntExtensions
{
    public static void ThrowIfOutOfRange(this int value, int min, int max)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(nameof(value), value, $"Value must be in range [{min}, {max}].");
    }
    //
    // public static void MustBeGreaterThan(this int value, int min)
    // {
    //     if (value <= min)
    //         throw new ArgumentOutOfRangeException(nameof(value), value, $"Value must be greater than {min}.");
    // }
    //
    // public static void MustBeLessThan(this int value, int max)
    // {
    //     if (value >= max)
    //         throw new ArgumentOutOfRangeException(nameof(value), value, $"Value must be less than {max}.");
    // }
}