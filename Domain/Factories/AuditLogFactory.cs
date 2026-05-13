using System;
using Domain.Entities;

namespace Domain.Factories
{
    public static class AuditLogFactory
    {
        public static AuditLog CreateForSeatReservation(int userId, Guid seatId)
        {
            return new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Action = "Reserve Seat",
                EntityType = "Seat",
                EntityId = seatId.ToString(),
                Details = $"Reserva tentativa para la butaca ID: {seatId}. El estado pasó a Reserved.",
                CreatedAt = DateTime.UtcNow
            };
        }

        public static AuditLog CreateForPaymentProcessed(int userId, Guid reservationId)
        {
            return new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Action = "Process Payment",
                EntityType = "Reservation",
                EntityId = reservationId.ToString(),
                Details = $"Pago procesado para la reserva ID: {reservationId}. El estado pasó a Paid.",
                CreatedAt = DateTime.UtcNow
            };
        }

        public static AuditLog CreateForAutoCancellation(int? userId, Guid reservationId)
        {
            return new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Action = "System Auto Cancellation",
                EntityType = "Reservation",
                EntityId = reservationId.ToString(),
                Details = $"El sistema anuló automáticamente la reserva porque expiró el tiempo límite (5 min). Butaca liberada.",
                CreatedAt = DateTime.UtcNow
            };
        }

        public static AuditLog CreateForConcurrencyConflict(int userId, Guid seatId)
        {
            return new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Action = "Concurrency Conflict",
                EntityType = "Seat",
                EntityId = seatId.ToString(),
                Details = $"Reserva fallida por conflicto de concurrencia optimista para la butaca ID: {seatId}.",
                CreatedAt = DateTime.UtcNow
            };
        }

        public static AuditLog CreateForManualCancellation(int userId, Guid reservationId)
        {
            return new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Action = "Manual Cancellation",
                EntityType = "Reservation",
                EntityId = reservationId.ToString(),
                Details = $"Reserva cancelada manualmente por el usuario {userId}.",
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
