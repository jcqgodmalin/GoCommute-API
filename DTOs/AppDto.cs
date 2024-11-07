namespace GoCommute.DTOs
{
    public class AppDto
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
    }

    public class NewAppDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<string> Role { get; set; }
    }

    public class AppGenerateTokenDto {
        public string SecretKey { get; set; }
        public string AppID { get; set; }
    }

    public class AppRefreshTokenDto {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}