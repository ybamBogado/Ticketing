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
    public class GetSeatStatusQueryHandler : IGetSeatStatusQueryHandler
    {
        private readonly IAppDbContext _context ;
        public GetSeatStatusQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SeatStatusDto>> HandlerAsync(GetSeatStatusQuery query)
        {
            var seats = await _context.Seats
                .Include(seat => seat.Sector)
                .Where(seat => seat.Sector.EventId == query.EventId)
                .ToListAsync();
           
            var result = seats.Select(seat => new SeatStatusDto
            {
                Id = seat.Id,
                SectorId = seat.SectorId,
                SeatNumber = seat.SeatNumber,
                RowIdentifier = seat.RowIdentifier,
                Status = seat.Status 
            });
            return result; 
        }
    }
}
