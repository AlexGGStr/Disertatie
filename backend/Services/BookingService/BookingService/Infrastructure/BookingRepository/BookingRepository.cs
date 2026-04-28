using BookingService.DTOs;
using BookingService.Infrastructure.Producers;
using BookingService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared;

namespace BookingService.Infrastructure.BookingRepository;

public class BookingRepository : IBoookingRepository
{
    private readonly IMongoCollection<Booking> _bookingsCollection;
    private readonly IBookingCreatedProducer _producer;


    public BookingRepository(IOptions<MongoDbSettings> options, IBookingCreatedProducer producer)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _bookingsCollection = database.GetCollection<Booking>("Bookings");

        _producer = producer;
    }
    
    public async Task<bool> AreDatesAvailable(Guid propertyId, DateOnly from, DateOnly to)
    {
        if (from > to || from < DateOnly.FromDateTime(DateTime.Now)) return false;
        
            
        var filter = Builders<Booking>.Filter.And(
            Builders<Booking>.Filter.Eq(b => b.PropertyId, propertyId),
            Builders<Booking>.Filter.Lt(b => b.From, to),
            Builders<Booking>.Filter.Gt(b => b.To, from)
        );

        return !await _bookingsCollection.Find(filter).AnyAsync();
    }

    public async Task<Guid> InsertBooking(Booking booking)
    {
        await _bookingsCollection.InsertOneAsync(booking);

        _producer.SendBooking(booking.PropertyId.ToString(), new BookingCreatedEvent
        {
            BookingId = booking.Id,
            From = booking.From,
            To = booking.To,
            User = booking.User,
            OwnerEmail = booking.PropertySnapshot.OwnerEmail,
            NoOfPeople = booking.NoOfPeople,
            PropertyName = booking.PropertySnapshot.Name,
            PricePerNight = booking.PropertySnapshot.PricePerNight,
            TotalPrice = booking.TotalPrice,
            CreatedAt = booking.CreatedAt
        });
        
        return booking.Id;
    }

    public async Task<bool> ConfirmBooking(Guid bookingId)
    {
        var filter = Builders<Booking>.Filter.Eq(b => b.Id, bookingId);
        var update = Builders<Booking>.Update
            .Set(b => b.Status, BookingStatus.Confirmed)
            .Set(b => b.UpdatedAt, DateTime.UtcNow);

        var result = await _bookingsCollection.UpdateOneAsync(filter, update);

        return result.ModifiedCount > 0;
    }

    public async Task<List<GetBookingsDTO>> GetBookingsForUser(Guid userId)
    {
        var data  = await _bookingsCollection.Find(b => b.User.Id == userId).ToListAsync();

        return data.Select(b => new GetBookingsDTO
        {
            Id = b.Id,
            From = b.From,
            NoOfPeople = b.NoOfPeople,
            PropertyId = b.PropertyId,
            Status = b.Status.ToString(),
            To = b.To,
            TotalPrice = b.TotalPrice,
            Location = b.PropertySnapshot.Location,
            PropertyName = b.PropertySnapshot.Name
        }).ToList();
    }
}