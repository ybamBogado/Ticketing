using Application.Commands;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IGetEventCatalogQueryHandler _getEventCatalogQueryHandler;
        private readonly ICreateEventCommandHandler _createEventCommandHandler;

        public EventsController(IGetEventCatalogQueryHandler getEventCatalogQueryHandler, ICreateEventCommandHandler createEventCommandHandler)
        {
            _getEventCatalogQueryHandler = getEventCatalogQueryHandler;
            _createEventCommandHandler = createEventCommandHandler;
        }

        /// <summary>
        /// Obtiene el catálogo completo de todos los eventos disponibles.
        /// </summary>
        /// <returns>Una lista de eventos resumidos en formato DTO.</returns>
        /// <response code="200">Retorna la lista de eventos exitosamente.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EventCatalogDto>>> GetCatalog()
        {
            var result= await _getEventCatalogQueryHandler.HandlerAsync(new GetEventCatalogQuery());
            
            return Ok(result);
        }

        /// <summary>
        /// Crea un nuevo evento en el sistema.
        /// </summary>
        /// <param name="request">Los datos del evento a crear (nombre, fecha, lugar).</param>
        /// <returns>El identificador único (ID) del evento creado.</returns>
        /// <response code="201">El evento fue creado con éxito y devuelve su ID.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateEventCommand request)
        {
            var id = await _createEventCommandHandler.HandlerAsync(request);
            
            return StatusCode(201, id);

        }

    }
}
