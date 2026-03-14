namespace DeliveryCostEstimator.Core.Models;

public class Package
{
	public required string Id { get; set; }
	public decimal WeightInKg { get; set; }
	public decimal DistanceInKm { get; set; }
	public string? OfferCode { get; set; }
}
