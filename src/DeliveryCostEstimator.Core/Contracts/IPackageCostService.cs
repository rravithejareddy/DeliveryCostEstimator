using DeliveryCostEstimator.Core.Models;

namespace DeliveryCostEstimator.Core.Services;

public interface IPackageCostService
{
    CostEstimation EstimateCost(decimal baseDeliveryCost, Package package);
}
