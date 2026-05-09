using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        Task AddReservationAsync(Reservation reservation);
        Task<Reservation?> GetReservationByIdAsync(Guid id);
        Task<IEnumerable<Reservation>> GetExpiredReservationsAsync(DateTime currentUtcTime);
    }
}