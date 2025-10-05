namespace RowadSystem.API.Entity;

public class Governorate
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<City> Cities { get; set; }
}
