using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly string _secretKey;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = "MyVerySecureSuperSecretKey12345!"; // Assurez-vous que la clé est identique à celle de Program.cs
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLogin user)
    {
        try
        {
            // Vérifie les informations d'identification (exemple basique)
            if (user.Username == "admin" && user.Password == "password")
            {
                var token = GenerateJwtToken("Admin");
                return Ok(new { token });
            }
            else if (user.Username == "user" && user.Password == "password")
            {
                var token = GenerateJwtToken("User");
                return Ok(new { token });
            }

            return Unauthorized("Invalid credentials");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private string GenerateJwtToken(string role)
    {
        var key = Encoding.ASCII.GetBytes(_secretKey);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, role) }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

// Modèle pour les informations de connexion de l'utilisateur
public class UserLogin
{
    public string Username { get; set; } = string.Empty;  // Valeur par défaut pour éviter l'erreur nullable
    public string Password { get; set; } = string.Empty;  // Valeur par défaut pour éviter l'erreur nullable
}
