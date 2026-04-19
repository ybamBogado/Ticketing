using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IGetEventCatalogQueryHandler _getEventCatalogQueryHandler;
        private readonly ICreateEventCommandHandler _createEventCommandHandler;

        public EventController(IGetEventCatalogQueryHandler getEventCatalogQueryHandler, ICreateEventCommandHandler createEventCommandHandler)
        {
            _getEventCatalogQueryHandler = getEventCatalogQueryHandler;
            _createEventCommandHandler = createEventCommandHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventCatalogDto>>> GetCatalog()
        {
            var result= await _getEventCatalogQueryHandler.HandlerAsync(new GetEventCatalogQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventCommand request)
        {
            var id = await _createEventCommandHandler.HandlerAsync(request);
            return Ok(id);
        }

    }
}
