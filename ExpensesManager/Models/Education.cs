using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.Models;

public class Education
{
    public int Id { get; set; }
    public string Name { get; set; }
    [Precision(6, 2)]
    public double Price { get; set; }
    public DateOnly Date { get; set; }
}