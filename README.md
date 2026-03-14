# DeliveryCostEstimator

Simple .NET console app to calculate:
- package discount
- total delivery cost
- estimated delivery time (ETA)

## What This App Does

1. Reads package input and base delivery cost.
2. Applies offer rules to calculate discount.
3. Calculates total cost per package.
4. Optionally calculates ETA based on:
- number of vehicles
- vehicle speed
- max carriable weight

## What You Can Modify

1. Offer rules:
- File: `src/DeliveryCostEstimator.Core/Config/OfferConfiguration.cs`
- You can add/change offer code, discount ratio, weight range, and distance range.

2. Cost formula:
- File: `src/DeliveryCostEstimator.Core/ServiceImplementations/PackageCostService.cs`
- Current formula: `base + (weight * 10) + (distance * 5)`

3. Shipment selection logic:
- File: `src/DeliveryCostEstimator.Core/ServiceImplementations/ShipmentSelectionService.cs`

4. ETA calculation logic:
- File: `src/DeliveryCostEstimator.Core/ServiceImplementations/EtaEstimationService.cs`

5. Add separate services as needed (recommended):
- This project uses dependency injection, so each responsibility is kept in a separate service.
- Example: discount service, package cost service, shipment selection service, and ETA service are all separate.
- If you add a new business rule, create a new service for it instead of putting everything in one class.
- Then register that service in DI and inject it where required.

## Why Services Are Registered as Transient

All core services (discount, cost, shipment, ETA) are registered as **transient** in DI:
- A fresh instance is created for every request/operation.
- This avoids any shared state between calls.
- Safe for parallel or sequential use without side effects.

## Facade Pattern — DeliveryEstimator

`DeliveryEstimator` (`src/DeliveryCostEstimator.Core/Services/DeliveryEstimator.cs`) acts as the **facade** of the application:
- It is the single entry point for all delivery logic.
- It hides the complexity of the underlying services (cost, discount, shipment selection, ETA).
- Callers (e.g. the CLI) only interact with `IDeliveryEstimator` and do not need to know about the individual services behind it.

## Why Offer Rules Are Registered as Singleton

In DI setup, offers are registered once:
- File: `src/DeliveryCostEstimator.Cli/DependencyInjection/ServiceCollectionExtensions.cs`

Reason:
- Offers are configuration data.
- They do not change per request.
- Reusing one instance is simple and efficient.

## How To Run

1. Restore and build:

```bash
dotnet build DeliveryCostEstimator.slnx -c Release
```

2. Run CLI app:

```bash
dotnet run --project src/DeliveryCostEstimator.Cli/DeliveryCostEstimator.Cli.csproj
```

3. Run tests:

```bash
dotnet test DeliveryCostEstimator.slnx -c Release
```

## Example Input (with ETA)

```text
100 5
PKG1 50 30 OFR001
PKG2 75 125 OFFR0008
PKG3 175 100 OFFR003
PKG4 110 60 OFR002
PKG5 155 95 NA
2 70 200
```
