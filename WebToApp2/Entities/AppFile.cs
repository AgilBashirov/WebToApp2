namespace WebToApp2.Entities;

public class AppFile : CommonEntity
{
    public string Name { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Extension { get; set; } = null!;
}