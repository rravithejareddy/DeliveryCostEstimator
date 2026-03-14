using DeliveryCostEstimator.Cli;
using DeliveryCostEstimator.Cli.DependencyInjection;
using DeliveryCostEstimator.Core.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddDeliveryCostEstimatorServices();

using var serviceProvider = services.BuildServiceProvider();
serviceProvider.GetRequiredService<DeliveryCliRunner>().Run();
