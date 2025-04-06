namespace ConcertManagement.Core.Dtos
{
    public class TicketTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int TotalSeats { get; set; }
        public decimal Price { get; set; }
        public int EventId { get; set; }
    }
}
