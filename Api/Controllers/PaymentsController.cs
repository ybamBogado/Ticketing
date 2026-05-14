using Application.Commands;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/v1/reservations/{reservationId}/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IProcessPaymentCommandHandler _processPaymentCommandHandler;

        public PaymentsController(IProcessPaymentCommandHandler processPaymentCommandHandler)
        {
            _processPaymentCommandHandler = processPaymentCommandHandler;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProcessPayment(Guid reservationId, [FromBody] ProcessPaymentCommand command)
        {
            command.ReservationId = reservationId;

            try
            {
                var result = await _processPaymentCommandHandler.HandlerAsync(command);
                
                if (!result) 
                    return BadRequest("El pago fue rechazado o la reserva no es válida.");

                return Ok("Pago procesado con éxito. Asiento vendido.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al procesar el pago.");
            }
        }
    }
}