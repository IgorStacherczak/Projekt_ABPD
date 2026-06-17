namespace ProjektABPD.DTOs;

public class DiscountDto
{
    public int IdDiscount { get; set; }
    public string Name { get; set; } = null!;
    public decimal Value { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}