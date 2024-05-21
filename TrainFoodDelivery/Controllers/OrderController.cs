using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TrainFoodDelivery.Controllers.Utils;
using TrainFoodDelivery.DTOs;
using TrainFoodDelivery.Models;
using TrainFoodDelivery.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainFoodDelivery.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _repository;
    private readonly IDistributedCache _cache;
    private readonly ControllerUtils _utils;
    public OrderController(IOrderRepository repository, IDistributedCache cache, ControllerUtils utils)
    {
        _repository = repository;
        _cache = cache;
        _utils = utils;
    }

    [HttpGet]
    public async Task<IActionResult> Products(string jwt, int ticketIndex)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Customer);
        if (ticket is null)
            return BadRequest();

        var key = "P" + ticket.TrainNumber + "_" + ticket.WagonNumber;
        var json = await _cache.GetStringAsync(key);
        if (json == "")
        {
            json = JsonSerializer.Serialize(await _repository.GetProducts(ticket.TrainNumber), ControllerUtils._serializerOptions);
            _cache.SetStringAsync(key, json);
        }
        return Ok(json);
    }

    // GET api/<OrderControllerController>/5 + способ оплаты
    [HttpGet]
    public async Task<IActionResult> Product(int id)
    {
        if(id == 0)
            return BadRequest();
        var key = "p" + id;
        var json = await _cache.GetStringAsync(key);
        if (json == "")
        {
            json = JsonSerializer.Serialize(await _repository.GetProduct(id),ControllerUtils._serializerOptions);
            _cache.SetStringAsync(key, json);
        }
        return Ok(json);

    }

    // POST api/<OrderControllerController>
    [HttpGet]
    public async Task<IActionResult> Orders(string jwt, int ticketIndex)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Customer);
        if (ticket is null)
            return Forbid();
        var key = "O" + jwt;
        var json = await _cache.GetStringAsync(key);
        if (json == "")
        {
            json = JsonSerializer.Serialize(await _repository.GetOrders(ticket.Id),ControllerUtils._serializerOptions);
            _cache.SetStringAsync(key, json);
        }
        return Ok(json);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(string jwt, int ticketIndex, int productId, int orderId, int amount)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Customer);
        if (ticket is null)
            return Forbid();
        var order = await _repository.GetOrder(orderId);
        if(order.Status != OrderStatus.Ordering)
            return BadRequest();
        await _repository.AddProductToOrder(orderId, productId, amount);
        await _cache.RemoveAsync("O" + jwt);
        await _cache.RemoveAsync("o" + orderId);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(string jwt, int ticketIndex, int productId, int orderId, int amount)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Customer);
        if (ticket is null)
            return Forbid();
        var order = await _repository.GetOrder(orderId);
        if (order.Status != OrderStatus.Ordering || order.TicketId != ticket.Id)
            return BadRequest();
        await _repository.RemoveProductFromOrder(orderId, productId, amount);
        await _cache.RemoveAsync("O" + jwt);
        await _cache.RemoveAsync("o" + orderId);
        await _cache.RemoveAsync("O" + ticket.TrainNumber + "_" + ticket.WagonNumber);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(string jwt, int ticketIndex, OrderDto order)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Customer);
        if (ticket is null)
            return Forbid();
        order.Status = OrderStatus.Ordering;
        order.TicketId = ticket.Id;
        order = await _repository.CreateOrder(order);
        await _cache.RemoveAsync("O" + jwt);
        return Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> GetCart(string jwt, int ticketIndex)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Customer);
        if(ticket is null)
            return BadRequest();
        var cart = await _repository.GetCart(ticket.Id);
        return Ok(JsonSerializer.Serialize(cart,ControllerUtils._serializerOptions));
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmOrder(string jwt, int ticketIndex, int orderId)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Customer);
        var order = await _repository.GetOrder(orderId);
        if (order.Status != OrderStatus.Ordering || order.TicketId != ticket.Id)
            return BadRequest();
        _repository.ConfirmOrder(order);
        _cache.RemoveAsync("CO"+ticket.TrainNumber+"_"+ticket.WagonNumber);
        _cache.RemoveAsync("O" + ticket.TrainNumber + "_" + ticket.WagonNumber);
        return Ok();
    }
}
