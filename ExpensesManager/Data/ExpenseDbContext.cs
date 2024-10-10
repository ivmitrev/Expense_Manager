using ExpensesManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExpensesManager.Data;

public class ExpenseDbContext : DbContext
{
    private readonly IConfiguration configuration;
    public ExpenseDbContext() { }
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DatabasePg"));
    }

    public DbSet<Bill> Bills { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<Transport> Transports { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Leisure> Leisures { get; set; }
    public DbSet<Other> Others { get; set; }
    
    
}

