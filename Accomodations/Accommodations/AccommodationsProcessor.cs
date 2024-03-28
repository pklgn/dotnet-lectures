using Accommodations.Commands;

namespace Accommodations;

public class AccommodationsProcessor
{
    private static BookingService _bookingService = new();
    private static Dictionary<int, ICommand> _executedCommands = new();
    private static int s_commandIndex = 0;

    public static void Run(string[] args)
    {
        Console.WriteLine("Booking Command Line Interface");
        Console.WriteLine("Commands:");
        Console.WriteLine("'book <UserId> <Category> <StartDate> <EndDate>' - to book a room");
        Console.WriteLine("'cancel <BookingId>' - to cancel a booking");
        Console.WriteLine("'undo' - to undo the last command");
        Console.WriteLine("'find <BookingId>' - to find a booking by ID");
        Console.WriteLine("'search <StartDate> <EndDate> <CategoryName>' - to search bookings");
        Console.WriteLine("'exit' - to exit the application");

        string input;
        while ((input = Console.ReadLine()) != "exit")
        {
            try
            {
                ProcessCommand(input);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private static void ProcessCommand(string input)
    {
        string[] parts = input.Split(' ');
        string commandName = parts[0];

        switch (commandName)
        {
            case "book":
                if (parts.Length != 5)
                {
                    Console.WriteLine("Invalid number of arguments for booking.");
                    return;
                }

                BookingDto bookingDto = new BookingDto
                {
                    UserId = int.Parse(parts[1]),
                    Category = parts[2],
                    StartDate = DateTime.Parse(parts[3]),
                    EndDate = DateTime.Parse(parts[4])
                };

                BookCommand bookCommand = new BookCommand(_bookingService, bookingDto);
                bookCommand.Execute();
                _executedCommands.Add(++s_commandIndex, bookCommand);
                Console.WriteLine("Booking successful.");
                break;

            case "cancel":
                if (parts.Length != 2)
                {
                    Console.WriteLine("Invalid number of arguments for canceling.");
                    return;
                }

                Guid bookingId = Guid.Parse(parts[1]);
                CancelBookingCommand cancelCommand = new CancelBookingCommand(_bookingService, bookingId);
                cancelCommand.Execute();
                _executedCommands.Add(++s_commandIndex, cancelCommand);
                Console.WriteLine("Cancellation successful.");
                break;

            case "undo":
                _executedCommands[s_commandIndex].Undo();
                _executedCommands.Remove(s_commandIndex);
                s_commandIndex--;
                Console.WriteLine("Last command undone.");

                break;
            case "find":
                if (parts.Length != 2)
                {
                    Console.WriteLine("Invalid arguments for 'find'. Expected format: 'find <BookingId>'");
                    return;
                }
                Guid id = Guid.Parse(parts[1]);
                var findCommand = new FindBookingByIdCommand(_bookingService, id);
                findCommand.Execute();
                break;

            case "search":
                if (parts.Length != 4)
                {
                    Console.WriteLine("Invalid arguments for 'search'. Expected format: 'search <StartDate> <EndDate> <CategoryName>'");
                    return;
                }
                DateTime startDate = DateTime.Parse(parts[1]);
                DateTime endDate = DateTime.Parse(parts[2]);
                string categoryName = parts[3];
                var searchCommand = new SearchBookingsCommand(_bookingService, startDate, endDate, categoryName);
                searchCommand.Execute();
                break;

            default:
                Console.WriteLine("Unknown command.");
                break;
        }
    }
}
