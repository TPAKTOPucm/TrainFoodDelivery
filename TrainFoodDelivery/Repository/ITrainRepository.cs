using TrainFoodDelivery.DTOs;

namespace TrainFoodDelivery.Repository;

public interface ITrainRepository
{
    public Task<List<TrainDto>> GetTrains();
    public Task AddTrain(TrainDto train);
    public Task UpdateTrain(TrainDto train);
    public Task<TrainDto> DeleteTrain(int id);
    public Task<TrainDto> GetTrain(int id);
}
