using DeLong.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Infrastructure.Contexts;

public class AppDbContext : DbContext
{
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<CashRegister> CashRegisters { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }


}
