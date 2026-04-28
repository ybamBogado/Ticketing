using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly AppDbContext _context;

        public SeatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Seat> GetSeatByIdAsync(Guid seatId)
        {
            return await _context.Seats.FindAsync(seatId);
        }

        public async Task<IEnumerable<Seat>> GetSeatsByEventIdAsync(int eventId)
        {
            return await _context.Seats
                .Include(seat => seat.Sector)
                .Where(seat => seat.Sector.EventId == eventId)
                .ToListAsync();
        }
    }
}
