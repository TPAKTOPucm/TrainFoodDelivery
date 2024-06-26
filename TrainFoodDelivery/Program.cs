using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using TrainFoodDelivery.Controllers.Utils;
using TrainFoodDelivery.Data;
using TrainFoodDelivery.Repository;
using TrainFoodDelivery.Services;

var front = "front";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TrainFoodDeliveryContext>(options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("postgres")
    ));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: front,
                      builder => {
                          builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});

builder.Services.AddScoped<ControllerUtils>();
builder.Services.AddScoped<ITrainRepository, TrainRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = builder.Configuration.GetConnectionString("redis");
    options.InstanceName = "train_";
});

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
