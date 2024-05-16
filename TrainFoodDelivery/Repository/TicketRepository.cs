using Microsoft.EntityFrameworkCore;
using TrainFoodDelivery.Data;
using TrainFoodDelivery.DTOs;
using TrainFoodDelivery.Models;

namespace TrainFoodDelivery.Repository;

public class TicketRepository : ITicketRepository
{
    private readonly TrainFoodDeliveryContext _db;
    public TicketRepository(TrainFoodDeliveryContext db)
    {
        _db = db;
    }
    public Task CreateTicket(TicketDto ticket)
    {
        _db.Add(new Ticket
        {
            TrainNumber = ticket.TrainNumber,
            WagonNumber = ticket.WagonNumber,
            SeatNumber = ticket.SeatNumber,
            UserId = ticket.User.Id,
            Role = ticket.Role,
            ArrivalTime = ticket.ArrivalTime,
            DepartureTime = ticket.DepartureTime
        });
        return _db.SaveChangesAsync();
    }

    public Task<TicketDto> GetTicket(string userId, int ticketNumber)
    {
        return _db.Tickets.Where(t => t.UserId == userId && t.ArrivalTime > DateTime.Now).OrderBy(t => t.ArrivalTime).Skip(ticketNumber).Select(t => new TicketDto
        {
            Id = t.Id,
            SeatNumber = t.SeatNumber,
            Role = t.Role,
            TrainNumber = t.TrainNumber,
            WagonNumber = t.WagonNumber,
            ArrivalTime = t.ArrivalTime,
            DepartureTime = t.DepartureTime,
            UserId = t.UserId
        }).FirstOrDefaultAsync();
    }

    public Task UpdateTicket(TicketDto ticket)
    {
        throw new NotImplementedException();
    }
}
