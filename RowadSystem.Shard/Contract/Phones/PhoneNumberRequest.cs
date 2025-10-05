namespace RowadSystem.Shard.Contract.Phones;

//public record PhoneNumberRequest
//(
//    string Number,
//    bool IsPrimary
//);

public class PhoneNumberRequest
{
    public string Number { get; set; } = string.Empty;
    public bool IsPrimary { get; set; } = false;
}