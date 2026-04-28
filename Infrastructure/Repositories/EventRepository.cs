using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddEventAsync(Event eventt)
        {
            await _context.Events.AddAsync(eventt);
        }

        public async Task<IEnumerable<Event>> GetActiveEventsWithSectorsAsync()
        {
            return await _context.Events
                .Include(e => e.Sectors)
                .Where(e => e.Status == "Active")
                .ToListAsync();
        }
    }
}
