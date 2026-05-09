using System;

namespace Application.Commands
{
    public class ProcessPaymentCommand
    {
        public Guid ReservationId { get; set; }
        public int UserId { get; set; }
        public string CardNumber { get; set; } 
        public string CardHolderName { get; set; }
    }
}
