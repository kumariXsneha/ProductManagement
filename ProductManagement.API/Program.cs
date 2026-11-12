using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.Services;
using ProductManagement.API.Patterns;
using ProductManagement.API.Mappings;
using Microsoft.AspNetCore.Identity;
using ProductManagement.API.Models;

var builder = WebApplication.CreateBuilder(args);

// ===== Register services =====
builder.Services.AddControllers();
// AutoMapper: finds MappingProfile and registers IMapper for DI
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Auth services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


// DbContext (Scoped by default)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Your own services
builder.Services.AddScoped<ISupplierService, SupplierService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== Configure the HTTP pipeline =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
// Count every request using the Singleton
app.Use(async (context, next) =>
{
    ApiStatistics.Instance.RecordRequest();
    await next();
});
app.MapControllers();

app.Run();