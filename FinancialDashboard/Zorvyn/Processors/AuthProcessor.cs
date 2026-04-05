using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class AuthProcessor : IAuthProcessor
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthProcessor> _logger;
    public AuthProcessor(AppDbContext context, IConfiguration config,ILogger<AuthProcessor> logger)
    {
        _context = context;
        _config = config;
        _logger = logger;
    }

    public async Task<LoginResponse> ProcessLoginAsync(LoginRequest request)
    {
        try
        {
            _logger.LogInformation("Validating user {Email}", request.Email);

            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                x.Email == request.Email &&
                x.IsActive &&
                !x.IsDeleted);

            if (user == null)
            {
                throw new Exception("Invalid credentials");
            }
            if (user.IsFirstLogin)
            {
                if (user.PasswordHash != request.Password)
                throw new Exception("Invalid password");

                var hashedPassword = HashPassword(request.Password);

                user.PasswordHash = hashedPassword;
                user.IsFirstLogin = false;
                user.UpdatedAt = DateTime.UtcNow;
                user.UpdatedBy = user.Email;

                await _context.SaveChangesAsync();
            }
            else
            {
                if (HashPassword(request.Password) != user.PasswordHash)
                {
                    throw new Exception("Invalid credentials");
                }
            }
            var token = GenerateJwtToken(user);
            return new LoginResponse
            {
                Email = request.Email,
                Token = token
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login processing");
            throw;
        }
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

        return Convert.ToBase64String(bytes);
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddHours(2),
            claims: claims,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}