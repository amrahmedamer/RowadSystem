namespace RowadSystem.Shard.Contract.Acounts;

public record UpdateUserProfileRequest
(
    string FirstName,
    string LastName,
    string PhoneNumber
);

