using Microsoft.EntityFrameworkCore;
using PondLite.Api.Data;
using PondLite.Api.Repositories;
using PondLite.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PondLiteDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PondLiteDb")));
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IUserAccountRepository, InMemoryUserAccountRepository>();
builder.Services.AddSingleton<IAuthTokenRepository, InMemoryAuthTokenRepository>();
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
