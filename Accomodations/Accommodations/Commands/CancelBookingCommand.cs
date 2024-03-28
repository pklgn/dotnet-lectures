using Accommodations;
using Accommodations.Commands;
using Accomodations;

public class CancelBookingCommand : ICommand
{
    private readonly BookingService _bookingService;
    private readonly Guid _bookingId;
    private Booking? _canceledBooking;

    public CancelBookingCommand(BookingService bookingService, Guid bookingId)
    {
        _bookingService = bookingService;
        _bookingId = bookingId;
    }

    public void Execute()
    {
        _canceledBooking = _bookingService.FindBookingById(_bookingId);
        if (_canceledBooking != null)
        {
            _bookingService.CancelBooking(_bookingId);
            decimal cancellationPenalty = _bookingService.CalculateCancellationPenaltyAmount(_canceledBooking);
            Console.WriteLine($"Booking {_canceledBooking.Id} was canceled. Cancellation penalty: {cancellationPenalty}");
        }
        else
        {
            Console.WriteLine($"Booking {_bookingId} not found.");
        }
    }

    public void Undo()
    {
        if (_canceledBooking != null)
        {
            Console.WriteLine("Undo for cancel is not supported");
        }
    }
}
