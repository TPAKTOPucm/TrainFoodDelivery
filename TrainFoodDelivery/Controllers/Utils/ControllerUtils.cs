using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using System.Text;
using TrainFoodDelivery.DTOs;
using TrainFoodDelivery.Models;
using TrainFoodDelivery.Repository;

namespace TrainFoodDelivery.Controllers.Utils;

public class ControllerUtils
{
    private readonly IDistributedCache _cache;
    private readonly ITicketRepository _ticketRepository;
    public ControllerUtils(IDistributedCache cache, ITicketRepository ticketRepository)
    {
        _cache = cache;
        _ticketRepository = ticketRepository;
    }
    public async Task<TicketDto> CheckIfAlowed(string jwt, int ticketIndex, UserRole role)
    {
        var decodedJwt = JWTDecoder.Decoder.DecodeToken(jwt);
        Console.WriteLine(decodedJwt.Payload);
        var userId = JObject.Parse(decodedJwt.Payload)["nameid"].ToString();
        if (await _cache.GetStringAsync(userId) != jwt)
        {
            using HttpClient client = new HttpClient();
            var uri = $"http://localhost:5280/account/check/?token={jwt}";
            var response = await client.PostAsync(uri, new StringContent('"' + userId + '"', Encoding.UTF8, "application/json"));
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                return null;
            } else
            {
                _cache.SetStringAsync(userId, jwt);
            }
        }
        var ticket = await _ticketRepository.GetTicket(userId, ticketIndex);
        Console.WriteLine(ticket.Role);
        if (ticket.Role != role)
            return null;
        return ticket;
    }
}
