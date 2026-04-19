using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Commands;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly IGetSeatStatusQueryHandler _getSeatStatusQueryHandler;
        private readonly IReserveSeatCommandHandler _reserveSeatCommandHandler;
        public SeatController(IGetSeatStatusQueryHandler getSeatStatusQueryHandler, IReserveSeatCommandHandler reserveSeatCommandHandler)
        {
            _getSeatStatusQueryHandler = getSeatStatusQueryHandler;
            _reserveSeatCommandHandler = reserveSeatCommandHandler;
        }
        [HttpGet("{eventId}")] 
        public async Task<ActionResult<IEnumerable<SeatStatusDto>>> GetSeatsByEvent(int eventId)
        {
            var query = new GetSeatStatusQuery { EventId = eventId }; 
            var result = await _getSeatStatusQueryHandler.HandlerAsync(query); 

            return Ok(result); 
        }
        [HttpPost("reserve")]
        public async Task<IActionResult>ReserveSeat([FromBody] ReserveSeatCommand command)
        {
            var result= await _reserveSeatCommandHandler.HandlerAsync(command);
            if (!result) return BadRequest("No se pudo reservar la butaca.");
        
            return Ok("Reserva completada con éxito.");
        }
    }
}
