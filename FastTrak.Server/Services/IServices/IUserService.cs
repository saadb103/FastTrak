using FastTrak.Shared.DTOs;

namespace FastTrak.Server.Services.IServices;
public interface IUserService
{
    public Task<RegistrationResponseDTO> Register(RegistrationRequestDTO request);
    public Task<AuthenticationResponseDTO> Login(AuthenticationRequestDTO request);
}