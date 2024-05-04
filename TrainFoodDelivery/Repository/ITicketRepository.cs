using TrainFoodDelivery.DTOs;
using TrainFoodDelivery.Models;

namespace TrainFoodDelivery.Repository;

public interface ITicketRepository
{
    public Task<TicketDto> GetTicket(string userId, int ticketNumber);
    public Task UpdateTicket(TicketDto ticket);
    public Task CreateTicket(TicketDto ticket);
}
