using FastTrak.Shared.Models;

namespace FastTrak.Shared.DTOs;

public class RegistrationResponseDTO
{
    public bool isSuccessful { get; set; }
    public User? User { get; set; }
    public String Errors { get; set; } = String.Empty;
}

