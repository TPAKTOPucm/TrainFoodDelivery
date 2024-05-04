﻿using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainFoodDelivery.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{

    // GET api/<OrderControllerController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value" + id;
    }

    // POST api/<OrderControllerController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<OrderControllerController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<OrderControllerController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
