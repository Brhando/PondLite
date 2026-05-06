using Microsoft.EntityFrameworkCore;
using PondLite.Api.Data;
using PondLite.Api.Repositories;
using PondLite.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PondLiteDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PondLiteDb")));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRelationshipService, RelationshipService>();

builder.Services.AddScoped<IUserAccountRepository, EfUserAccountRepository>();
builder.Services.AddScoped<IAuthTokenRepository, EfAuthTokenRepository>();
builder.Services.AddScoped<IRelationshipRepository, EfRelationshipRepository>();
builder.Services.AddScoped<IRelationshipMemberRepository, EfRelationshipMemberRepository>();
builder.Services.AddScoped<IFrogRepository, EfFrogRepository>();
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
