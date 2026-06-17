namespace ProjektABPD.DTOs;

public class CreateContractDto
{
    public int IdClient { get; set; }
    public int IdSoftware { get; set; }
    public int IdSoftwareVersion { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public decimal Price { get; set; }
    public int SupportYears { get; set; }
}