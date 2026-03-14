using DeliveryCostEstimator.Core.Models;
using DeliveryCostEstimator.Core.Models.RequestModels;

namespace DeliveryCostEstimator.Core.Services;

public interface IDeliveryEstimator
{
    List<DeliveryEstimation> EstimateCostsAndDeliveryTime(DeliveryEstimationRequest request);
}
