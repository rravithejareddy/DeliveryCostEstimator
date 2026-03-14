using DeliveryApp.Core.Models;

namespace DeliveryApp.Core.Models.RequestModels;

public class EtaEstimationRequest
{
    public List<Package> Packages { get; set; } = new();
    public int VehicleCount { get; set; }
    public decimal MaxSpeedKmPerHour { get; set; }
    public decimal MaxCarriableWeight { get; set; }
}