using PropertyService.DTOs;
using PropertyService.Models;

namespace PropertyService.Infrastructure;

public interface IPropertyRepository
{
    Task<Guid> AddProperty(Property property);

    Task<bool> AddBookingToProperty(Guid propertyId, Booking booking);

    Task<List<Booking>> GetBookingsForProperty(Guid propertyId);

    Task<Guid> GetOwnerForProperty(Guid propertyId);

    Task<List<GetSearchedPropertyDto>> SearchPropertiesByLocationAndDatesAsync(SearchPropertiesDto search);

    Task<GetPropertyByIdDto> GetPropertyByIdAsync(Guid propertyId);
    
    Task<List<Property>> GetPropertiesForOwnerAsync(Guid ownerId);
}