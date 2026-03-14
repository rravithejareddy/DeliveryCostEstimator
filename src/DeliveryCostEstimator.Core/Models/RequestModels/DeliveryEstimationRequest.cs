namespace DeliveryCostEstimator.Core.Models.RequestModels;

public class DeliveryEstimationRequest
{
    public decimal BaseDeliveryCost { get; set; }
    public EtaEstimationRequest Eta { get; set; } = new();
}