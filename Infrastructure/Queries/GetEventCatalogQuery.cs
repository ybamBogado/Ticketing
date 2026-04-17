using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries
{

    public class GetEventCatalogQuery : IGetEventCatalogQuery
    {
        private readonly AppDbContext _context;

        public GetEventCatalogQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventCatalogDto>> ExecuteAsync()
        {
            // 1. Buscamos en la BD los eventos activos
            var events = await _context.Events
                .Include(e => e.Sectors) // Traemos también sus sectores
                .Where(e => e.Status == "Active")
                .ToListAsync();

            // 2. Mapeamos (transformamos) la Entidad pesada al DTO liviano
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