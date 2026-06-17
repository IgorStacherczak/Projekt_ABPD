namespace ProjektABPD.Models;
public class Discount
{
    public int IdDiscount { get; set; }

    public string Name { get; set; } = null!;
    public int Percentage { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}