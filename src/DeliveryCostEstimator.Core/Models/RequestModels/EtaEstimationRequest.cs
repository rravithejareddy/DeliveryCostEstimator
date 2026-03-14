using DeliveryCostEstimator.Core.Models;

namespace DeliveryCostEstimator.Core.Models.RequestModels;

public record EtaEstimationRequest(
    List<Package> Packages,
    int VehicleCount,
    decimal MaxSpeedKmPerHour,
    decimal MaxCarriableWeight
);