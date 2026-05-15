using System;

namespace Application.DTOs
{
    public class UserTicketDto
    {
        public Guid ReservationId { get; set; }
        public string EventName { get; set; }
        public string Venue { get; set; }
        public DateTime EventDate { get; set; }
        public string SectorName { get; set; }
        public string RowIdentifier { get; set; }
        public int SeatNumber { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
    }
}
