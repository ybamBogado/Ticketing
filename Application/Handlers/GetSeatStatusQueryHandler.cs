using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers
{
    public class GetSeatStatusQueryHandler : IGetSeatStatusQueryHandler
    {
        private readonly ISeatRepository _seatRepository;

        public GetSeatStatusQueryHandler(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<IEnumerable<SeatStatusDto>> HandlerAsync(GetSeatStatusQuery query)
        {
            var seats = await _seatRepository.GetSeatsByEventIdAsync(query.EventId);

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
