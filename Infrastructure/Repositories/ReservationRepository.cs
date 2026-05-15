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
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;

        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
        }

        public async Task<Reservation?> GetReservationByIdAsync(Guid id)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Reservation>> GetExpiredReservationsAsync(DateTime currentUtcTime)
        {
            return await _context.Reservations
                .Include(r => r.Seat)
                .Where(r => r.Status == "Reserved" && r.ExpiresAt <= currentUtcTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsBySeatIdAsync(Guid seatId)
        {
            return await _context.Reservations
                .Where(r => r.SeatId == seatId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId)
        {
            return await _context.Reservations
                .Include(r => r.Seat)
                    .ThenInclude(s => s.Sector)
                        .ThenInclude(sc => sc.Event)
                .Where(r => r.UserId == userId && r.Status == "Completed")
                .OrderByDescending(r => r.ReservedAt)
                .ToListAsync();
        }

        public async Task DeleteReservationAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            await Task.CompletedTask;
        }

        public async Task<int> GetActiveReservationsCountAsync(int userId)
        {
            return await _context.Reservations
                .CountAsync(r => r.UserId == userId && r.Status == "Reserved" && r.ExpiresAt > DateTime.UtcNow);
        }

        public async Task<bool> HasRecentReservationAsync(int userId, Guid seatId, DateTime since)
        {
            return await _context.Reservations
                .AnyAsync(r => r.UserId == userId && r.SeatId == seatId && (r.Status == "Expired" || r.ExpiresAt <= DateTime.UtcNow) && r.ExpiresAt >= since);
        }
    }
}