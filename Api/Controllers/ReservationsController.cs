using Application.Commands;
using Application.Interfaces;
using Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;

namespace Api.Controllers
{
    [Route("api/v1/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReserveSeatCommandHandler _reserveSeatCommandHandler;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReservationRepository _reservationRepository;
        private readonly ICancelReservationCommandHandler _cancelReservationCommandHandler;
        private readonly IGetUserTicketsQueryHandler _getUserTicketsQueryHandler;

        public ReservationsController(IReserveSeatCommandHandler reserveSeatCommandHandler, IAuditLogRepository auditLogRepository, IUnitOfWork unitOfWork, IReservationRepository reservationRepository, ICancelReservationCommandHandler cancelReservationCommandHandler, IGetUserTicketsQueryHandler getUserTicketsQueryHandler)
        {
            _reserveSeatCommandHandler = reserveSeatCommandHandler;
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
            _reservationRepository = reservationRepository;
            _cancelReservationCommandHandler = cancelReservationCommandHandler;
            _getUserTicketsQueryHandler = getUserTicketsQueryHandler;
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
                if (!result.Success) return BadRequest(result.ErrorMessage ?? "No se pudo reservar la butaca.");
            
                return StatusCode(StatusCodes.Status201Created, new { message = "Reserva completada con éxito.", reservationId = result.ReservationId, expiresAt = result.ExpiresAt });
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException || ex is DbUpdateException)
            {
                _unitOfWork.Clear();
                
                var auditEntry = Domain.Factories.AuditLogFactory.CreateForConcurrencyConflict(command.UserId, command.SeatId);
                await _auditLogRepository.AddAuditLogAsync(auditEntry);
                await _unitOfWork.SaveChangesAsync();

                return Conflict("La butaca acaba de ser reservada por otro usuario. Por favor, seleccione otra.");
            }
        }

        /// <summary>
        /// Cancela una reserva existente.
        /// </summary>
        /// <param name="command">Los datos de reserva que contienen el ID de la reserva.</param>
        /// <returns>Un mensaje de confirmación de cancelación.</returns>
        /// <response code="200">La reserva fue cancelada exitosamente.</response>
        /// <response code="400">La reserva no se pudo cancelar (podría estar expirada o no existir).</response>
        /// <response code="404">La reserva no existe.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelReservation([FromBody] CancelReservationCommand command)
        {
            try
            {
                var result = await _cancelReservationCommandHandler.HandlerAsync(command);
                if (!result) return BadRequest("No se pudo cancelar la reserva.");
                return Ok(new { message = "Reserva cancelada con éxito." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene todas las entradas compradas por un usuario específico.
        /// </summary>
        /// <param name="userId">El ID del usuario.</param>
        /// <returns>Una lista de reservas con estado 'Sold' incluyendo detalles del evento.</returns>
        /// <response code="200">Retorna la lista de entradas del usuario.</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserTickets(int userId)
        {
            var query = new GetUserTicketsQuery { UserId = userId };
            var result = await _getUserTicketsQueryHandler.HandlerAsync(query);
            return Ok(result);
        }
    }
}