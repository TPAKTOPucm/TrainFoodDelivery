using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainFoodDelivery.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReadyToDeliverController : ControllerBase
{
    private readonly IDistributedCache _cache;
    public ReadyToDeliverController(IDistributedCache cache)
    {
        _cache = cache;
    }
    // GET: api/<ReadyToDeliverController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<ReadyToDeliverController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<ReadyToDeliverController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<ReadyToDeliverController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ReadyToDeliverController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        
    }

    private async Task<bool> CheckIfAlowed()
    {
        var jwt = Request.Cookies.Where(c => c.Key == "token").FirstOrDefault().Value;
        var userId = "ss";
        if (await _cache.GetStringAsync(userId) == jwt)
        {
            return true;
        }
        using HttpClient client = new HttpClient();
        var baseUri = "http://localhost:5280/account/check";
        var response = await client.PostAsync(baseUri, new  StringContent(jwt));
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            _cache.SetStringAsync(userId, jwt);
            return true;
        }
        return false;

    }
}
