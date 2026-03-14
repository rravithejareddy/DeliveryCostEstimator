using DeliveryCostEstimator.Core.Models.RequestModels;

namespace DeliveryCostEstimator.Core.Services;

public interface IEtaEstimationService
{
    Dictionary<string, decimal> EstimateEtas(EtaEstimationRequest request);
}
