namespace ProjektABPD.Models;
public class Software
{
    public int IdSoftware { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!;

    public ICollection<SoftwareVersion> Versions { get; set; } = new List<SoftwareVersion>();
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}