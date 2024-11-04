namespace GoCommute;

public class RoutePoint
{
    public int Id { get; set; }
    public int RouteId {get; set;}
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Route Route { get; set; }

}
