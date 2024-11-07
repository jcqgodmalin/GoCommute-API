namespace GoCommute.DTOs
{
    public class CommuterDto
    {
        public int Id { get; set; }
        public double StartingLat { get; set; }
        public double StartingLon { get; set; }
        public double DestinationLat { get; set; }
        public double DestinationLon { get; set; }
        public DateTime Created_At { get; set; }
    }
}