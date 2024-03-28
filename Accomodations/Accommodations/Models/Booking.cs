namespace Accomodations.Models;

public class Booking
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Category Category { get; set; }
    public Currency Currency { get; set; }
    public decimal Cost { get; set; }
}
