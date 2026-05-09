using System;

namespace Application.Commands
{
    public class CreateAuditLogCommand
    {
        public int? UserId { get; set; }
        public Guid SeatId { get; set; }
    }
}
