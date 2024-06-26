using Gleeman.JwtGenerator.Configuration;
using JwtGen.Data;
using JwtGen.Services;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var front = "front";
builder.Services.AddCors(options => {
    options.AddPolicy(name: front,
                      policy => {
                          policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddDbContext<AuthContext>(options => options.UseSqlite(
        builder.Configuration.GetConnectionString("SQLite")
    ));

builder.Services.AddJwtGenerator(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(front);
app.MapControllers();

app.Run();
