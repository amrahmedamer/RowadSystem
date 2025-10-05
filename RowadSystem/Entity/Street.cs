namespace RowadSystem.API.Entity;

public class Street
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CityId { get; set; }  // FK
    public City City { get; set; }
}
