using Application.Commands;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v1/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReserveSeatCommandHandler _reserveSeatCommandHandler;

        public ReservationsController(IReserveSeatCommandHandler reserveSeatCommandHandler)
        {
            _reserveSeatCommandHandler = reserveSeatCommandHandler;
        }

        /// <summary>
        /// Reserva un asiento específico para un usuario.
        /// </summary>
        /// <param name="command">Los datos de reserva que contienen el ID de la butaca y del usuario.</param>
        /// <returns>Un mensaje de confirmación de reserva.</returns>
        /// <response code="201">El asiento fue reservado exitosamente.</response>
        /// <response code="400">El asiento no se pudo reservar (podría estar ocupado o no existir).</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReserveSeat([FromBody] ReserveSeatCommand command)
        {
            var result = await _reserveSeatCommandHandler.HandlerAsync(command);
            if (!result) return BadRequest("No se pudo reservar la butaca.");
        
            return StatusCode(StatusCodes.Status201Created, "Reserva completada con éxito.");
        }
    }
}
