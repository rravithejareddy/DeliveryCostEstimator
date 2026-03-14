using DeliveryCostEstimator.Core.Models;

namespace DeliveryCostEstimator.Core.Services;

public interface IOfferDiscountService
{
    decimal CalculateDiscount(Package package, decimal deliveryCost);
}
