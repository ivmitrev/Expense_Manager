using ExpensesManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExpensesManager.Data;

public class ExpenseDbContext : DbContext
{
    public ExpenseDbContext() { }
  
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // It's not a good practice to show the connection string here but we use local database
        // For security measures we can put connection strings in appsettings.json
        optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=ADMIN;Database=ExpenseManager");
    }

    public DbSet<Bill> Bills { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<Transport> Transports { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Leisure> Leisures { get; set; }
    public DbSet<Other> Others { get; set; }
    
    
}

