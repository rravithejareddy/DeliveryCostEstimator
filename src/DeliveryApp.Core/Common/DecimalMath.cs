namespace DeliveryApp.Core.Common;

public static class DecimalMath
{
    public static decimal Truncate(decimal value, int decimals)
    {
        if (decimals < 0 || decimals > 28)
        {
            throw new ArgumentOutOfRangeException(nameof(decimals), "decimals must be between 0 and 28.");
        }

        var factor = 1m;
        for (var i = 0; i < decimals; i++)
        {
            factor *= 10m;
        }

        return Math.Truncate(value * factor) / factor;
    }
}