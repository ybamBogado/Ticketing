using Application.Commands;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Factories;

namespace Application.Handlers
{
    public class CancelReservationCommandHandler : ICancelReservationCommandHandler
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelReservationCommandHandler(
            IReservationRepository reservationRepository,
            ISeatRepository seatRepository,
            IAuditLogRepository auditLogRepository,
            IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _seatRepository = seatRepository;
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> HandlerAsync(CancelReservationCommand request)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(request.ReservationId);
            if (reservation == null) return false;
            
            var seat = await _seatRepository.GetSeatByIdAsync(reservation.SeatId);
            if (seat != null)
            {
                seat.Status = "Available";
                seat.Version++;
            }
            
            var auditLog = AuditLogFactory.CreateForManualCancellation(request.UserId, request.ReservationId);
            await _auditLogRepository.AddAuditLogAsync(auditLog);

            await _reservationRepository.DeleteReservationAsync(reservation);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}