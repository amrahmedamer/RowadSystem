using RowadSystem.API.Entity;

namespace RowadSystem.Entity;

public  class Address
{
    public int Id { get; set; }

    public int GovernorateId { get; set; }
    public Governorate Governorate { get; set; } = null!;

    public int CityId { get; set; }
    public City City { get; set; } = null!;

    public string? Street { get; set; }
    public string? AddressDetails { get; set; }
}
