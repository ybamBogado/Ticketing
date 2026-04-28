using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Commands;

namespace Api.Controllers
{
    [Route("api/v1/events/{eventId}/seats")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly IGetSeatStatusQueryHandler _getSeatStatusQueryHandler;
        
        public SeatsController(IGetSeatStatusQueryHandler getSeatStatusQueryHandler)
        {
            _getSeatStatusQueryHandler = getSeatStatusQueryHandler;
        }

        /// <summary>
        /// Obtiene el estado de todos los asientos correspondientes a un evento específico.
        /// </summary>
        /// <param name="eventId">El identificador numérico del evento a consultar.</param>
        /// <returns>Una lista de asientos y su estado de disponibilidad actual.</returns>
        /// <response code="200">Retorna la lista de los estados de los asientos exitosamente.</response>
        [HttpGet] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SeatStatusDto>>> GetSeatsByEvent(int eventId)
        {
            var query = new GetSeatStatusQuery { EventId = eventId }; 
            var result = await _getSeatStatusQueryHandler.HandlerAsync(query); 
            return Ok(result); 
        }
    }
}
