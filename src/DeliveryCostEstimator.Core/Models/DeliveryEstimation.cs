namespace DeliveryCostEstimator.Core.Models;

public class DeliveryEstimation
{
    public required string PackageId { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalCost { get; set; }
    public decimal EstimatedDeliveryTime { get; set; }
}
