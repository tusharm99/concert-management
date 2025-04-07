namespace ConcertManagement.Core.Dtos
{
    public class EventSeatsAvailability
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public int TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public int MaxSeats { get; set; }
        public int AvailableTickets { get; set; }
    }
}
