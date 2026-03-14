namespace DeliveryCostEstimator.Core.Models.RequestModels;

public record DeliveryEstimationRequest(
    decimal BaseDeliveryCost,
    EtaEstimationRequest Eta
);