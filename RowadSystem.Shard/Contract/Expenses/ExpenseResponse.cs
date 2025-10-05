using RowadSystem.Shard.consts.Enums;

namespace RowadSystem.Shard.Contract.Expenses;
public class ExpenseResponse
{
    public decimal Id { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
    public string ExpenseCategoryName { get; set; }=string.Empty;
}
