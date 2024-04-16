namespace TrainFoodDelivery.Models;

public class Ticket
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int TrainId { get; set; }
    public Train Train { get; set; }
    public int WagonId { get; set; }
    public Wagon Wagon { get; set; }
    public int SeatNumber { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
}
