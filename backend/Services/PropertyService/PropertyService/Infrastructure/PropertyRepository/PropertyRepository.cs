using System.Collections;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PropertyService.DTOs;
using PropertyService.Infrastructure.PropertyKafkaProducer;
using PropertyService.Models;
using Shared;

namespace PropertyService.Infrastructure;

public class PropertyRepository : IPropertyRepository
{
    private readonly IMongoCollection<Property> _propertiesCollection;
    private readonly IPropertyProducer _producer;

    public PropertyRepository(IOptions<MongoDbSettings> options, IPropertyProducer producer)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _propertiesCollection = database.GetCollection<Property>("Properties");

        _producer = producer;
    }
    public async Task<Guid> AddProperty(Property property)
    {
        await _propertiesCollection.InsertOneAsync(property);


        _producer.SendPropertyCreated(property.Owner.Id.ToString(),new PropertyEvents.PropertyCreatedEvent
        {
            PropertyName = property.Name,
            PropertyId = property.PropertyId,
            UserName = property.Owner.Name,
            UserEmail = property.Owner.Email,
            Capacity = property.NoOfPeople,
            PricePerNight = property.PricePerNight,
            Location = $"{property.Location.Address}"
        });
        
        return property.PropertyId;
    }

    public async Task<bool> AddBookingToProperty(Guid propertyId, Booking booking)
    {
        try
        {
            var propertyToUpdate = Builders<Property>.Filter.Eq(p => p.PropertyId, propertyId);
            var update = Builders<Property>.Update.Push(p => p.Bookings, booking).Set(p => p.UpdatedAt, DateTime.Now);

            await _propertiesCollection.UpdateOneAsync(propertyToUpdate, update);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<List<Booking>> GetBookingsForProperty(Guid propertyId)
    {
        var filter = Builders<Property>.Filter.Eq(p => p.PropertyId, propertyId);
        var projection = Builders<Property>.Projection.Expression(p => p.Bookings);

        var bookings = await _propertiesCollection.Find(filter).Project(projection).FirstOrDefaultAsync();

        return bookings;
    }

    public async Task<Guid> GetOwnerForProperty(Guid propertyId)
    {
        return await _propertiesCollection
            .Find(p => p.PropertyId == propertyId)
            .Project(p => p.Owner.Id).FirstOrDefaultAsync();

    }

    public async Task<List<GetSearchedPropertyDto>> SearchPropertiesByLocationAndDatesAsync(SearchPropertiesDto search)
    {
        var filterBuilder = Builders<Property>.Filter;
        var filterList = new List<FilterDefinition<Property>>();

        if (!string.IsNullOrEmpty(search.PlaceId))
        {
            filterList.Add(filterBuilder.Where(p => p.Location.LocationId.Equals(search.PlaceId, StringComparison.OrdinalIgnoreCase)));
        }

        if (search.NoOfPeople != 0)
        {
            filterList.Add(filterBuilder.Gte(p => p.NoOfPeople, search.NoOfPeople));
        }

        // Exclude properties with overlapping bookings
        filterList.Add(filterBuilder.Not(
            filterBuilder.ElemMatch(p => p.Bookings,
                bd => search.From <= bd.EndDate && search.To >= bd.StartDate
            )
        ));

        var finalFilter = filterBuilder.And(filterList);

        var properties = await _propertiesCollection.Find(finalFilter).ToListAsync();
        
        return properties.Select(p => new GetSearchedPropertyDto
        {
            PropertyId = p.PropertyId,
            PropertyName = p.Name,
            Location = p.Location,
            Capacity = p.NoOfPeople,
            PricePerNight = p.PricePerNight,
            NoOfRooms = p.Rooms,
            Images = p.Images
        }).ToList();
    }

    public async Task<GetPropertyByIdDto> GetPropertyByIdAsync(Guid propertyId)
    {
        var property =  await _propertiesCollection.Find(p => p.PropertyId == propertyId).FirstOrDefaultAsync();

        return new GetPropertyByIdDto
        {
            PropertyId = property.PropertyId,
            PropertyName = property.Name,
            Capacity = property.NoOfPeople,
            Location = property.Location,
            NoOfRooms = property.Rooms,
            PricePerNight = property.PricePerNight,
            Description = property.Description ?? "",
            Images = property.Images
        };
    }

    public async Task<List<Property>> GetPropertiesForOwnerAsync(Guid ownerId)
    {
        return await _propertiesCollection.Find(p => p.Owner.Id == ownerId).ToListAsync();
    }

}