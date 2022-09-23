namespace FastTrak.Shared.DTOs;

public class AuthenticationResponseDTO
{
    public bool isSuccessful { get; set; }
    public String Token { get; set; } = String.Empty;
    public String Errors { get; set; } = String.Empty;
}

