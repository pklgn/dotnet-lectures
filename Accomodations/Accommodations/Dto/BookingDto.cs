namespace Accommodations.Dto;

public class BookingDto
{
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Category { get; set; }
    public CurrencyDto Currency { get; set; }
}
