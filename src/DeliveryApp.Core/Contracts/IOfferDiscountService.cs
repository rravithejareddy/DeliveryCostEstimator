using DeliveryApp.Core.Models;

namespace DeliveryApp.Core.Services;

public interface IOfferDiscountService
{
    decimal CalculateDiscount(Package package, decimal deliveryCost);
}
