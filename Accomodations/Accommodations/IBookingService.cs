using Accomodations.Models;

namespace Accomodations;

public interface IBookingService
{
    Booking Book(int userId, string category, DateTime startDate, DateTime endDate, bool applyDiscount,
        Currency currency);

    void CancelBooking(Guid bookingId);
    Booking FindBookingById(Guid bookingId);
    IEnumerable<Booking> SearchBookings(DateTime startDate, DateTime endDate, string categoryName);
    decimal CalculateCancellationPenaltyAmount(Booking booking);
}
