namespace DeliveryApp.Core.Models;

public class NumberRange
{
    public decimal Min { get; set; }
    public decimal Max { get; set; }

    public bool Includes(decimal value) => value >= Min && value <= Max;
}
