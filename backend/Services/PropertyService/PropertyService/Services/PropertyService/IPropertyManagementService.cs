using PropertyService.DTOs;
using PropertyService.Models;
using Shared;

namespace PropertyService.Services.PropertyService;

public interface IPropertyManagementService
{
    Task<ServiceResponse<Guid>> AddProperty(PropertyOwner owner, AddPropertyDTO property);

    Task<ServiceResponse<List<Booking>>> GetBookingsForProperty(Guid ownerId, Guid propertyId);
    
    Task<ServiceResponse<List<GetSearchedPropertyDto>>> SearchPropertiesByLocationAndDatesAsync(SearchPropertiesDto search);

    Task<ServiceResponse<GetPropertyByIdDto>> GetPropertyById(Guid propertyId);

    Task<ServiceResponse<List<GooglePlaceDto>>> SearchCity(string city, bool onlyCity);

    Task<ServiceResponse<List<GetSearchedPropertyDto>>> GetPropertiesForOwnerAsync(Guid ownerId);
}