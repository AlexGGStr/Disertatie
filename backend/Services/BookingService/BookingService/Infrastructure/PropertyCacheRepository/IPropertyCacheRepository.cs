using BookingService.Models;

namespace BookingService.Infrastructure.PropertyCacheRepository;

public interface IPropertyCacheRepository
{
    Task<bool> AddPropertyToCache(CachedProperty property);

    Task<CachedProperty> GetPropertyById(Guid propertyId);
}