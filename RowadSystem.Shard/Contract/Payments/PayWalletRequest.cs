namespace RowadSystem.Shard.Contract.Payments;

public record PayWalletRequest
(
    int Amount,
    string PhoneNumber
);
