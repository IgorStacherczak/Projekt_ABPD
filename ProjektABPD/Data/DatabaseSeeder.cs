using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjektABPD.Models;

namespace ProjektABPD.Data;

public static class DatabaseSeeder
{
    public static async Task SeedEmployeesAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var passwordHasher = new PasswordHasher<Employee>();

        var admin = await context.Employees.FirstOrDefaultAsync(e => e.Login == "admin");

        if (admin == null)
        {
            admin = new Employee
            {
                Login = "admin",
                Role = "Admin"
            };

            admin.Password = passwordHasher.HashPassword(admin, "admin123");
            context.Employees.Add(admin);
        }

        var user = await context.Employees.FirstOrDefaultAsync(e => e.Login == "user");

        if (user == null)
        {
            user = new Employee
            {
                Login = "user",
                Role = "User"
            };

            user.Password = passwordHasher.HashPassword(user, "user123");
            context.Employees.Add(user);
        }

        await context.SaveChangesAsync();
    }
}