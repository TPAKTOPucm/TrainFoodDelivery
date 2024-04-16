using System.ComponentModel.DataAnnotations;

namespace TrainFoodDelivery.Models;

public class Train
{
    [Key]
    public int TrainNumber { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int RouteId { get; set; }
    public List<Wagon> Wagons { get; set; }
}
