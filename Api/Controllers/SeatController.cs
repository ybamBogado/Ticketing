using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly IGetSeatStatusQueryHandler _getSeatStatusQueryHandler;
        public SeatController(IGetSeatStatusQueryHandler getSeatStatusQueryHandler)
        {
            _getSeatStatusQueryHandler = getSeatStatusQueryHandler;
        }
        [HttpGet("{eventId}")] 
        public async Task<ActionResult<IEnumerable<SeatStatusDto>>> GetSeatsByEvent(int eventId)
        {
            var query = new GetSeatStatusQuery { EventId = eventId }; 
            var result = await _getSeatStatusQueryHandler.HandlerAsync(query); 

            return Ok(result); 
        }
    }
}
