namespace RowadSystem.Shard.Contract.Acounts;

public record ChangePasswordRequest
(
    string OldPassword,
    string NewPassword
);
