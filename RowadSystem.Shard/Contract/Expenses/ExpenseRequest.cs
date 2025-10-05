

using RowadSystem.Shard.consts.Enums;

namespace RowadSystem.Shard.Contract.Expenses;
public class ExpenseRequest
{
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
    public int ExpenseCategoryId { get; set; }
}
