using DeliveryCostEstimator.Core.Models;

namespace DeliveryCostEstimator.Core.Config;

public static class OfferConfiguration
{
    public static IReadOnlyList<OfferRule> DefaultOffers { get; } =
        new List<OfferRule>
        {
            new OfferRule
            {
                Code = "OFR001",
                DiscountRatio = 0.10m,
                WeightRange = new NumberRange { Min = 70m, Max = 200m },
                DistanceRange = new NumberRange { Min = 0m, Max = 200m }
            },
            new OfferRule
            {
                Code = "OFR002",
                DiscountRatio = 0.07m,
                WeightRange = new NumberRange { Min = 100m, Max = 250m },
                DistanceRange = new NumberRange { Min = 50m, Max = 150m }
            },
            new OfferRule
            {
                Code = "OFR003",
                DiscountRatio = 0.05m,
                WeightRange = new NumberRange { Min = 10m, Max = 150m },
                DistanceRange = new NumberRange { Min = 50m, Max = 250m }
            }
        }.AsReadOnly();
}
