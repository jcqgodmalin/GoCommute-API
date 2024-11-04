namespace GoCommute;

public class RouteDto
{

    public int Id { get; set; }
    public string VehicleType { get; set; }
    public string? BusName { get; set; }
    public string? RouteNumber { get; set; }
    public bool Verified { get; set; }
    public List<Marker> Markers { get; set; }
    public List<RoutePoint> RoutePoints { get; set; }
    public string Created_By { get; set; }
    public string Updated_By { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }

}
