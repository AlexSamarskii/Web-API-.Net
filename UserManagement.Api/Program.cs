using Microsoft.EntityFrameworkCore;
using UserManagement.Api.Middleware;
using UserManagement.Application.Services;
using UserManagement.Infrastructure.Data;
using UserManagement.Infrastructure.Repositories;
using UserManagement.Application.Security;
using UserManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using UserManagement.Application.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DbInitializer.Initialize(services);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseRequestLogging();        
app.UseExceptionHandling();  

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
