using RowadSystem.Shard.consts.Enums;
using System.Reflection.Emit;

namespace RowadSystem.Shard.Contract.Payments;

//public record PaymentRequest
//(
//    decimal Amount,
//    PaymentMethod Method
//);


public class PaymentRequest
{
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
}