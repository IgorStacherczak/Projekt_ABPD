namespace ProjektABPD.DTOs;

public class CreateSoftwareVersionDto
{
    public string VersionNumber { get; set; } = null!;
    public int IdSoftware { get; set; }
}