using DeLong.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Infrastructure.Contexts;

public class AppDbContext : DbContext
{
    public DbSet<Asset> Assets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionItem> TransactionItems { get; set; }
    public DbSet<CashRegister> CashRegisters { get; set; }
    public DbSet<CashTransfer> CashTransfers { get; set; }
    public DbSet<CashWarehouse> CashWarehouses { get; set; }
    public DbSet<KursDollar> KursDollars { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }
    public DbSet<Debt> Debts { get; set; }
    public DbSet<DebtPayment> DebtPayments { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<ReturnProduct> ReturnProducts { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>().HasIndex(rt => rt.Token).IsUnique();

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Sale>()
            .HasOne(i => i.Customer)
            .WithMany()
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}