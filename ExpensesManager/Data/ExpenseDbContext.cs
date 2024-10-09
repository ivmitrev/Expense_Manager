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
}