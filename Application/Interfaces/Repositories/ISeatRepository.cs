using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetSeatsByEventIdAsync(int eventId);
        Task<Seat?> GetSeatByIdAsync(Guid seatId);
    }
}