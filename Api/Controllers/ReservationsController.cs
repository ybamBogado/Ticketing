using Application.Commands;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/v1/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReserveSeatCommandHandler _reserveSeatCommandHandler;
        private readonly ICreateAuditLogCommandHandler _createAuditLogCommandHandler;

        public ReservationsController(
            IReserveSeatCommandHandler reserveSeatCommandHandler,
            ICreateAuditLogCommandHandler createAuditLogCommandHandler)
        {
            _reserveSeatCommandHandler = reserveSeatCommandHandler;
            _createAuditLogCommandHandler = createAuditLogCommandHandler;
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
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ReserveSeat([FromBody] ReserveSeatCommand command)
        {
            try
            {
                var result = await _reserveSeatCommandHandler.HandlerAsync(command);
                if (!result.Success) return BadRequest("No se pudo reservar la butaca.");
            
                return StatusCode(StatusCodes.Status201Created, new { message = "Reserva completada con éxito.", reservationId = result.ReservationId });
            }
            catch (DbUpdateConcurrencyException)
            {
                await _createAuditLogCommandHandler.HandleAsync(new CreateAuditLogCommand
                {
                    UserId = command.UserId,
                    SeatId = command.SeatId
                });

                return Conflict("La butaca acaba de ser reservada por otro usuario. Por favor, seleccione otra.");
            }
        }
    }
}
