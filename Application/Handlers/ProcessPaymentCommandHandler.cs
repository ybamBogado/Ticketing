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
            try
            {
                var reservation = await _reservationRepository.GetReservationByIdAsync(command.ReservationId);
                if (reservation == null || reservation.Status != "Reserved" || reservation.ExpiresAt <= DateTime.UtcNow) {return false;}

                bool paymentSuccess = await SimulatePaymentGatewayAsync(command.CardNumber);
                if (!paymentSuccess) {return false;}
                
                await _unitOfWork.BeginTransactionAsync();

                reservation = await _reservationRepository.GetReservationByIdAsync(command.ReservationId);
                if (reservation == null || reservation.Status != "Reserved" || reservation.ExpiresAt <= DateTime.UtcNow)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                var seat = await _seatRepository.GetSeatByIdAsync(reservation.SeatId);
                if (seat != null) {seat.Status = "Sold"; }
                reservation.Status = "Completed";

                var auditEntry = Domain.Factories.AuditLogFactory.CreateForPaymentProcessed(command.UserId, reservation.Id);
                await _auditLogRepository.AddAuditLogAsync(auditEntry);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                try { await _unitOfWork.RollbackTransactionAsync(); } catch { }
                return false;
            }
        }
        
        private async Task<bool> SimulatePaymentGatewayAsync(string cardNumber)
        {
            await Task.Delay(8000);
            return !string.IsNullOrEmpty(cardNumber);
        }
    }
}