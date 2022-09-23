using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

using FastTrak.Server.Services.IServices;
using FastTrak.Shared.Models;
using FastTrak.Shared.DTOs;
using FastTrak.Server.Data;


namespace FastTrak.Server.Services;
public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public UserService(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task<RegistrationResponseDTO> Register(RegistrationRequestDTO request)
    {
        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        bool usernameExists = _context.Users.Where(u => u.Username == request.Username).Count() > 0 ? true : false;

        if (usernameExists)
            return new RegistrationResponseDTO() { isSuccessful = false, Errors = "Another user with the username already exists" };

        User user = new User
        {
            Username = request.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            UserRole = request.UserRole
        };

        await _context.Users.AddAsync(user);
        _context.SaveChanges();

        return new RegistrationResponseDTO() { isSuccessful = true, User = user };
    }

    public async Task<AuthenticationResponseDTO> Login(AuthenticationRequestDTO request)
    {
        User user = _context.Users.Where(u => u.Username == request.Username).FirstOrDefault();

        if (user is null)
            return new AuthenticationResponseDTO() { isSuccessful = false, Errors = "The user does not exist" };

        bool matches = VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);

        if (!matches)
            return new AuthenticationResponseDTO() { isSuccessful = false, Errors = "The username or password is incorrect" };

        return new AuthenticationResponseDTO() { isSuccessful = true, Token = CreateToken(user) };
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using(var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using(var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.UserRole)
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:SecretKey").Value));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}