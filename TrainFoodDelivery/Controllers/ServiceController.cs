using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using TrainFoodDelivery.Controllers.Utils;
using TrainFoodDelivery.DTOs;
using TrainFoodDelivery.Models;
using TrainFoodDelivery.Repository;

namespace TrainFoodDelivery.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class ServiceController : ControllerBase
{
    private readonly ITrainRepository _trainRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ControllerUtils _utils;

    public ServiceController(ITrainRepository trainRepository, ITicketRepository ticketRepository, IOrderRepository orderRepository, ControllerUtils utils)
    {
        _orderRepository = orderRepository;
        _utils = utils;
        _ticketRepository = ticketRepository;
        _trainRepository = trainRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetRole(string jwt, int ticketIndex)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, null);
        if (ticket == null) 
            return NotFound();
        return Ok(ticket.Role.ToString());
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(string jwt, int ticketIndex, int? wagonNumber=null)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, null);
        if (ticket is null)
            return Forbid();
        return Ok(await _orderRepository.GetProducts(ticket.TrainNumber, wagonNumber));
    }

    [HttpPost]
    public async Task<IActionResult> MoveProduct(string jwt, int ticketIndex, int wagonNumberFrom, int wagonNumberTo, int productId, int amount)
    {
        if(amount<0) return Forbid();
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Deliverer);
        if (ticket is null)
            return Forbid();
        await _orderRepository.AddProductToWagon(productId, ticket.TrainNumber, wagonNumberFrom, -amount);
        await _orderRepository.AddProductToWagon(productId, ticket.TrainNumber, wagonNumberTo, amount);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(string jwt, int ticketIndex, int wagonNumber, int productId, int amount)
    {
        var ticket = await _utils.CheckIfAlowed(jwt,ticketIndex, UserRole.Admin);
        if (ticket is null)
            return Forbid();
        await _orderRepository.AddProductToWagon(productId, ticket.TrainNumber, wagonNumber, amount);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetTrains(string jwt, int ticketIndex)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Admin);
        if (ticket is null)
            return Forbid();
        return Ok(await _trainRepository.GetTrains());
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTrain(string jwt, int ticketIndex, [FromBody] TrainDto train)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Admin);
        if (ticket is null)
            return Forbid();
        await _trainRepository.UpdateTrain(train);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> AddTrain(string jwt, int ticketIndex, [FromBody] TrainDto train)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Admin);
        if (ticket is null)
            return Forbid();
        await _trainRepository.AddTrain(train);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTrain(string jwt, int ticketIndex, int id)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Admin);
        if (ticket is null)
            return Forbid();
        return Ok(await _trainRepository.DeleteTrain(id));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTicket(string jwt, int ticketIndex, [FromBody] TicketDto ticket)
    {
        var ticket1 = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Admin);
        if (ticket1 is null)
            return Forbid();
        await _ticketRepository.UpdateTicket(ticket);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket(string jwt, int ticketIndex, [FromBody] TicketDto ticket)
    {
        var ticket1 = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Admin);
        if (ticket1 is null)
            return Forbid();
        await _ticketRepository.CreateTicket(ticket);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetTrainInfo(string jwt, int ticketIndex)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Admin);
        if (ticket is null)
            return Forbid();
        var train = await _trainRepository.GetTrain(ticket.TrainNumber);
        return Ok(train);
    }
}
