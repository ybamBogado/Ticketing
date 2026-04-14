using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class USER
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // Relaciones
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>(); [cite: 135]
        public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>(); [cite: 144]
    }
}
