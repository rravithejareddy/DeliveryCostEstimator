using DeliveryCostEstimator.Core.Models;

namespace DeliveryCostEstimator.Core.Services;

public sealed class ShipmentSelectionService : IShipmentSelectionService
{
    public List<Package> SelectBestShipment(List<Package> remainingPackages, decimal maxCarriableWeight)
    {
        var candidates = new List<List<Package>>();
        BuildCandidates(remainingPackages, maxCarriableWeight, 0, new List<Package>(), candidates);

        return candidates
            .OrderByDescending(c => c.Count)
            .ThenByDescending(c => c.Sum(p => p.WeightInKg))
            .ThenBy(c => c.Max(p => p.DistanceInKm))
            .ThenBy(c => string.Join("|", c.Select(p => p.Id).OrderBy(id => id, StringComparer.OrdinalIgnoreCase)))
            .First();
    }

    private static void BuildCandidates(
        List<Package> allPackages,
        decimal maxCarriableWeight,
        int index,
        List<Package> current,
        List<List<Package>> candidates)
    {
        if (index == allPackages.Count)
        {
            if (current.Count > 0)
            {
                candidates.Add(current.ToList());
            }

            return;
        }

        BuildCandidates(allPackages, maxCarriableWeight, index + 1, current, candidates);

        var package = allPackages[index];
        var currentWeight = current.Sum(p => p.WeightInKg);
        if (currentWeight + package.WeightInKg > maxCarriableWeight)
        {
            return;
        }

        current.Add(package);
        BuildCandidates(allPackages, maxCarriableWeight, index + 1, current, candidates);
        current.RemoveAt(current.Count - 1);
    }
}
