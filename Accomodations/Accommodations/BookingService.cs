using Accomodations.Models;

namespace Accomodations;

public class BookingService : IBookingService
{
    private List<Booking> _bookings = [];

    private List<Category> _categories =
    [
        new Category { Name = "Standard", BaseRate = 100, AvailableRooms = 10 },
        new Category { Name = "Deluxe", BaseRate = 200, AvailableRooms = 5 }
    ];

    private List<User> _users =
    [
        new User { Id = 1, Name = "Alice Johnson", IsEligibleForDiscount = true },
        new User { Id = 2, Name = "Bob Smith", IsEligibleForDiscount = false },
        new User { Id = 3, Name = "Charlie Brown", IsEligibleForDiscount = true },
        new User { Id = 4, Name = "Diana Prince", IsEligibleForDiscount = false },
        new User { Id = 5, Name = "Evan Wright", IsEligibleForDiscount = true }
    ];

    public Booking Book(int userId, string category, DateTime startDate, DateTime endDate, bool applyDiscount,
        Currency currency)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException("End date cannot be earlier than start date");
        }

        Category? selectedCategory = _categories.FirstOrDefault(c => c.Name == category);
        if (selectedCategory == null)
        {
            throw new ArgumentException("Category not found");
        }

        if (selectedCategory.AvailableRooms <= 0)
        {
            throw new ArgumentException("No available rooms");
        }

        User? user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        int days = (endDate - startDate).Days;
        decimal currencyRate = GetCurrencyRate(currency);
        decimal totalCost = CalculateBookingCost(selectedCategory.BaseRate, days, userId, currencyRate);

        Booking booking = new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate,
            Category = selectedCategory,
            Cost = totalCost,
            Currency = currency
        };

        _bookings.Add(booking);
        selectedCategory.AvailableRooms--;

        return booking;
    }

    public void CancelBooking(Guid bookingId)
    {
        Booking? booking = _bookings.FirstOrDefault(b => b.Id == bookingId);
        if (booking == null)
        {
            throw new ArgumentException($"Booking with id: '{bookingId}' does not exist");
        }

        if (booking.StartDate <= DateTime.Now)
        {
            throw new ArgumentException("Start date cannot be earlier than now date");
        }

        Console.WriteLine($"Возврат средств на сумму {CalculateBookingCost(
            booking.Category.BaseRate,
            (booking.EndDate - booking.StartDate).Days,
            booking.UserId,
            GetCurrencyRate(booking.Currency))
        }");
        _bookings.Remove(booking);
        Category? category = _categories.FirstOrDefault(c => c.Name == booking.Category.Name);
        category.AvailableRooms++;
    }

    private decimal CalculateDiscount(int userId)
    {
        return 0.1m;
    }

    public Booking FindBookingById(Guid bookingId)
    {
        return _bookings.FirstOrDefault(b => b.Id == bookingId);
    }

    public IEnumerable<Booking> SearchBookings(DateTime startDate, DateTime endDate, string categoryName)
    {
        IQueryable<Booking> query = _bookings.AsQueryable();

        query = query.Where(b => b.StartDate >= startDate);

        query = query.Where(b => b.EndDate <= endDate);

        if (!string.IsNullOrEmpty(categoryName))
        {
            query = query.Where(b => b.Category.Name == categoryName);
        }

        return query.ToList();
    }

    public decimal CalculateCancellationPenaltyAmount(Booking booking)
    {
        if (booking.StartDate <= DateTime.Now)
        {
            throw new ArgumentException("Start date cannot be earlier than now date");
        }

        int daysBeforeArrival = (DateTime.Now - booking.StartDate).Days;

        return 5000.0m / daysBeforeArrival;
    }

    private decimal GetCurrencyRate(Currency currency)
    {
        decimal currencyRate = 1m;
        currencyRate *= currency switch
        {
            Currency.Usd => (decimal)(new Random().NextDouble() * 100) + 1,
            Currency.Cny => (decimal)(new Random().NextDouble() * 12) + 1,
            Currency.Rub => 1m,
            _ => throw new ArgumentOutOfRangeException(nameof(currency), currency, null)
        };

        return currencyRate;
    }

    private decimal CalculateBookingCost(decimal baseRate, int days, int userId, decimal currencyRate)
    {
        decimal cost = baseRate * days;
        decimal totalCost = cost - cost * CalculateDiscount(userId) * currencyRate;
        return totalCost;
    }
}
