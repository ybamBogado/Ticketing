using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Queries;

namespace Application.Handlers
{
    public class GetUserTicketsQueryHandler : IGetUserTicketsQueryHandler
    {
        private readonly IReservationRepository _reservationRepository;

        public GetUserTicketsQueryHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<IEnumerable<UserTicketDto>> HandlerAsync(GetUserTicketsQuery query)
        {
            var reservations = await _reservationRepository.GetReservationsByUserIdAsync(query.UserId);

            var result = reservations.Select(r => new UserTicketDto
            {
                ReservationId = r.Id,
                EventName = r.Seat.Sector.Event.Name,
                Venue = r.Seat.Sector.Event.Venue,
                EventDate = r.Seat.Sector.Event.EventDate,
                SectorName = r.Seat.Sector.Name,
                RowIdentifier = r.Seat.RowIdentifier,
                SeatNumber = r.Seat.SeatNumber,
                Status = r.Status,
                Price = r.Seat.Sector.Price
            });

            return result;
        }
    }
}
