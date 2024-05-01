namespace TrainFoodDelivery.DTOs;

public class TicketDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public Models.User User { get; set; }
    public int TrainNumber { get; set; }
    public int WagonNumber { get; set; }
    public int SeatNumber { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
}
