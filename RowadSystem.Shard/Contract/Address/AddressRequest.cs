
namespace RowadSystem.Shard.Contract.Address;

//public record AddressRequest
//(
//    string City,
//    string Governorate,
//    string Street,
//    string? Notes
//);

public class AddressRequest
{
    public int GovernorateId { get; set; } 
    public int CityId { get; set; }
    public string? Street { get; set; }
    public string? AddressDetails { get; set; }
}