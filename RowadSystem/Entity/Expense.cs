namespace RowadSystem.API.Entity;

public class Expense : AuditableEntity
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
    public int ExpenseCategoryId { get; set; }
    public ExpenseCategory ExpenseCategory { get; set; }
}
