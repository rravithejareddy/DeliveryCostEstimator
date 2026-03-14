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
        ArgumentNullException.ThrowIfNull(request);

        var (packages, vehicleCount, speed, maxCarriableWeight) = request;
        ArgumentNullException.ThrowIfNull(packages);

        if (vehicleCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request), "Vehicle count must be greater than zero.");
        }

        if (speed <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request), "Vehicle speed must be greater than zero.");
        }

        if (maxCarriableWeight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request), "Max carriable weight must be greater than zero.");
        }

        if (packages.Count == 0)
        {
            return new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        }

        if (packages.Any(p => p.WeightInKg > maxCarriableWeight))
        {
            throw new InvalidOperationException("At least one package exceeds max carriable weight and cannot be assigned to any shipment.");
        }

        var etas = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        var remaining = packages.ToList();
        var vehicleAvailability = new List<decimal>(new decimal[vehicleCount]);

        while (remaining.Count > 0)
        {
            var shipment = _shipmentSelectionService.SelectBestShipment(remaining, maxCarriableWeight);
            if (shipment.Count == 0)
            {
                throw new InvalidOperationException("No valid shipment combination can be formed for remaining packages.");
            }

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
