using DeliveryApp.Core.Models.RequestModels;

namespace DeliveryApp.Core.Services;

public interface IEtaEstimationService
{
    Dictionary<string, decimal> EstimateEtas(EtaEstimationRequest request);
}
