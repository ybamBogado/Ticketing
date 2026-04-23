using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers
{
    public class GetEventCatalogQueryHandler: IGetEventCatalogQueryHandler
    {
        private readonly IAppDbContext _context;

        public GetEventCatalogQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventCatalogDto>> HandlerAsync(GetEventCatalogQuery query)
        {

            var events = await _context.Events
                .Include(e => e.Sectors)
                .Where(e => e.Status == "Active")
                .ToListAsync();

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
