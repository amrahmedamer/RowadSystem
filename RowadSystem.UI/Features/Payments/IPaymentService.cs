using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.consts.Enums;
using RowadSystem.Shard.Contract.Orders;
using RowadSystem.Shard.Contract.Payments;
using RowadSystem.Shard.Contract.Paymob;
using RowadSystem.Shard.Contract.ShoppingCarts;

namespace RowadSystem.UI.Features.Payments;

public interface IPaymentService
{
    Task<Result<List<ShoppingCartResponse>>> GetCartItem();
    Task<Result<OrderResponse>> GetOrder();
    Task<Result> UpdatePaymentMethodOrder(PaymentMethodOrder paymentMethodOrder);
    Task<Result<CheckoutResponse>> ProcessToCheckOutAsync(OrderRequest orderRequest);
    Task<StartPaymentResponse> Wallet(StartWalletPaymentRequest payload);
    Task<StartPaymentResponse> Card(StartPaymentRequest payload);
}
