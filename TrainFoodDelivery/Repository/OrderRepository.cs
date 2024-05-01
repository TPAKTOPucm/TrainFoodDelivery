using Microsoft.EntityFrameworkCore;
using TrainFoodDelivery.Data;
using TrainFoodDelivery.Models;

namespace TrainFoodDelivery.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly TrainFoodDeliveryContext _db;
    public OrderRepository(TrainFoodDeliveryContext context)
    {
        _db = context;
    }
    public bool AddProductToOrder(int orderId, int productId, int amount)
    {
        throw new NotImplementedException();
    }

    public bool CreateOrder(Order order)
    {
        throw new NotImplementedException();
    }

    public bool CreateProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public Product DeleteProduct(int productId)
    {
        throw new NotImplementedException();
    }

    public async Task<Order> GetOrder(int id)
    {
        var order = await _db.Orders.Include(o => o.Products).ThenInclude(o => o.Product).Where(o => o.Id == id).FirstOrDefaultAsync();
        if (order is null)
            return null;
        foreach(var po in order.Products)
        {
            await _db.Entry<ProductOrder>(po).Reference(p => p.Product).LoadAsync();
            await _db.Entry<Product>(po.Product).Collection(p => p.WagonProducts).LoadAsync();
            po.Product.NearestWagon = po.Product.WagonProducts.Where(wp => wp.ProductAmount >= po.Amount && wp.TrainNumber == order.Ticket.TrainNumber).OrderBy(wp => Math.Abs(wp.WagonNumber - order.Ticket.WagonNumber)).Select(wp => wp.WagonNumber).FirstOrDefault();
        }
        return order;
    }

    public async Task<IEnumerable<Order>> GetOrders(int trainNumber, int wagonNumber)
    {
        return await _db.Orders.Include(o=>o.Products).ThenInclude(o => o.Product).Where(o => o.Ticket.TrainNumber == trainNumber && o.Ticket.WagonNumber == wagonNumber).ToArrayAsync();
    }

    public Product GetProduct(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product> GetProducts(int trainNumber)
    {
        throw new NotImplementedException();
    }

    public bool RemoveProductFromOrder(int orderId, int productId, int amount = 0)
    {
        throw new NotImplementedException();
    }

    public bool UpdateOrder(Order order)
    {
        throw new NotImplementedException();
    }

    public bool UpdateProduct(Product product)
    {
        throw new NotImplementedException();
    }
}
