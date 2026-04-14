using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SEAT
    {
        public Guid Id { get; set; }
        public int SectorId { get; set; }
        public string RowIdentifier { get; set; }
        public int SeatNumber { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public int Version { get; set; }

        [cite_start]
        [ConcurrencyCheck] // .NET 8: Esto ayuda al Optimistic Locking [cite: 122]
        public int Version { get; set; }

        // Relaciones
        public virtual Sector Sector { get; set; } = null!; // "Tiene un" Sector [cite: 109]
        public virtual Reservation? CurrentReservation { get; set; } // Puede tener una reserva activa [cite: 136]
    }
}
