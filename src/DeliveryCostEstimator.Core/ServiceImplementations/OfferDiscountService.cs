using DeliveryCostEstimator.Core.Models;

namespace DeliveryCostEstimator.Core.Services;

public sealed class OfferDiscountService : IOfferDiscountService
{
    private readonly Dictionary<string, OfferRule> _offersByCode;

    public OfferDiscountService(IEnumerable<OfferRule> offers)
    {
        _offersByCode = offers.ToDictionary(o => o.Code, StringComparer.OrdinalIgnoreCase);
    }

    public decimal CalculateDiscount(Package package, decimal deliveryCost)
    {
        if (string.IsNullOrEmpty(package.OfferCode))
        {
            return 0m;
        }

        if (!_offersByCode.TryGetValue(package.OfferCode, out var offer))
        {
            return 0m;
        }

        if (!offer.IsApplicable(package))
        {
            return 0m;
        }

        return deliveryCost * offer.DiscountRatio;
    }
}
