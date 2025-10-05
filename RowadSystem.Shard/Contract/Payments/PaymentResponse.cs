

using RowadSystem.Shard.consts.Enums;

namespace RowadSystem.Shard.Contract.Payments;
public class PaymentResponse
{
    public PaymentMethod Method { get; set; } 
    public decimal Amount { get; set; } 
    public DateTime PaymentDate { get; set; } // Date of payment
}
