namespace DeliveryCostEstimator.Core.Models;

public class OfferRule
{
    public required string Code { get; set; }
    public decimal DiscountRatio { get; set; }
    public required NumberRange WeightRange { get; set; }
    public required NumberRange DistanceRange { get; set; }

    public bool IsApplicable(Package package) =>
        WeightRange.Includes(package.WeightInKg) &&
        DistanceRange.Includes(package.DistanceInKm);
}
