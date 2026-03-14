using DeliveryApp.Core.Models;
using DeliveryApp.Core.Models.RequestModels;
using DeliveryApp.Core.Services;

namespace DeliveryApp.Tests;

public class DeliveryEstimatorTests
{
    private static readonly OfferRule[] DefaultOffers =
    [
        new OfferRule
        {
            Code = "OFR001",
            DiscountRatio = 0.10m,
            WeightRange = new NumberRange { Min = 70m, Max = 200m },
            DistanceRange = new NumberRange { Min = 0m, Max = 200m }
        },
        new OfferRule
        {
            Code = "OFR002",
            DiscountRatio = 0.07m,
            WeightRange = new NumberRange { Min = 100m, Max = 250m },
            DistanceRange = new NumberRange { Min = 50m, Max = 150m }
        },
        new OfferRule
        {
            Code = "OFR003",
            DiscountRatio = 0.05m,
            WeightRange = new NumberRange { Min = 10m, Max = 150m },
            DistanceRange = new NumberRange { Min = 50m, Max = 250m }
        }
    ];

    [Fact]
    public void CostEstimation_MatchesSampleInput1()
    {
        var estimator = CreateEstimator();

        var packages = new List<Package>
        {
            new Package { Id = "PKG1", WeightInKg = 5m, DistanceInKm = 5m, OfferCode = "OFR001" },
            new Package { Id = "PKG2", WeightInKg = 15m, DistanceInKm = 5m, OfferCode = "OFR002" },
            new Package { Id = "PKG3", WeightInKg = 10m, DistanceInKm = 100m, OfferCode = "OFR003" }
        };

        var result = estimator.EstimateCosts(baseDeliveryCost: 100m, packages);

        Assert.Collection(result,
            item =>
            {
                Assert.Equal("PKG1", item.PackageId);
                Assert.Equal(0m, item.Discount);
                Assert.Equal(175m, item.TotalCost);
            },
            item =>
            {
                Assert.Equal("PKG2", item.PackageId);
                Assert.Equal(0m, item.Discount);
                Assert.Equal(275m, item.TotalCost);
            },
            item =>
            {
                Assert.Equal("PKG3", item.PackageId);
                Assert.Equal(35m, item.Discount);
                Assert.Equal(665m, item.TotalCost);
            });
    }

    [Fact]
    public void CostAndEtaEstimation_MatchesSampleInput2()
    {
        var estimator = CreateEstimator();

        var packages = new List<Package>
        {
            new Package { Id = "PKG1", WeightInKg = 50m, DistanceInKm = 30m, OfferCode = "OFR001" },
            new Package { Id = "PKG2", WeightInKg = 75m, DistanceInKm = 125m, OfferCode = "OFFR0008" },
            new Package { Id = "PKG3", WeightInKg = 175m, DistanceInKm = 100m, OfferCode = "OFFR003" },
            new Package { Id = "PKG4", WeightInKg = 110m, DistanceInKm = 60m, OfferCode = "OFR002" },
            new Package { Id = "PKG5", WeightInKg = 155m, DistanceInKm = 95m, OfferCode = "NA" }
        };

        var request = new DeliveryEstimationRequest
        {
            BaseDeliveryCost = 100m,
            Eta = new EtaEstimationRequest
            {
                Packages = packages,
                VehicleCount = 2,
                MaxSpeedKmPerHour = 70m,
                MaxCarriableWeight = 200m
            }
        };

        var result = estimator.EstimateCostsAndDeliveryTime(request);

        Assert.Collection(result,
            item =>
            {
                Assert.Equal("PKG1", item.PackageId);
                Assert.Equal(0m, item.Discount);
                Assert.Equal(750m, item.TotalCost);
                Assert.Equal(3.98m, item.EstimatedDeliveryTime);
            },
            item =>
            {
                Assert.Equal("PKG2", item.PackageId);
                Assert.Equal(0m, item.Discount);
                Assert.Equal(1475m, item.TotalCost);
                Assert.Equal(1.78m, item.EstimatedDeliveryTime);
            },
            item =>
            {
                Assert.Equal("PKG3", item.PackageId);
                Assert.Equal(0m, item.Discount);
                Assert.Equal(2350m, item.TotalCost);
                Assert.Equal(1.42m, item.EstimatedDeliveryTime);
            },
            item =>
            {
                Assert.Equal("PKG4", item.PackageId);
                Assert.Equal(105m, item.Discount);
                Assert.Equal(1395m, item.TotalCost);
                Assert.Equal(0.85m, item.EstimatedDeliveryTime);
            },
            item =>
            {
                Assert.Equal("PKG5", item.PackageId);
                Assert.Equal(0m, item.Discount);
                Assert.Equal(2125m, item.TotalCost);
                Assert.Equal(4.19m, item.EstimatedDeliveryTime);
            });
    }

    private static DeliveryEstimator CreateEstimator()
    {
        var offerDiscountService = new OfferDiscountService(DefaultOffers);
        var packageCostService = new PackageCostService(offerDiscountService);
        var shipmentSelectionService = new ShipmentSelectionService();
        var etaEstimationService = new EtaEstimationService(shipmentSelectionService);
        return new DeliveryEstimator(packageCostService, etaEstimationService);
    }
}
