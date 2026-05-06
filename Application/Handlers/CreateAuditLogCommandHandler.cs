using Application.Commands;
using Application.Interfaces;
using Domain.Entities;
using Application.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class CreateAuditLogCommandHandler : ICreateAuditLogCommandHandler
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAuditLogCommandHandler(IAuditLogRepository auditLogRepository, IUnitOfWork unitOfWork)
        {
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(CreateAuditLogCommand request)
        {
            // Limpiamos el DbContext para evitar que intente guardar cambios fallidos de una transacción anterior (ej. DbUpdateConcurrencyException)
            _unitOfWork.Clear();

            var auditEntry = new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Action = "ReserveSeat",
                EntityType = "Seat",
                EntityId = request.SeatId.ToString(),
                Details = $"Reserva fallida por conflicto de concurrencia optimista para la butaca ID: {request.SeatId}.",
                CreatedAt = DateTime.UtcNow
            };
            
            await _auditLogRepository.AddAuditLogAsync(auditEntry);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
