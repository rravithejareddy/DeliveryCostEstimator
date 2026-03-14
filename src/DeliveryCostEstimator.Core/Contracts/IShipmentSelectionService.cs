using DeliveryCostEstimator.Core.Models;

namespace DeliveryCostEstimator.Core.Services;

public interface IShipmentSelectionService
{
    List<Package> SelectBestShipment(List<Package> remainingPackages, decimal maxCarriableWeight);
}
