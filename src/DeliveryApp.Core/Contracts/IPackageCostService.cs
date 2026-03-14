using DeliveryApp.Core.Models;

namespace DeliveryApp.Core.Services;

public interface IPackageCostService
{
    CostEstimation EstimateCost(decimal baseDeliveryCost, Package package);
}
