using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using TrainFoodDelivery.Controllers.Utils;
using TrainFoodDelivery.Data;
using TrainFoodDelivery.Repository;
using TrainFoodDelivery.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TrainFoodDeliveryContext>(options => options.UseSqlite(
        builder.Configuration.GetConnectionString("sqlite")
    ));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ControllerUtils>();
builder.Services.AddScoped<IDistributedCache, NullCache>();
builder.Services.AddScoped<ITrainRepository, TrainRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
/*builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "redis";
    options.InstanceName = "local";
});*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
