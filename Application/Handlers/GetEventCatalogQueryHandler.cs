using Application.DTOs;
using Application.Queries;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetEventCatalogQueryHandler: IGetEventCatalogQueryHandler
    {
        private readonly IEventRepository _eventRepository;

        public GetEventCatalogQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<EventCatalogDto>> HandlerAsync(GetEventCatalogQuery query)
        {
            var events = await _eventRepository.GetActiveEventsWithSectorsAsync();

            var catalog = events.Select(e => new EventCatalogDto
            {
                Id = e.Id,
                Name = e.Name,
                EventDate = e.EventDate,
                Venue = e.Venue,
                Sectors = e.Sectors.Select(s => new SectorDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Price = s.Price,
                    Capacity = s.Capacity
                }).ToList()
            });
            return catalog;
        }
    }
}
