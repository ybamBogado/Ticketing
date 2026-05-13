namespace Application.Commands
{
    public class CancelReservationCommand 
    {
        public Guid ReservationId { get; set; }
        public int UserId { get; set; }
    }
}