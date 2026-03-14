using DeliveryCostEstimator.Cli;
using DeliveryCostEstimator.Core.Config;
using DeliveryCostEstimator.Core.Models;
using DeliveryCostEstimator.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryCostEstimator.Cli.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDeliveryCostEstimatorServices(this IServiceCollection services)
    {
        services.AddSingleton<IEnumerable<OfferRule>>(_ => OfferConfiguration.DefaultOffers);
        services.AddTransient<IOfferDiscountService, OfferDiscountService>();
        services.AddTransient<IPackageCostService, PackageCostService>();
        services.AddTransient<IShipmentSelectionService, ShipmentSelectionService>();
        services.AddTransient<IEtaEstimationService, EtaEstimationService>();
        services.AddTransient<IDeliveryEstimator, DeliveryEstimator>();
        services.AddTransient<DeliveryCliRunner>();

        return services;
    }
}
