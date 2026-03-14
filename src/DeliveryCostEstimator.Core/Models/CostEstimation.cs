namespace DeliveryCostEstimator.Core.Models;

public class CostEstimation
{
	public string? PackageId { get; set; }
	public decimal Discount { get; set; }
	public decimal TotalCost { get; set; }
}
