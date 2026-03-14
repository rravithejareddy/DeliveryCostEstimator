namespace DeliveryApp.Core.Domain;

public sealed class Package
{
	public string Id { get; set; }
	public decimal WeightInKg { get; set; }
	public decimal DistanceInKm { get; set; }
	public string? OfferCode { get; set; }

	public Package(string id, decimal weightInKg, decimal distanceInKm, string? offerCode)
	{
		Id = id;
		WeightInKg = weightInKg;
		DistanceInKm = distanceInKm;
		OfferCode = offerCode;
	}
}
