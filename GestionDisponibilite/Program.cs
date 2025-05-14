using FluentValidation.AspNetCore;
using GestionDisponibilite.Data;
using GestionDisponibilite.Mapping;
using GestionDisponibilite.Repository;
using GestionDisponibilite.Service;
using GestionDisponibilite.Validator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<CreateEmployeDtoValidator>()); ;
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddScoped<IProjetRepository, ProjetRepository>();
builder.Services.AddScoped<IEmployeRepository, EmployeRepository>();
builder.Services.AddScoped<IEmployeProjetRepository, EmployeProjetRepository>();
builder.Services.AddScoped<IEmployeService, EmployeService>();
builder.Services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
builder.Services.AddOpenApi();

MapsterConfig.RegisterMappings();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
