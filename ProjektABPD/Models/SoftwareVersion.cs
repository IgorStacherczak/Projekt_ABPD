namespace ProjektABPD.Models;
public class SoftwareVersion
{
    public int IdSoftwareVersion { get; set; }

    public string VersionNumber { get; set; } = null!;

    public int IdSoftware { get; set; }
    public Software Software { get; set; } = null!;

    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}