﻿namespace GoCommute;

public class User
{
    public int Id { get; set; }
    public string SecretKey { get; set; }
    public string AppID { get; set; }
    public string Role { get; set; }
    public DateTime Create_At { get; set; }
    public DateTime Updated_At { get; set; }
}