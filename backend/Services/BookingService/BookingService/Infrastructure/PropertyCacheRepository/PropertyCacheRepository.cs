using BookingService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared;

namespace BookingService.Infrastructure.PropertyCacheRepository;

public class PropertyCacheRepository : IPropertyCacheRepository
{
    private readonly IMongoCollection<CachedProperty> _propertiesCollection;

    public PropertyCacheRepository(IOptions<MongoDbSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _propertiesCollection = database.GetCollection<CachedProperty>("PropertyCache");
    }
    public async Task<bool> AddPropertyToCache(CachedProperty property)
    {
        try
        {
            await _propertiesCollection.InsertOneAsync(property);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }

    public async Task<CachedProperty> GetPropertyById(Guid propertyId)
    {
        var filter = Builders<CachedProperty>.Filter.Eq(p => p.Id, propertyId);
        return await _propertiesCollection.Find(filter).FirstOrDefaultAsync();
    }
}