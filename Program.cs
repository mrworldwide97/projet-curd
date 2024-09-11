using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql; // Assurez-vous d'avoir ajouté cette directive using
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuration de la chaîne de connexion à la base de données
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty; // Assurez-vous que connectionString n'est jamais null

// Ajouter les services au conteneur.
builder.Services.AddControllers();

// Ajouter le service de la base de données avec Pomelo.EntityFrameworkCore.MySql
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configuration JWT
var key = Encoding.ASCII.GetBytes("MyVerySecureSuperSecretKey12345!"); // Assurez-vous que la clé est identique à celle utilisée dans AuthController
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
