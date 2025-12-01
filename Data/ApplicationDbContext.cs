using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InvoiceSystem.Models;

namespace InvoiceSystem.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<InvoiceHeader> InvoiceHeaders { get; set; }
    public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // DateOnly conversion
        modelBuilder.Entity<InvoiceHeader>()
            .Property(x => x.OrderDate)
            .HasConversion(
                v => v.ToDateTime(TimeOnly.MinValue),
                v => DateOnly.FromDateTime(v)
            );

        modelBuilder.Entity<Item>()
            .Property(x => x.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InvoiceDetail>()
            .Property(x => x.UnitPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InvoiceHeader>()
            .Property(x => x.TotalFee)
            .HasPrecision(18, 2);

        // Make Customer.EmailAddress unique
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.EmailAddress)
            .IsUnique();
    }

}
