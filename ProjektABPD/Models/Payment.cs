namespace ProjektABPD.Models;
public class Payment
{
    public int IdPayment { get; set; }

    public int IdContract { get; set; }
    public Contract Contract { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }
}