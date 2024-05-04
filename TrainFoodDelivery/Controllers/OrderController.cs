using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TrainFoodDelivery.Controllers.Utils;
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
            return Forbid();

        var key = "P" + ticket.TrainNumber + "_" + ticket.WagonNumber;
        var json = await _cache.GetStringAsync(key);
        if (json is null)
        {
            var orders = _repository.GetProducts(ticket.TrainNumber);
            _cache.SetStringAsync(key, JsonSerializer.Serialize(orders));
            return Ok(orders);
        }
        return Ok(json);
    }

    // GET api/<OrderControllerController>/5
    [HttpGet]
    public async Task<IActionResult> Product(int id)
    {
        if(id == 0)
            return BadRequest();
        var key = "p" + id;
        var json = await _cache.GetStringAsync(key);
        if (json is null)
        {
            var product = _repository.GetProduct(id);
            _cache.SetStringAsync(key, JsonSerializer.Serialize(product));
            return Ok(product);
        }
        return Ok(json);

    }

    // POST api/<OrderControllerController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<OrderControllerController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<OrderControllerController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
