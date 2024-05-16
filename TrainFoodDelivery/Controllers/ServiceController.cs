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
    private readonly ControllerUtils _utils;
    public ServiceController(ITrainRepository trainRepository, ITicketRepository ticketRepository, IDistributedCache cache, ControllerUtils utils)
    {
        _utils = utils;
        _ticketRepository = ticketRepository;
        _trainRepository = trainRepository;
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
}
