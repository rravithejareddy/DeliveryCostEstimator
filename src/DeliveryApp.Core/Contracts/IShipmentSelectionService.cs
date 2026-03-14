using DeliveryApp.Core.Models;

namespace DeliveryApp.Core.Services;

public interface IShipmentSelectionService
{
    List<Package> SelectBestShipment(List<Package> remainingPackages, decimal maxCarriableWeight);
}
