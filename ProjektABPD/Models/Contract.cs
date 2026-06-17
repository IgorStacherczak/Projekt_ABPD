namespace ProjektABPD.Models;
public class Contract
{
    public int IdContract { get; set; }

    public int IdClient { get; set; }
    public Client Client { get; set; } = null!;

    public int IdSoftware { get; set; }
    public Software Software { get; set; } = null!;

    public int IdSoftwareVersion { get; set; }
    public SoftwareVersion SoftwareVersion { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public decimal Price { get; set; }

    public int SupportYears { get; set; }

    public bool IsSigned { get; set; }
    public bool IsCancelled { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}