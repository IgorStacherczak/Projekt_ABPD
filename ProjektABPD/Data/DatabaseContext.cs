using Microsoft.EntityFrameworkCore;
using ProjektABPD.Models;

namespace ProjektABPD.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<SoftwareVersion> SoftwareVersions { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>().HasKey(c => c.IdClient);
        modelBuilder.Entity<Software>().HasKey(s => s.IdSoftware);
        modelBuilder.Entity<SoftwareVersion>().HasKey(sv => sv.IdSoftwareVersion);
        modelBuilder.Entity<Discount>().HasKey(d => d.IdDiscount);
        modelBuilder.Entity<Contract>().HasKey(c => c.IdContract);
        modelBuilder.Entity<Payment>().HasKey(p => p.IdPayment);
        modelBuilder.Entity<Employee>().HasKey(e => e.IdEmployee);

        modelBuilder.Entity<Contract>()
            .Property(c => c.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Contract>()
            .HasOne(c => c.Client)
            .WithMany(c => c.Contracts)
            .HasForeignKey(c => c.IdClient)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SoftwareVersion>()
            .HasOne(v => v.Software)
            .WithMany(s => s.Versions)
            .HasForeignKey(v => v.IdSoftware)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Contract>()
            .HasOne(c => c.Software)
            .WithMany(s => s.Contracts)
            .HasForeignKey(c => c.IdSoftware)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Contract>()
            .HasOne(c => c.SoftwareVersion)
            .WithMany(v => v.Contracts)
            .HasForeignKey(c => c.IdSoftwareVersion)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Contract)
            .WithMany(c => c.Payments)
            .HasForeignKey(p => p.IdContract)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Discount>()
            .Property(d => d.Percentage)
            .IsRequired();

        modelBuilder.Entity<Employee>()
            .Property(e => e.Login)
            .HasMaxLength(100);

        modelBuilder.Entity<Employee>()
            .Property(e => e.Password)
            .HasMaxLength(255);

        modelBuilder.Entity<Employee>()
            .Property(e => e.Role)
            .HasMaxLength(50);

        modelBuilder.Entity<Software>().HasData(
            new Software
            {
                IdSoftware = 1,
                Name = "System CRM",
                Description = "System do zarządzania klientami",
                Category = "Business"
            }
        );

        modelBuilder.Entity<SoftwareVersion>().HasData(
            new SoftwareVersion
            {
                IdSoftwareVersion = 1,
                VersionNumber = "1.0",
                IdSoftware = 1
            }
        );

        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                IdEmployee = 1,
                Login = "admin",
                Password = "admin123",
                Role = "Admin"
            },
            new Employee
            {
                IdEmployee = 2,
                Login = "user",
                Password = "user123",
                Role = "User"
            }
        );
    }
}