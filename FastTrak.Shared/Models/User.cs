namespace FastTrak.Shared.Models;

public class User
{
    public int Id { get; set; }
    public String Username { get; set; } = String.Empty;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public int Age { get; set; }
    public String UserRole { get; set; } = "Customer";
}