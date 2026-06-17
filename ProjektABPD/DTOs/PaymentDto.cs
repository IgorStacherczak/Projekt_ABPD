namespace ProjektABPD.DTOs;

public class PaymentDto
{
    public int IdPayment { get; set; }
    public int IdContract { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}