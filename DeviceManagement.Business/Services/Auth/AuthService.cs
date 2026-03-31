using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DeviceManagement.Business.Models.Auth;
using DeviceManagement.Data;
using DeviceManagement.Data.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace DeviceManagement.Business.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IAuthRepository authRepository, IConfiguration configuration)
    {
        _authRepository = authRepository;
        _configuration = configuration;
    }

    public async Task<AuthResultDto> RegisterAsync(RegisterDto request)
    {
        var existingUser = await _authRepository.RetrieveUserByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new Exception("User already exists.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        var newUser = new UserDto
        {
            Email = request.Email,
            PasswordHash = passwordHash
        };

        await _authRepository.InsertUserAsync(newUser);

        return new AuthResultDto
        {
            Token = GenerateJwtToken(newUser)
        };
    }

    public async Task<AuthResultDto> LoginAsync(LoginDto request)
    {
        var existingUser = await _authRepository.RetrieveUserByEmailAsync(request.Email);
        if (existingUser == null)
        {
            throw new Exception("Invalid email or password.");
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, existingUser.PasswordHash);
        if (!isPasswordValid)
        {
            throw new Exception("Invalid email or password.");
        }

        return new AuthResultDto
        {
            Token = GenerateJwtToken(existingUser)
        };
    }

    private string GenerateJwtToken(UserDto user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(int.Parse(jwtSettings["ExpiryInDays"]!)),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
