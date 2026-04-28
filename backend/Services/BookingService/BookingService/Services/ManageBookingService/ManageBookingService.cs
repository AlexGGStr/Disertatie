using BookingService.DTOs;
using BookingService.Infrastructure.BookingRepository;
using BookingService.Infrastructure.PropertyCacheRepository;
using BookingService.Models;
using Shared;

namespace BookingService.Services.ManageBookingService;

public class ManageBookingService(IBoookingRepository bookingRepository, IPropertyCacheRepository propertyRepository) : IManageBookingService
{
    public async Task<ServiceResponse<Guid>> CreateBooking(User user, CreateBookingDTO booking)
    {
        ServiceResponse<Guid> result = new ServiceResponse<Guid>();
        var propertySnapshot = await propertyRepository.GetPropertyById(booking.PropertyId);
        
        //Pre Checks
        if (booking.NoOfPeople > propertySnapshot.Capacity)
        {
            result.Message = "Over Capacity!";
            result.Success = false;

            return result;
        }

        if (!await bookingRepository.AreDatesAvailable(booking.PropertyId, from: booking.From, to: booking.To) 
            || booking.From.ToDateTime(TimeOnly.MinValue) < DateTime.UtcNow || booking.To < booking.From)
        {
            result.Message = "Dates Unavailable";
            result.Success = false;

            return result;
        }

        Booking bookingToAdd = new Booking
        {
            PropertyId = booking.PropertyId,
            PropertySnapshot = new PropertySnapshot
            {
                Capacity = propertySnapshot.Capacity,
                Name = propertySnapshot.PropertyName,
                PricePerNight = propertySnapshot.PricePerNight,
                OwnerEmail = propertySnapshot.OwnerEmail,
                Location = propertySnapshot.Location
            },
            User = user,
            From = booking.From,
            To = booking.To,
            NoOfPeople = booking.NoOfPeople,
            TotalPrice = (booking.To.DayNumber - booking.From.DayNumber) * propertySnapshot.PricePerNight
        };

        result.Data = await bookingRepository.InsertBooking(bookingToAdd);
        return result;
    }

    public async Task<ServiceResponse<bool>> AreDatesAvailable(Guid propertyId, DateOnly start, DateOnly end)
    {
        var result = new ServiceResponse<bool>();
        try
        {
            result.Data = await bookingRepository.AreDatesAvailable(propertyId, start, end);
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.ToString();
        }

        return result;
    }

    public async Task<ServiceResponse<List<GetBookingsDTO>>> GetBookings(Guid userId)
    {
        var result = new ServiceResponse<List<GetBookingsDTO>>();

        try
        {
            result.Data = await bookingRepository.GetBookingsForUser(userId);
        }
        catch (Exception e)
        {
            result.Message = e.Message;
            result.Success = false;
        }

        return result;
    }
}