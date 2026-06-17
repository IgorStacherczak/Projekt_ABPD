namespace ProjektABPD.DTOs;

public class ContractDto
{
    public int IdContract { get; set; }

    public int IdClient { get; set; }
    public int IdSoftware { get; set; }
    public int IdSoftwareVersion { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public decimal Price { get; set; }
    public int SupportYears { get; set; }

    public bool IsSigned { get; set; }
    public bool IsCancelled { get; set; }
}