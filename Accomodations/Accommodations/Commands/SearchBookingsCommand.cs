using Accommodations;
using Accommodations.Commands;

public class SearchBookingsCommand : ICommand
{
    private readonly BookingService _bookingService;
    private readonly DateTime _startDate;
    private readonly DateTime _endDate;
    private readonly string _categoryName;

    public SearchBookingsCommand(BookingService bookingService, DateTime startDate, DateTime endDate, string categoryName)
    {
        _bookingService = bookingService;
        _startDate = startDate;
        _endDate = endDate;
        _categoryName = categoryName;
    }

    public void Execute()
    {
        var bookings = _bookingService.SearchBookings(_startDate, _endDate, _categoryName);
        if (bookings.Any())
        {
            Console.WriteLine($"Found {bookings.Count()} bookings for category '{_categoryName}' between {_startDate} and {_endDate}:");
            foreach (var booking in bookings)
            {
                Console.WriteLine($"- Booking ID: {booking.Id}, User ID: {booking.UserId}");
            }
        }
        else
        {
            Console.WriteLine("No bookings found.");
        }
    }

    public void Undo()
    {
        Console.WriteLine($"Undo operation is not supported for {nameof(GetType)}.");
    }
}
