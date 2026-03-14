using DeliveryApp.Core.Models;
using DeliveryApp.Core.Models.RequestModels;

namespace DeliveryApp.Core.Services;

public interface IDeliveryEstimator
{
    List<CostEstimation> EstimateCosts(decimal baseDeliveryCost, List<Package> packages);

    List<DeliveryEstimation> EstimateCostsAndDeliveryTime(DeliveryEstimationRequest request);
}
