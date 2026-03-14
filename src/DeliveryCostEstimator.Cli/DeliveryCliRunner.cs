using DeliveryCostEstimator.Core.Models;
using DeliveryCostEstimator.Core.Models.RequestModels;
using DeliveryCostEstimator.Core.Services;
using System.Globalization;

namespace DeliveryCostEstimator.Cli;

public sealed class DeliveryCliRunner
{
    private readonly IDeliveryEstimator _estimator;

    public DeliveryCliRunner(IDeliveryEstimator estimator)
    {
        _estimator = estimator;
    }

    public void Run()
    {
        var isInteractive = !Console.IsInputRedirected;

        try
        {
            var inputLines = Console.IsInputRedirected
                ? ReadFromRedirectedInput()
                : ReadInteractively();

            if (inputLines.Count == 0)
            {
                throw new InvalidOperationException("No input received.");
            }

            var header = SplitParts(inputLines[0]);
            if (header.Length < 2)
            {
                throw new InvalidOperationException("First line must be: base_delivery_cost no_of_packages");
            }

            var baseDeliveryCost = decimal.Parse(header[0], CultureInfo.InvariantCulture);
            var packageCount = int.Parse(header[1]);

            if (inputLines.Count < packageCount + 1)
            {
                throw new InvalidOperationException("Missing package lines.");
            }

            var packages = new List<Package>(packageCount);
            for (var i = 1; i <= packageCount; i++)
            {
                var parts = SplitParts(inputLines[i]);
                if (parts.Length < 4)
                {
                    throw new InvalidOperationException($"Package line {i} must be: pkg_id weight distance offer_code");
                }

                packages.Add(new Package
                {
                    Id = parts[0],
                    WeightInKg = decimal.Parse(parts[1], CultureInfo.InvariantCulture),
                    DistanceInKm = decimal.Parse(parts[2], CultureInfo.InvariantCulture),
                    OfferCode = parts[3]
                });
            }

            if (inputLines.Count == packageCount + 2)
            {
                if (isInteractive)
                {
                    Console.WriteLine();
                    Console.WriteLine("Output Columns: package_id discount total_cost estimated_delivery_time_hours");
                }

                var vehicleInfo = SplitParts(inputLines[^1]);
                if (vehicleInfo.Length < 3)
                {
                    throw new InvalidOperationException("Vehicle line must be: no_of_vehicles max_speed max_carriable_weight");
                }

                var vehicleCount = int.Parse(vehicleInfo[0]);
                var speed = decimal.Parse(vehicleInfo[1], CultureInfo.InvariantCulture);
                var maxWeight = decimal.Parse(vehicleInfo[2], CultureInfo.InvariantCulture);

                var request = new DeliveryEstimationRequest
                {
                    BaseDeliveryCost = baseDeliveryCost,
                    Eta = new EtaEstimationRequest
                    {
                        Packages = packages,
                        VehicleCount = vehicleCount,
                        MaxSpeedKmPerHour = speed,
                        MaxCarriableWeight = maxWeight
                    }
                };

                var result = _estimator.EstimateCostsAndDeliveryTime(request);

                foreach (var item in result)
                {
                    Console.WriteLine($"{item.PackageId} {FormatNumber(item.Discount)} {FormatNumber(item.TotalCost)} {FormatNumber(item.EstimatedDeliveryTime)}");
                }
            }
            else
            {
                if (isInteractive)
                {
                    Console.WriteLine();
                    Console.WriteLine("Output Columns: package_id discount total_cost");
                }

                var result = _estimator.EstimateCosts(baseDeliveryCost, packages);

                foreach (var item in result)
                {
                    Console.WriteLine($"{item.PackageId} {FormatNumber(item.Discount)} {FormatNumber(item.TotalCost)}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Input error: {ex.Message}");
        }

        if (isInteractive)
        {
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }

    private static string[] SplitParts(string raw) => raw.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    private static List<string> ReadFromRedirectedInput()
    {
        var inputLines = new List<string>();
        string? line;
        while ((line = Console.ReadLine()) is not null)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                inputLines.Add(line.Trim());
            }
        }

        return inputLines;
    }

    private static List<string> ReadInteractively()
    {
        Console.WriteLine("Enter: base_delivery_cost no_of_packages");
        var header = ReadRequiredLine();
        var parts = SplitParts(header);
        if (parts.Length < 2)
        {
            throw new InvalidOperationException("First line must contain exactly base_delivery_cost and no_of_packages.");
        }

        var packageCount = int.Parse(parts[1]);

        var lines = new List<string> { header };

        Console.WriteLine("Enter package lines: pkg_id pkg_weight_in_kg distance_in_km offer_code");
        for (var i = 0; i < packageCount; i++)
        {
            Console.Write($"Package {i + 1}/{packageCount}: ");
            lines.Add(ReadRequiredLine());
        }

        Console.Write("Calculate ETA as well? (y/n): ");
        var includeEta = (Console.ReadLine() ?? string.Empty).Trim();
        if (includeEta.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Enter vehicle line: no_of_vehicles max_speed max_carriable_weight");
            lines.Add(ReadRequiredLine());
        }

        return lines;
    }

    private static string ReadRequiredLine()
    {
        while (true)
        {
            var line = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(line))
            {
                return line.Trim();
            }

            Console.WriteLine("Input cannot be empty. Please try again.");
        }
    }

    private static string FormatNumber(decimal value)
    {
        var rounded = Math.Round(value, 2, MidpointRounding.AwayFromZero);
        if (rounded == decimal.Truncate(rounded))
        {
            return decimal.Truncate(rounded).ToString("0");
        }

        return rounded.ToString("0.##");
    }
}
