using DeliveryApp.Cli;
using DeliveryApp.Cli.DependencyInjection;
using DeliveryApp.Core.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddDeliveryAppServices();

using var serviceProvider = services.BuildServiceProvider();
serviceProvider.GetRequiredService<DeliveryCliRunner>().Run();
