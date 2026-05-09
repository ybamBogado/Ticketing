using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.BackgroundJobs
{
    public class ReservationCleanupWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReservationCleanupWorker> _logger;

        public ReservationCleanupWorker(IServiceProvider serviceProvider, ILogger<ReservationCleanupWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ReservationCleanupWorker ha iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessExpiredReservationsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ocurrió un error al procesar las reservas expiradas.");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

            _logger.LogInformation("ReservationCleanupWorker se ha detenido.");
        }

        private async Task ProcessExpiredReservationsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            
            var reservationRepo = scope.ServiceProvider.GetRequiredService<IReservationRepository>();
            var auditRepo = scope.ServiceProvider.GetRequiredService<IAuditLogRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var currentTime = DateTime.UtcNow;
            
            var expiredReservations = await reservationRepo.GetExpiredReservationsAsync(currentTime);

            bool hasChanges = false;

            foreach (var reservation in expiredReservations)
            {
                _logger.LogInformation("Cancelando reserva expirada ID: {ReservationId}", reservation.Id);

                await unitOfWork.BeginTransactionAsync();
                try
                {
                    reservation.Status = "Expiro";

                    if (reservation.Seat != null)
                    {
                        reservation.Seat.Status = "Available";
                    }

                    var auditLog = Domain.Factories.AuditLogFactory.CreateForAutoCancellation(reservation.UserId, reservation.Id);

                    await auditRepo.AddAuditLogAsync(auditLog);
                    await unitOfWork.SaveChangesAsync();
                    await unitOfWork.CommitTransactionAsync();
                    hasChanges = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al cancelar la reserva {ReservationId}", reservation.Id);
                    await unitOfWork.RollbackTransactionAsync();
                }
            }

            if (hasChanges)
            {
                _logger.LogInformation("Se han liberado reservas expiradas.");
            }
        }
    }
}
