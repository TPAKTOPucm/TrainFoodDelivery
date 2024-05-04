using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
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
        var userId = JObject.Parse(decodedJwt.Payload)["userid"].ToString();
        if (await _cache.GetStringAsync(userId) != jwt)
        {
            using HttpClient client = new HttpClient();
            var baseUri = "http://localhost:5280/account/check";
            var response = await client.PostAsync(baseUri, new StringContent(jwt));
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            } else
            {
                _cache.SetStringAsync(userId, jwt);
            }
        }
        var ticket = await _ticketRepository.GetTicket(userId, 0);
        if (ticket.Role != role)
            return null;
        return ticket;
    }
}
