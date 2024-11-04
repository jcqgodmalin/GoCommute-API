namespace GoCommute;

public class Marker
{
    public int Id {get; set;}
    public int RouteId { get; set; }
    public string StreetName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Route Route { get; set; }
}
