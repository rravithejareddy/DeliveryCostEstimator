using DeliveryCostEstimator.Core.Common;
using DeliveryCostEstimator.Core.Models;
using DeliveryCostEstimator.Core.Models.RequestModels;

namespace DeliveryCostEstimator.Core.Services;

public sealed class EtaEstimationService : IEtaEstimationService
{
    private readonly IShipmentSelectionService _shipmentSelectionService;

    public EtaEstimationService(IShipmentSelectionService shipmentSelectionService)
    {
        _shipmentSelectionService = shipmentSelectionService;
    }

    public Dictionary<string, decimal> EstimateEtas(EtaEstimationRequest request)
    {
        var packages = request.Packages;
        var vehicleCount = request.VehicleCount;
        var speed = request.MaxSpeedKmPerHour;
        var maxCarriableWeight = request.MaxCarriableWeight;

        var etas = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        var remaining = packages.ToList();
        var vehicleAvailability = Enumerable.Repeat(0m, vehicleCount).ToList();

        while (remaining.Count > 0)
        {
            var shipment = _shipmentSelectionService.SelectBestShipment(remaining, maxCarriableWeight);
            var selectedVehicle = NextAvailableVehicle(vehicleAvailability);
            var departureTime = vehicleAvailability[selectedVehicle];
            var shipmentMaxDistance = shipment.Max(p => p.DistanceInKm);
            var oneWayHours = DecimalMath.Truncate(shipmentMaxDistance / speed, 2);

            foreach (var package in shipment)
            {
                var travelHours = DecimalMath.Truncate(package.DistanceInKm / speed, 2);
                var eta = departureTime + travelHours;
                etas[package.Id] = DecimalMath.Truncate(eta, 2);
                remaining.Remove(package);
            }

            vehicleAvailability[selectedVehicle] = DecimalMath.Truncate(departureTime + (2m * oneWayHours), 2);
        }

        return etas;
    }

    private static int NextAvailableVehicle(List<decimal> availability)
    {
        var minValue = availability.Min();
        for (var i = 0; i < availability.Count; i++)
        {
            if (availability[i] == minValue)
            {
                return i;
            }
        }

        return 0;
    }
}
