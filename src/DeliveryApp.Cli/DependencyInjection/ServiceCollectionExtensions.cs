using DeliveryApp.Cli;
using DeliveryApp.Core.Config;
using DeliveryApp.Core.Models;
using DeliveryApp.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryApp.Cli.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDeliveryAppServices(this IServiceCollection services)
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
