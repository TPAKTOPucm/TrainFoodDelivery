﻿using Microsoft.EntityFrameworkCore;
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
        await _db.ProductOrders.AddAsync(new ProductOrder
        {
            ProductId = productId,
            Amount = amount,
            OrderId = orderId
        });
        await _db.SaveChangesAsync();
    }

    public async Task CreateOrder(OrderDto order)
    {
        await _db.AddAsync(new Order
        {
            Products = order.Products.Select(p => new ProductOrder
            {
                Amount = p.Amount,
                ProductId = p.Id
            }),
            Status = order.Status,
            TicketId = order.Ticket.Id
        });
        await _db.SaveChangesAsync();
    }

    public async Task CreateProduct(ProductDto product)
    {
        await _db.AddAsync(new Product
        {
            Name = product.Name,
            Cost = product.Cost,
            Description = product.Description,
            ImagePath = product.ImagePath,
            RecipeId = product.RecipeId
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
            RecipeId = product.RecipeId
        };
    }

    public Task<OrderDto> GetOrder(int id)
    {
        return _db.Orders.Where(o => o.Id == id).Select(o => new OrderDto
        {
            Id = o.Id,
            Products = o.Products.Select(p => new ProductDto
            {
                Id = p.ProductId,
                Cost = p.Product.Cost,
                Name = p.Product.Name,
                ImagePath = p.Product.ImagePath,
                RecipeId = p.Product.RecipeId,
                NearestWagons = p.Product.WagonProducts
                    .Where
                    (
                        wp => wp.ProductAmount >= p.Amount
                        && wp.TrainNumber == o.Ticket.TrainNumber
                    )
                    .OrderBy(wp => Math.Abs(wp.WagonNumber - o.Ticket.WagonNumber))
                    .Select(wp => wp.WagonNumber).Take(3)
            }),
            Status = o.Status,
            Ticket = new TicketDto
            {
                TrainNumber = o.Ticket.TrainNumber,
                WagonNumber = o.Ticket.WagonNumber,
                SeatNumber = o.Ticket.SeatNumber,
                User = o.Ticket.User
            }
        }).FirstOrDefaultAsync();
    }

    public Task<List<OrderDto>> GetOrders(int trainNumber, int wagonNumber) =>
        _db.Orders.Where(o => o.Ticket.TrainNumber == trainNumber && o.Ticket.WagonNumber == wagonNumber)
        .Select(o => new OrderDto
        {
            Id = o.Id,
            Products = o.Products.Select(p => new ProductDto
            {
                Name = p.Product.Name,
                Cost = p.Product.Cost
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

    public Task<ProductDto> GetProduct(int id) => _db.Products.Where(p => p.Id == id).Select(p => new ProductDto
    {
        Id = p.Id,
        ImagePath = p.ImagePath,
        Cost = p.Cost,
        Name = p.Name,
        Description = p.Description,
        Amount = p.WagonProducts.Where(wp => wp.ProductId == p.Id).Sum(wp => wp.ProductAmount)
    }).FirstOrDefaultAsync();

    public Task<List<ProductDto>> GetProducts(int trainNumber) =>
        _db.Products.Where(p => p.WagonProducts.Any(wp => wp.TrainNumber == trainNumber))
        .Select(p => new ProductDto 
        { 
            ImagePath = p.ImagePath,
            Cost = p.Cost,
            Name = p.Name,
            Amount = p.WagonProducts.Where(wp => wp.TrainNumber == trainNumber).Sum(wp => wp.ProductAmount)
        })
        .ToListAsync();

    public async Task RemoveProductFromOrder(int orderId, int productId, int amount = 0)
    {
        var po = await _db.ProductOrders.Where(po => po.OrderId == orderId && po.ProductId == productId).FirstOrDefaultAsync();
        if(amount == 0)
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

    public Task UpdateOrder(OrderDto order)
    {
        _db.Update(new Order
        {
            Id = order.Id,
            Status = order.Status,
            TicketId = order.Ticket.Id
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
            ImagePath = product.ImagePath,
            RecipeId = product.RecipeId
        });
        return _db.SaveChangesAsync();
    }
}