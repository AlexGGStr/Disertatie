using System.Text.Json;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json.Linq;
using PropertyService.DTOs;
using PropertyService.Infrastructure;
using PropertyService.Models;
using PropertyService.Services.ImageService;
using Shared;

namespace PropertyService.Services.PropertyService;

public class PropertyManagementService(IPropertyRepository repository, 
    GoogleApi googleApi, 
    IHttpClientFactory httpClientFactory,
    IImageService imageService,
    IHttpContextAccessor httpContextAccessor) : IPropertyManagementService
{
    public async Task<ServiceResponse<Guid>> AddProperty(PropertyOwner owner ,AddPropertyDTO property)
    {
        var result = new ServiceResponse<Guid>();
        try
        {
            var infoAboutLocation = await this.getInfoAboutLocation(property);

            var cityIdResponse = await this.SearchCity(infoAboutLocation.city + " " + infoAboutLocation.country, true);

            var cityId = cityIdResponse.Data.First().PlaceId;

            if (string.IsNullOrEmpty(infoAboutLocation.city) || string.IsNullOrEmpty(infoAboutLocation.country))
                throw new Exception("Could not determine city or country");

            var images = await imageService.AddImages(property.Images);
            
            var data = await repository.AddProperty(new Property
            {
                Name = property.Name,
                Location = new Location
                {
                    Address = property.LocationAdress,
                    LocationId = cityId,
                    City = infoAboutLocation.city,
                    Country = infoAboutLocation.country,
                    Latitude = infoAboutLocation.latitude,
                    Longitude = infoAboutLocation.longitude,
                },
                Images = images.Data,
                Description = property.Description,
                PricePerNight = property.PricePerNight,
                Rooms = property.Rooms,
                NoOfPeople = property.NoOfPeople,
                Owner = owner,
            });
            
            result.Data = data;

        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = ex.Message;
        }
        
        return result;
    }

    private List<string> GetImagesUrls(List<string> images)
    {
        if (images == null)
        {
            return new List<string>();
        }
        
        var request = httpContextAccessor.HttpContext.Request;
        return images.Select(image
                => String.Format("{0}://{1}{2}/Images/{3}", request.Scheme, request.Host, request.PathBase, image))
            .ToList();
    }

    public async Task<ServiceResponse<List<Booking>>> GetBookingsForProperty(Guid ownerId, Guid propertyId)
    {
        var result = new ServiceResponse<List<Booking>>();
        try
        {
            if (ownerId != await repository.GetOwnerForProperty(propertyId))
            {
                throw new Exception("You are not the owner of this property");
            }
            
            result.Data = await repository.GetBookingsForProperty(propertyId);
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
            result.Success = false;
        }

        return result;
    }

    public async Task<ServiceResponse<List<GetSearchedPropertyDto>>> SearchPropertiesByLocationAndDatesAsync(SearchPropertiesDto search)
    {
        var result = new ServiceResponse<List<GetSearchedPropertyDto>>();

        try
        {
            result.Data = await repository.SearchPropertiesByLocationAndDatesAsync(search);
            result.Data.ForEach(p => p.Images = this.GetImagesUrls(p.Images));

            if (result.Data.Count == 0) throw new Exception("No data Found");
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = ex.Message;
        }

        return result;
    }

    public async Task<ServiceResponse<GetPropertyByIdDto>> GetPropertyById(Guid propertyId)
    {
        var result = new ServiceResponse<GetPropertyByIdDto>();

        try
        {
            var data = await repository.GetPropertyByIdAsync(propertyId);
            data.Images = this.GetImagesUrls(data.Images);
            result.Data = data;

            if (result.Data == null) throw new Exception("No data Found");
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = ex.Message;
        }

        return result;
    }

    public async Task<ServiceResponse<List<GooglePlaceDto>>> SearchCity(string input, bool onlyCity)
    {
        var result = new ServiceResponse<List<GooglePlaceDto>>();

        try
        {
            var client = httpClientFactory.CreateClient();

            var requestUri = $"{googleApi.PlacesAPI}/autocomplete/json?" +
                             $"input={Uri.EscapeDataString(input)}{(onlyCity ? "&types=(cities)" : "")}&key={googleApi.Key}";

            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var predictions = root.GetProperty("predictions");

            result.Data = predictions.EnumerateArray()
                .Select(p => new GooglePlaceDto
                {
                    Description = p.GetProperty("description").GetString() ?? "",
                    PlaceId = p.GetProperty("place_id").GetString() ?? ""
                }).ToList();
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = ex.Message;
        }

        return result;
    }

    public async Task<ServiceResponse<List<GetSearchedPropertyDto>>> GetPropertiesForOwnerAsync(Guid ownerId)
    {
        var result = new ServiceResponse<List<GetSearchedPropertyDto>>();

        try
        {
            var properties = await repository.GetPropertiesForOwnerAsync(ownerId);
            properties.ForEach(p => p.Images = this.GetImagesUrls(p.Images));

            if (properties.Count == 0)
            {
                result.Success = false;
                result.Message = "No found properties";
            }

            result.Data = properties.Select(p => new GetSearchedPropertyDto
            {
                Capacity = p.NoOfPeople,
                Location = p.Location,
                NoOfRooms = p.Rooms,
                PricePerNight = p.PricePerNight,
                PropertyId = p.PropertyId,
                PropertyName = p.Name,
                Images = p.Images
            }).ToList();
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = ex.Message;
        }
        
        return result;
    }
    
    private async Task<(string city, string country, double latitude, double longitude)> getInfoAboutLocation(AddPropertyDTO property)
    {
        var client = httpClientFactory.CreateClient();

        var requestUri = $"{googleApi.PlacesAPI}/details/json?" +
                         $"place_id={property.LocationId}&key={googleApi.Key}";
            
            

        var response = await client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse(content);
        var jsonInfo = json["result"];

        // Get coordinates
        var geometry = jsonInfo["geometry"]?["location"];
        double lat = geometry?["lat"]?.Value<double>() ?? 0;
        double lng = geometry?["lng"]?.Value<double>() ?? 0;
            
        // Get address components
        string city = null;
        string countryCode = null;

        var components = jsonInfo["address_components"];
        foreach (var component in components)
        {
            var types = component["types"]?.Select(t => t.ToString()).ToArray();
            if (types == null) continue;

            if (types.Contains("locality"))
                city = component["long_name"]?.ToString();
            if (types.Contains("country"))
                countryCode = component["long_name"]?.ToString();
        }

        return (city, countryCode, lat, lng);
    }
}