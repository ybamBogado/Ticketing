using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public datetime EventDate { get; set; }
        public string Venue { get; set; }
        public string Status { get; set; }

        // Relación: Un Evento tiene muchos Sectores
        public virtual ICollection<Sector> Sectors { get; set; } = new List<Sector>(); [cite: 99]
    }
}
