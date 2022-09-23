namespace FastTrak.Shared.DTOs;

public class RegistrationRequestDTO
{
    public string Username { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string ConfirmPassword { get; set; } = String.Empty;
    public String UserRole { get; set; } = "Customer";
}

