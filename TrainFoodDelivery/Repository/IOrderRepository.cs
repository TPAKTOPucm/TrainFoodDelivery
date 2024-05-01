﻿using TrainFoodDelivery.DTOs;

namespace TrainFoodDelivery.Repository;

public interface IOrderRepository
{
    public Task<List<OrderDto>> GetOrders(int trainNumber, int wagonNumber);
    public Task UpdateOrder(OrderDto order);
    public Task<OrderDto> GetOrder(int id);
    public Task CreateOrder(OrderDto order);
    public Task AddProductToOrder(int orderId, int productId, int amount);
    public Task RemoveProductFromOrder(int orderId, int productId, int amount = 0);

    public Task<ProductDto> GetProduct(int id);
    public Task<List<ProductDto>> GetProducts(int trainNumber);

    public Task CreateProduct(ProductDto product);
    public Task UpdateProduct(ProductDto product);
    public Task<ProductDto> DeleteProduct(int productId);
}