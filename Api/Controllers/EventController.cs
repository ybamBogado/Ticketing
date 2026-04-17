using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IGetEventCatalogQuery _getEventCatalogQuery;
        private readonly ICreateEventCommand _createEventCommand;

        public EventController(IGetEventCatalogQuery getEventCatalogQuery, ICreateEventCommand createEventCommand)
        {
            _getEventCatalogQuery = getEventCatalogQuery;
            _createEventCommand = createEventCommand;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventCatalogDto>>> GetCatalog()
        {
            var result= await _getEventCatalogQuery.ExecuteAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
        {
            var id = await _createEventCommand.ExecuteAsync(request);
            return Ok(id);
        }

    }
}
