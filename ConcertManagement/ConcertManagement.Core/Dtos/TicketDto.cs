namespace ConcertManagement.Core.Dtos
{
    public class TicketDto
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public string TicketCode { get; set; } = null!;

        public string? SeatNo { get; set; }
    }

}
