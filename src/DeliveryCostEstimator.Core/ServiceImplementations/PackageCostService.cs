using DeliveryCostEstimator.Core.Models;

namespace DeliveryCostEstimator.Core.Services;

public sealed class PackageCostService : IPackageCostService
{
    private readonly IOfferDiscountService _offerDiscountService;

    public PackageCostService(IOfferDiscountService offerDiscountService)
    {
        _offerDiscountService = offerDiscountService;
    }

    public CostEstimation EstimateCost(decimal baseDeliveryCost, Package package)
    {
        var deliveryCost = baseDeliveryCost + (package.WeightInKg * 10m) + (package.DistanceInKm * 5m);
        var discount = _offerDiscountService.CalculateDiscount(package, deliveryCost);
        return new CostEstimation
        {
            PackageId = package.Id,
            Discount = discount,
            TotalCost = deliveryCost - discount
        };
    }
}
