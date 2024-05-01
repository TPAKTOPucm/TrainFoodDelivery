using Microsoft.EntityFrameworkCore;
using TrainFoodDelivery.Models;

namespace TrainFoodDelivery.Data;

public class TrainFoodDeliveryContext: DbContext
{
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Train> Trains { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<ProductOrder> ProductOrders { get; set; }
    public DbSet<WagonProduct> WagonProducts { get; set; }
    public TrainFoodDeliveryContext(DbContextOptions options): base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<WagonProduct>().HasKey(wp => new { wp.TrainNumber, wp.WagonNumber, wp.ProductId });
        modelBuilder.Entity<ProductOrder>().HasKey(po => new { po.ProductId, po.OrderId });
    }
}
