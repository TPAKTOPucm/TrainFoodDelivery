using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using TrainFoodDelivery.Controllers.Utils;
using TrainFoodDelivery.DTOs;
using TrainFoodDelivery.Models;
using TrainFoodDelivery.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainFoodDelivery.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class ReadyToDeliverController : ControllerBase
{
    private readonly IDistributedCache _cache;
    private readonly IOrderRepository _repository;
    private readonly ControllerUtils _utils;
    public ReadyToDeliverController(IDistributedCache cache, IOrderRepository repository, ControllerUtils utils)
    {
        _repository = repository;
        _cache = cache;
        _utils = utils;
    }
    // GET: api/<ReadyToDeliverController>
    [HttpGet]
    public async Task<IActionResult> Orders(string jwt, int ticketIndex)
    {
        var ticket = await _utils.CheckIfAlowed(jwt,0,UserRole.Deliverer);
        if (ticket is null)
            return Forbid();

        var key = "O" + ticket.TrainNumber + "_" + ticket.WagonNumber;
        var json = await _cache.GetStringAsync(key);
        if (json is null)
        {
            var orders = _repository.GetOrders(ticket.TrainNumber, ticket.WagonNumber);
            _cache.SetStringAsync(key, JsonSerializer.Serialize(orders));
            return Ok(orders);
        }
        return Ok(json);
    }

    // GET api/<ReadyToDeliverController>/5
    [HttpGet]
    public async Task<IActionResult> Order(string jwt,int id)
    {
        if(await _utils.CheckIfAlowed(jwt,0,UserRole.Deliverer) is null)
            return Forbid();
        var key = "o" + id;
        var json = await _cache.GetStringAsync(key);
        if (json is null)
        {
            var order = _repository.GetOrder(id);
            _cache.SetStringAsync(key, JsonSerializer.Serialize(order));
            return Ok(order);
        }
        return Ok(json);
    }

    // POST api/<ReadyToDeliverController>
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(string jwt, int orderId, OrderStatus status)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, 0, UserRole.Deliverer);
        if (ticket is null)
            return Forbid();
        var order = await _repository.GetOrder(orderId);
        order.Status = status;
        await _repository.UpdateOrder(order);
        await _cache.RemoveAsync("o" + orderId);
        await _cache.RemoveAsync("O"+ticket.TrainNumber+"_"+ticket.WagonNumber);
        return Ok();
    }

    // PUT api/<ReadyToDeliverController>/5
    [HttpPut("{id}")]
    public string Put(int id, [FromBody] string value)
    {
        return id + value;
    }

    // DELETE api/<ReadyToDeliverController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {

    }
}
