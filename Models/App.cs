namespace GoCommute.Models
{
    public class App
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string SecretKey { get; set; } = "";
        public string AppID { get; set; } = "";
        public List<string> Role { get; set; } = new List<string>();
        public string RefreshToken { get; set; } = "";
        public DateTime Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        public User User { get; set; }
        
    }
}