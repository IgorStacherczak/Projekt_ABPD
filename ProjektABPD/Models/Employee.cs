namespace ProjektABPD.Models;
public class Employee
{
    public int IdEmployee { get; set; }

    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
}