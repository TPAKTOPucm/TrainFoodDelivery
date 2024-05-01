using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
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
    public ReadyToDeliverController(IDistributedCache cache, IOrderRepository repository)
    {
        _repository = repository;
        _cache = cache;
    }
    // GET: api/<ReadyToDeliverController>
    [HttpGet]
    public async Task<IActionResult> Orders(int trainNumber,int wagonNumber)
    {
        var key = "O" + trainNumber + "_" + wagonNumber;
        var json = await _cache.GetStringAsync(key);
        if (json is null)
        {
            var orders = _repository.GetOrders(trainNumber, wagonNumber);
            _cache.SetStringAsync(key, JsonSerializer.Serialize(orders));
            return Ok(orders);
        }
        return Ok(json);
    }

    // GET api/<ReadyToDeliverController>/5
    [HttpGet]
    public async Task<IActionResult> Order(int id)
    {
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
    public async Task<IActionResult> UpdateStatus(int orderId, OrderStatus status)
    {
        var order = await _repository.GetOrder(orderId);
        order.Status = status;
        await _repository.UpdateOrder(order);
        await _cache.RemoveAsync("o" + orderId);
        return Ok();
    }

    // PUT api/<ReadyToDeliverController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ReadyToDeliverController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        
    }

    private async Task<bool> CheckIfAlowed()
    {
        var jwt = Request.Cookies.Where(c => c.Key == "token").FirstOrDefault().Value;
        var userId = "ss";
        if (await _cache.GetStringAsync(userId) == jwt)
        {
            return true;
        }
        using HttpClient client = new HttpClient();
        var baseUri = "http://localhost:5280/account/check";
        var response = await client.PostAsync(baseUri, new  StringContent(jwt));
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            _cache.SetStringAsync(userId, jwt);
            return true;
        }
        return false;

    }
}
