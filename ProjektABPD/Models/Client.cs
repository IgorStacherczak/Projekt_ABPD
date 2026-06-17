namespace ProjektABPD.Models;
public class Client
{
    public int IdClient { get; set; }

    public string ClientType { get; set; } = null!;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? CompanyName { get; set; }

    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    public string? Pesel { get; set; }
    public string? Krs { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}