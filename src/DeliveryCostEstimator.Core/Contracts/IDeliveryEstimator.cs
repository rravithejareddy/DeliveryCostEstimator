using DeliveryCostEstimator.Core.Models;
using DeliveryCostEstimator.Core.Models.RequestModels;

namespace DeliveryCostEstimator.Core.Services;

public interface IDeliveryEstimator
{
    List<CostEstimation> EstimateCosts(decimal baseDeliveryCost, List<Package> packages);

    List<DeliveryEstimation> EstimateCostsAndDeliveryTime(DeliveryEstimationRequest request);
}
