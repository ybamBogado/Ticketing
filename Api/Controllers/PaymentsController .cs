[HttpPost("{reservationId}/pay")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<IActionResult> ProcessPayment(Guid reservationId, [FromBody] ProcessPaymentCommand command)
{
    // Aseguramos que el ID de la ruta coincida con el comando
    command.ReservationId = reservationId;

    try
    {
        var result = await _processPaymentCommandHandler.HandleAsync(command);
        
        if (!result) 
            return BadRequest("El pago fue rechazado o la reserva no es válida.");

        return Ok("Pago procesado con éxito. Asiento vendido.");
    }
    catch (Exception ex)
    {
        // El rollback ya se ejecutó en el Handler, acá solo devolvemos el error 500
        return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al procesar el pago.");
    }
}
