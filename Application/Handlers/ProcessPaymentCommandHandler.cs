using Application.Commands;
using Application.Interfaces;
using Domain.Entities;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class ProcessPaymentCommandHandler : IProcessPaymentCommandHandler
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProcessPaymentCommandHandler(IReservationRepository reservationRepository, ISeatRepository seatRepository, IAuditLogRepository auditLogRepository, IUnitOfWork unitOfWork)
        {
            _seatRepository = seatRepository;
            _reservationRepository = reservationRepository;
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> HandlerAsync(ProcessPaymentCommand command)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var reservation = await _reservationRepository.GetReservationByIdAsync(command.ReservationId);
                if (reservation == null || reservation.Status != "Reserved")
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }
                reservation.Status = "Paid";

                bool paymentSuccess = await SimulatePaymentGatewayAsync(command.CardNumber);

                if (!paymentSuccess)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                var seat = await _seatRepository.GetSeatByIdAsync(reservation.SeatId);
                if (seat != null)
                {
                    seat.Status = "Sold"; 
                }
                reservation.Status = "Completed";

                var auditEntry = new AuditLog
                {
                    Id = Guid.NewGuid(),
                    UserId = command.UserId,
                    Action = "ProcessPayment",
                    EntityType = "Reservation",
                    EntityId = reservation.Id.ToString(),
                    Details = $"Pago procesado para la reserva ID: {reservation.Id}. El estado pasó a Paid.",
                    CreatedAt = DateTime.UtcNow
                };
                
                await _auditLogRepository.AddAuditLogAsync(auditEntry);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
        }
        
        private async Task<bool> SimulatePaymentGatewayAsync(string cardNumber)
        {
            await Task.Delay(500);
            return !string.IsNullOrEmpty(cardNumber);
        }
    }
}