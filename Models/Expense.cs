using SQLite;
namespace BusinessExpenseTracker.Models;

[Table("Expenses")]
public class Expense
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [NotNull, MaxLength(150)]
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    [NotNull, MaxLength(100)]
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
}
