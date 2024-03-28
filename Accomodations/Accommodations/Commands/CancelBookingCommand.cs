using Accommodations;
using Accommodations.Commands;
using Accomodations;

public class CancelBookingCommand : ICommand
{
    private readonly BookingService bookingService;
    private readonly Guid bookingId;
    private Booking? canceledBooking;

    public CancelBookingCommand(BookingService bookingService, Guid bookingId)
    {
        this.bookingService = bookingService;
        this.bookingId = bookingId;
    }

    public void Execute()
    {
        canceledBooking = bookingService.FindBookingById(bookingId);
        if (canceledBooking != null)
        {
            bookingService.CancelBooking(bookingId);
            Console.WriteLine($"Booking {bookingId} canceled.");
        }
        else
        {
            Console.WriteLine($"Booking {bookingId} not found.");
        }
    }

    public void Undo()
    {
        if (canceledBooking != null)
        {
            Console.WriteLine("Undo for cancel is not supported");
        }
    }
}
