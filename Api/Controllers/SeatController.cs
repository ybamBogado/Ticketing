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

        /// <summary>
        /// Obtiene el estado de todos los asientos correspondientes a un evento específico.
        /// </summary>
        /// <param name="eventId">El identificador numérico del evento a consultar.</param>
        /// <returns>Una lista de asientos y su estado de disponibilidad actual.</returns>
        /// <response code="200">Retorna la lista de los estados de los asientos exitosamente.</response>
        [HttpGet("{eventId}")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SeatStatusDto>>> GetSeatsByEvent(int eventId)
        {
            var query = new GetSeatStatusQuery { EventId = eventId }; 
            var result = await _getSeatStatusQueryHandler.HandlerAsync(query); 

            return Ok(result); 
        }

        /// <summary>
        /// Reserva un asiento específico para un usuario.
        /// </summary>
        /// <param name="command">Los datos de reserva que contienen el ID de la butaca y del usuario.</param>
        /// <returns>Un mensaje de confirmación de reserva.</returns>
        /// <response code="200">El asiento fue reservado exitosamente.</response>
        /// <response code="400">El asiento no se pudo reservar (podría estar ocupado o no existir).</response>
        [HttpPost("reserve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult>ReserveSeat([FromBody] ReserveSeatCommand command)
        {
            var result= await _reserveSeatCommandHandler.HandlerAsync(command);
            if (!result) return BadRequest("No se pudo reservar la butaca.");
        
            return Ok("Reserva completada con éxito.");
        }
    }
}
