namespace ProjektABPD.DTOs;

public class CreateDiscountDto
{
    public string Name { get; set; } = null!;
    public int Percentage { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}