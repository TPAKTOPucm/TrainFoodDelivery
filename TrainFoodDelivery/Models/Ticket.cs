namespace TrainFoodDelivery.Models;

public class Ticket
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public int TrainNumber { get; set; }
    public Train Train { get; set; }
    public int WagonNumber { get; set; }
    public int SeatNumber { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
}
