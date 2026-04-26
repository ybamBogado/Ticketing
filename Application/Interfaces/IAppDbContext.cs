using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Event> Events { get; set; }
        DbSet<Seat> Seats { get; set; }
        DbSet<AuditLog> AuditLogs { get; set; }
        DbSet<Reservation> Reservations { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
