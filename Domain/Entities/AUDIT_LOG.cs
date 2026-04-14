using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AUDIT_LOG
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string Details { get; set; }
        public datetime CreatedAt { get; set; }

        // Relación opcional
        public virtual User? User { get; set; } [cite: 168]
    }
}
