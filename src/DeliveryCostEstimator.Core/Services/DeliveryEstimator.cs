using DeliveryCostEstimator.Core.Models;
using DeliveryCostEstimator.Core.Models.RequestModels;

namespace DeliveryCostEstimator.Core.Services;

public sealed class DeliveryEstimator : IDeliveryEstimator
{
    private readonly IPackageCostService _packageCostService;
    private readonly IEtaEstimationService _etaEstimationService;

    public DeliveryEstimator(IPackageCostService packageCostService, IEtaEstimationService etaEstimationService)
    {
        _packageCostService = packageCostService;
        _etaEstimationService = etaEstimationService;
    }

    public List<DeliveryEstimation> EstimateCostsAndDeliveryTime(DeliveryEstimationRequest request)
    {
        var (baseDeliveryCost, eta) = request;
        var packages = eta.Packages;
        var costs = packages
            .Select(p => new { Package = p, Cost = _packageCostService.EstimateCost(baseDeliveryCost, p) })
            .ToDictionary(x => x.Package.Id, x => x.Cost, StringComparer.OrdinalIgnoreCase);

        var etaByPackageId = _etaEstimationService.EstimateEtas(eta);

        return packages
            .Select(p =>
            {
                var cost = costs[p.Id];
                return new DeliveryEstimation
                {
                    PackageId = p.Id,
                    Discount = cost.Discount,
                    TotalCost = cost.TotalCost,
                    EstimatedDeliveryTime = etaByPackageId[p.Id]
                };
            })
            .ToList();
    }
}
