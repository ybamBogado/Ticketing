using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RESERVATION
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public Guid SeatId { get; set; }
        public string Status { get; set; }
        public datetime ReserverAt { get; set; }
        public datetime ExpiresAt { get; set; }

        // Relaciones
        public virtual User User { get; set; } = null!; [cite: 161]
        public virtual Seat Seat { get; set; } = null!; [cite: 160]
    }
}
