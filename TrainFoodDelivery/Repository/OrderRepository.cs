using Microsoft.EntityFrameworkCore;
using TrainFoodDelivery.Data;
using TrainFoodDelivery.DTOs;
using TrainFoodDelivery.Models;

namespace TrainFoodDelivery.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly TrainFoodDeliveryContext _db;
    public OrderRepository(TrainFoodDeliveryContext context)
    {
        _db = context;
    }
    public async Task AddProductToOrder(int orderId, int productId, int amount)
    {
        var productAmount = await _db.WagonProducts.Where(wp => wp.ProductId == productId).SumAsync(wp => wp.ProductAmount);
        if(productAmount < amount)
            throw new Exception("Not enough products in train");
        var product = await _db.ProductOrders.Where(p => p.ProductId == productId && p.OrderId == orderId).FirstOrDefaultAsync();
        if(product is null)
            await _db.ProductOrders.AddAsync(new ProductOrder
            {
                ProductId = productId,
                Amount = amount,
                OrderId = orderId
            });
        else
            product.Amount += amount;
        var wp = _db.WagonProducts.Where(wp => wp.ProductId == productId).FirstOrDefault();
        wp.ProductAmount -= amount;
            _db.Update(wp);
        await _db.SaveChangesAsync();
    }

    public async Task<OrderDto> CreateOrder(OrderDto order)
    {
        var entity = new Order
        {
            Products = order.Products?.Select(p => new ProductOrder
            {
                Amount = p.Amount,
                ProductId = p.Id
            }),
            Status = order.Status,
            TicketId = order.TicketId ?? 0
        };
        await _db.AddAsync(entity);
        await _db.SaveChangesAsync();
        order.Id = entity.Id;
        return order;
    }

    public async Task CreateProduct(ProductDto product)
    {
        await _db.AddAsync(new Product
        {
            Name = product.Name,
            Cost = product.Cost,
            Description = product.Description,
            ImagePath = product.ImagePath,
            Netto = product.OneAmount,
            NettoType = Enum.Parse<NettoType>(product.VolumeType)
        });
        await _db.SaveChangesAsync();
    }

    public async Task<ProductDto> DeleteProduct(int productId)
    {
        var product = await _db.Products.FindAsync(productId);
        _db.Products.Remove(product);
        _db.SaveChanges();
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Cost = product.Cost,
            Description = product.Description,
            ImagePath = product.ImagePath,
            RecipeId = product.Recipe.Id
        };
    }

    public async Task<OrderDto> GetOrder(int id)
    {
        var order = await _db.Orders.Where(o => o.Id == id).Select(o => new OrderDto
        {
            Id = o.Id,
            Products = o.Products.Select(p => new ProductDto
            {
                Id = p.ProductId,
                Cost = p.Product.Cost,
                Name = p.Product.Name,
                Amount = p.Amount,
                OneAmount = p.Product.Netto,
                VolumeType = p.Product.NettoType.ToString(),
                ImagePath = p.Product.ImagePath,
                RecipeId = p.Product.Recipe.Id,
                /*NearestWagons = p.Product.WagonProducts
                    .Where
                    (
                        wp => wp.ProductAmount >= p.Amount
                        && wp.TrainNumber == o.Ticket.TrainNumber
                    )
                    .OrderBy(wp => Math.Abs(wp.WagonNumber - o.Ticket.WagonNumber))
                    .Select(wp => wp.WagonNumber).Take(1)*/
            }),
            Status = o.Status,
            PaymentType = o.PaymentType,
            TicketId = o.TicketId,
            Ticket = new TicketDto
            {
                Id = o.TicketId,
                TrainNumber = o.Ticket.TrainNumber,
                WagonNumber = o.Ticket.WagonNumber,
                SeatNumber = o.Ticket.SeatNumber,
                User = o.Ticket.User
            }
        }).FirstOrDefaultAsync();
        foreach (var product in order.Products)
        {
            var amount = await _db.WagonProducts.Where(p => p.ProductId == product.Id && p.WagonNumber == order.Ticket.WagonNumber).Select(p => p.ProductAmount).FirstOrDefaultAsync();
            if (amount <= product.Amount)
                product.NearestWagons = new NearestWagonsInfo()
                {
                    Amount = product.Amount - amount,  //не хватает
                    Number = _db.WagonProducts.Where
                        (
                            wp => wp.ProductId == product.Id
                            && wp.ProductAmount >= product.Amount
                            && wp.TrainNumber == order.Ticket.TrainNumber
                        )
                        .OrderBy(wp => Math.Abs(wp.WagonNumber - order.Ticket.TrainNumber))
                        .Select(wp => wp.WagonNumber).Take(1)
                };
            else
                product.NearestWagons = new();
        }
        return order;
    }

    public Task<List<OrderDto>> GetOrders(int trainNumber, int wagonNumber) =>
        _db.Orders.Where(o => o.Ticket.TrainNumber == trainNumber && o.Ticket.WagonNumber == wagonNumber && (o.Status == OrderStatus.Cooked || o.Status == OrderStatus.Ordered || o.Status == OrderStatus.Delivering)) // not Ordering
        .Select(o => new OrderDto
        {
            Id = o.Id,
            PaymentType = o.PaymentType,
            Products = o.Products.Select(p => new ProductDto
            {
                Name = p.Product.Name,
                Cost = p.Product.Cost,
                Amount = p.Amount
            }),
            Status = o.Status,
            Ticket = new TicketDto
            {
                TrainNumber = o.Ticket.TrainNumber,
                WagonNumber = o.Ticket.WagonNumber,
                SeatNumber = o.Ticket.SeatNumber,
                User = o.Ticket.User
            }
        }).ToListAsync();

    public Task<List<OrderDto>> GetOrders(int trainNumber, int wagonNumber, OrderStatus status) =>
       _db.Orders.Where(o => o.Ticket.TrainNumber == trainNumber && (wagonNumber == 0 || o.Ticket.WagonNumber == wagonNumber) && o.Status == status)
       .Select(o => new OrderDto
       {
           Id = o.Id,
           PaymentType = o.PaymentType,
           Products = o.Products.Select(p => new ProductDto
           {
               Name = p.Product.Name,
               Cost = p.Product.Cost,
               Amount = p.Amount
           }),
           Status = o.Status,
           Ticket = new TicketDto
           {
               TrainNumber = o.Ticket.TrainNumber,
               WagonNumber = o.Ticket.WagonNumber,
               SeatNumber = o.Ticket.SeatNumber,
               User = o.Ticket.User
           }
       }).ToListAsync();

    public Task<List<OrderDto>> GetOrders(int ticketId) =>
        _db.Orders.Where(o => o.TicketId == ticketId && o.Status != OrderStatus.Ordering && o.Status != OrderStatus.Completed).Select(o => new OrderDto
        {
            Id = o.Id,
            Status = o.Status,
            PaymentType = o.PaymentType,
            Products = o.Products.Select(p => new ProductDto
            {
                Id=p.ProductId,
                Name = p.Product.Name,
                Cost = p.Product.Cost,
                Amount = p.Amount
            })
        }).ToListAsync();

    public async Task<OrderDto> GetCart(int ticketId)
    {
        var cart = await _db.Orders.Where(o => o.TicketId == ticketId && o.Status == OrderStatus.Ordering).Select(o => new OrderDto
        {
            Id = o.Id,
            Status = o.Status,
            Products = o.Products.Select(p => new ProductDto
            {
                Id = p.ProductId,
                Name = p.Product.Name,
                Cost = p.Product.Cost,
                ImagePath = p.Product.ImagePath,
                OneAmount = p.Product.Netto,
                VolumeType = p.Product.NettoType.ToString(),
                Amount = p.Amount
            })
        }).FirstOrDefaultAsync();
        if(cart is null)
            return await CreateOrder(new()
            {
                Status = OrderStatus.Ordering,
                TicketId = ticketId
            });
        return cart;
    }

    public async Task AddProductToWagon(int productId, int trainNumber, int wagonNumber, int amount)
    {
        var product = await _db.WagonProducts.Where(wp => wp.TrainNumber == trainNumber && wp.WagonNumber == wagonNumber && wp.ProductId == productId).FirstOrDefaultAsync();
        if (product is null) {
            if (amount < 0) return;
            _db.Add(new WagonProduct {
                ProductId = productId,
                TrainNumber = trainNumber,
                WagonNumber = wagonNumber,
                ProductAmount = amount
            });
        } else {
            product.ProductAmount += amount;
            if (product.ProductAmount < 0)
                _db.Remove(product);
            else
                _db.Update(product);
        }
        await _db.SaveChangesAsync();
    }

    public Task<ProductDto> GetProduct(int id) => _db.Products.Where(p => p.Id == id).Select(p => new ProductDto
    {
        Id = p.Id,
        ImagePath = p.ImagePath,
        Cost = p.Cost,
        Name = p.Name,
        Description = p.Description,
        OneAmount = p.Netto,
        VolumeType = p.NettoType.ToString(),
        Amount = p.WagonProducts.Where(wp => wp.ProductId == p.Id).Sum(wp => wp.ProductAmount)
    }).FirstOrDefaultAsync();

    public Task<List<ProductDto>> GetProducts(int trainNumber, int? wagonNumber = null) =>
        _db.Products/*.Where(p => p.WagonProducts.Any(wp => wp.TrainNumber == trainNumber && (wagonNumber == null || wp.WagonNumber == wagonNumber)))*/
        .OrderBy(p => p.WagonProducts.Where(wp => wp.TrainNumber == trainNumber && (wagonNumber == null || wp.WagonNumber == wagonNumber)).Sum(wp => wp.ProductAmount))
        .Select(p => new ProductDto 
        { 
            Id = p.Id,
            ImagePath = p.ImagePath,
            Cost = p.Cost,
            Name = p.Name,
            Description = p.Description,
            OneAmount = p.Netto,
            VolumeType = p.NettoType.ToString(),
            Amount = p.WagonProducts.Where(wp => wp.TrainNumber == trainNumber && (wagonNumber == null || wp.WagonNumber == wagonNumber)).Sum(wp => wp.ProductAmount)
        })
        .ToListAsync();

    public async Task RemoveProductFromOrder(int orderId, int productId, int amount = 0)
    {
        var po = await _db.ProductOrders.Where(po => po.OrderId == orderId && po.ProductId == productId).FirstOrDefaultAsync();
        var wp = _db.WagonProducts.Where(wp => wp.ProductId == productId).FirstOrDefault();
        wp.ProductAmount += amount;
        _db.Update(wp);
        if (amount == 0)
            _db.ProductOrders.Remove(po);
        else
        {
            po.Amount -= amount;
            if(po.Amount <= 0)
                _db.ProductOrders.Remove(po);
            else
                _db.Update(po);
        }
        await _db.SaveChangesAsync();
    }

    public async Task ConfirmOrder(OrderDto order)
    {
        var coockable = await _db.ProductOrders.Include(po => po.Product.Recipe).Where(po => po.Product.Recipe != null && po.OrderId == order.Id).ToListAsync();
        if (coockable.Count == 0)
        {
            order.Status = OrderStatus.Cooked;
        }
        else
        {
            foreach (var item in coockable)
            {
                _db.Add(new OrderRecipe
                {
                    Amount = item.Amount,
                    OrderId = item.OrderId,
                    RecipeId = item.Product.Recipe.Id
                });
            }
            order.Status = OrderStatus.Ordered;
        }
        await UpdateOrder(order);
    }

    public Task UpdateOrder(OrderDto order)
    {
        _db.Update(new Order
        {
            Id = order.Id,
            Status = order.Status,
            TicketId = order.Ticket.Id,
            PaymentType = order.PaymentType,
        });
        return _db.SaveChangesAsync();
    }

    public Task UpdateProduct(ProductDto product)
    {
        _db.Update(new Product
        {
            Id = product.Id,
            Name = product.Name,
            Cost = product.Cost,
            Description = product.Description,
            ImagePath = product.ImagePath
        });
        return _db.SaveChangesAsync();
    }
}
