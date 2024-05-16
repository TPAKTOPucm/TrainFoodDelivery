using Microsoft.EntityFrameworkCore;
using TrainFoodDelivery.Data;
using TrainFoodDelivery.DTOs;
using TrainFoodDelivery.Models;

namespace TrainFoodDelivery.Repository;

public class TrainRepository : ITrainRepository
{
    private readonly TrainFoodDeliveryContext _db;
    public TrainRepository(TrainFoodDeliveryContext db)
    {
        _db = db;
    }
    public Task AddTrain(TrainDto train)
    {
        _db.Add(new Train
        {
            DepartureTime = train.DepartureTime,
            ArrivalTime = train.ArrivalTime,
            Number = train.Number,
            WagonAmount = train.WagonAmount,
            RouteId = train.RouteId
        });
        return _db.SaveChangesAsync();
    }

    public async Task<TrainDto> DeleteTrain(int id)
    {
        var train = await _db.Trains.FindAsync(id);
        _db.Trains.Remove(train);
        await _db.SaveChangesAsync();
        return new TrainDto
        {
            ArrivalTime = train.ArrivalTime,
            DepartureTime = train.DepartureTime,
            Number = train.Number,
            WagonAmount = train.WagonAmount,
            RouteId = train.RouteId
        };
    }

    public Task<List<TrainDto>> GetTrains()
    {
        return _db.Trains.Select(t => new TrainDto
        { 
            ArrivalTime = t.ArrivalTime,
            DepartureTime = t.DepartureTime,
            Number = t.Number,
            WagonAmount = t.WagonAmount,
            RouteId = t.RouteId
        }).ToListAsync();
    }

    public Task UpdateTrain(TrainDto train)
    {
        _db.Update(new Train
        {
            ArrivalTime = train.ArrivalTime,
            DepartureTime = train.DepartureTime,
            Number = train.Number,
            WagonAmount = train.WagonAmount,
            RouteId = train.RouteId
        });
        return _db.SaveChangesAsync();
    }
}
