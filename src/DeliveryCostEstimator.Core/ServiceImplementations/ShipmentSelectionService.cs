using DeliveryCostEstimator.Core.Models;

namespace DeliveryCostEstimator.Core.Services;

public sealed class ShipmentSelectionService : IShipmentSelectionService
{
    public List<Package> SelectBestShipment(List<Package> remainingPackages, decimal maxCarriableWeight)
    {
        ArgumentNullException.ThrowIfNull(remainingPackages);

        if (maxCarriableWeight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxCarriableWeight), "Max carriable weight must be greater than zero.");
        }

        if (remainingPackages.Count == 0)
        {
            return [];
        }

        var validCombinations = new List<List<Package>>();
        BuildValidPackageCombinations(remainingPackages, maxCarriableWeight, validCombinations);

        if (validCombinations.Count == 0)
        {
            throw new InvalidOperationException("No valid shipment combination can be created with the provided weight limit.");
        }

        return validCombinations
            .OrderByDescending(c => c.Count)
            .ThenByDescending(c => c.Sum(p => p.WeightInKg))
            .ThenBy(c => c.Max(p => p.DistanceInKm))
            .ThenBy(c => string.Join("|", c.Select(p => p.Id).OrderBy(id => id, StringComparer.OrdinalIgnoreCase)))
            .First();
    }

    private static void BuildValidPackageCombinations(
        List<Package> allPackages,
        decimal maxCarriableWeight,
        List<List<Package>> validCombinations)
    {
        BuildValidPackageCombinations(allPackages, maxCarriableWeight, 0, 0m, new List<Package>(), validCombinations);
    }

    private static void BuildValidPackageCombinations(
        List<Package> allPackages,
        decimal maxCarriableWeight,
        int packageIndex,
        decimal currentWeight,
        List<Package> currentCombination,
        List<List<Package>> validCombinations)
    {
        if (packageIndex == allPackages.Count)
        {
            if (currentCombination.Count > 0)
            {
                validCombinations.Add(currentCombination.ToList());
            }

            return;
        }

        BuildValidPackageCombinations(
            allPackages,
            maxCarriableWeight,
            packageIndex + 1,
            currentWeight,
            currentCombination,
            validCombinations);

        var package = allPackages[packageIndex];
        if (currentWeight + package.WeightInKg > maxCarriableWeight)
        {
            return;
        }

        currentCombination.Add(package);
        BuildValidPackageCombinations(
            allPackages,
            maxCarriableWeight,
            packageIndex + 1,
            currentWeight + package.WeightInKg,
            currentCombination,
            validCombinations);
        currentCombination.RemoveAt(currentCombination.Count - 1);
    }
}
