using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using System.Text.Json;
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
    private readonly ITicketRepository _ticketRepository;
    public ReadyToDeliverController(IDistributedCache cache, IOrderRepository repository, ITicketRepository ticketRepository)
    {
        _repository = repository;
        _cache = cache;
        _ticketRepository = ticketRepository;
    }
    // GET: api/<ReadyToDeliverController>
    [HttpGet]
    public async Task<IActionResult> Orders(string jwt, int ticketIndex)
    {
        var ticket = await CheckIfAlowed(jwt);
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
        if(await CheckIfAlowed(jwt) is null)
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
        if (await CheckIfAlowed(jwt) is null)
            return Forbid();
        var order = await _repository.GetOrder(orderId);
        order.Status = status;
        await _repository.UpdateOrder(order);
        await _cache.RemoveAsync("o" + orderId);
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

    private async Task<TicketDto> CheckIfAlowed(string jwt)
    {
        var decodedJwt = JWTDecoder.Decoder.DecodeToken(jwt);
        var userId = JObject.Parse(decodedJwt.Payload)["userid"].ToString();
        if (await _cache.GetStringAsync(userId) != jwt)
        {
            using HttpClient client = new HttpClient();
            var baseUri = "http://localhost:5280/account/check";
            var response = await client.PostAsync(baseUri, new StringContent(jwt));
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            } else
            {
                _cache.SetStringAsync(userId, jwt);
            }
        }
        var ticket = await _ticketRepository.GetTicket(userId, 0);
        if(ticket.Role != UserRole.Deliverer)
            return null;
        return ticket;
    }
}
