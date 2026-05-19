using Microsoft.EntityFrameworkCore;
using PondLite.Api.Data;
using PondLite.Api.Repositories;
using PondLite.Api.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PondLiteDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PondLiteDb")));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRelationshipService, RelationshipService>();
builder.Services.AddScoped<IFrogService, FrogService>();
builder.Services.AddScoped<IDailyCheckInService, DailyCheckInService>();
builder.Services.AddScoped<IRibbitService, RibbitService>();
builder.Services.AddScoped<IDiscussionService, DiscussionService>();

builder.Services.AddScoped<IUserAccountRepository, EfUserAccountRepository>();
builder.Services.AddScoped<IAuthTokenRepository, EfAuthTokenRepository>();
builder.Services.AddScoped<IRelationshipRepository, EfRelationshipRepository>();
builder.Services.AddScoped<IRelationshipMemberRepository, EfRelationshipMemberRepository>();
builder.Services.AddScoped<IFrogRepository, EfFrogRepository>();
builder.Services.AddScoped<IDailyCheckInRepository, EfDailyCheckInRepository>();
builder.Services.AddScoped<IRibbitRepository, EfRibbitRepository>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });
builder.Services.AddScoped<IDiscussionPromptRepository, EfDiscussionPromptRepository>();
builder.Services.AddScoped<IDiscussionResponseRepository, EfDiscussionResponseRepository>();


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
