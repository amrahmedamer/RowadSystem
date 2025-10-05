namespace RowadSystem.API.Entity;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int GovernorateId { get; set; }  
    public Governorate Governorate { get; set; } = null!;
    //public List<Street> Streets { get; set; } = new();
}
