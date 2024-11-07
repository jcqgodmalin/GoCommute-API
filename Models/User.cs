using GoCommute.Models;

namespace GoCommute;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<string> Role {get; set;} = new List<string>();
    public List<App> Apps { get; set; } = new List<App>();
    public DateTime Created_At { get; set; }
    public DateTime? Updated_At { get; set; }
}
