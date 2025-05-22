using FluentValidation;
using FluentValidation.AspNetCore;
using GestionDisponibilite.Data;
using GestionDisponibilite.DTOs;
using GestionDisponibilite.Mapping;
using GestionDisponibilite.Options;
using GestionDisponibilite.Repository;
using GestionDisponibilite.Service;
using GestionDisponibilite.Validator;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Contrôleurs
builder.Services.AddControllers();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<UpdateEmployeDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"))
);

// Repositories & Services
builder.Services.AddScoped<IProjetRepository, ProjetRepository>();
builder.Services.AddScoped<IEmployeRepository, EmployeRepository>();
builder.Services.AddScoped<IEmployeProjetRepository, EmployeProjetRepository>();
builder.Services.AddScoped<IEmployeService, EmployeService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IMapper, Mapper>();


// Configuration JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),
        ClockSkew = TimeSpan.FromMinutes(2)
    };
});

// HashPassword
builder.Services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
builder.Services.Configure<PasswordPolicyOptions>(
    builder.Configuration.GetSection("PasswordPolicy"));

// CORS (politique permissive pour dev)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
    );
});

// Scalar API (alternative documentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Mapster config
MapsterConfig.RegisterMappings();

var app = builder.Build();

// Middleware gestion erreurs
app.UseExceptionHandler("/error");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

// IMPORTANT : Ordre correct
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var hasher = new Argon2PasswordHasher();
var adminHash = hasher.HashPassword("Admin@2024!");
var userHash = hasher.HashPassword("User@2024!");
Console.WriteLine(adminHash);
Console.WriteLine(userHash);

app.Run();


