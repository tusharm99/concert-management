namespace ConcertManagement.Core.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public int VenueId { get; set; }
        public string VenueName { get; set; } // Flattened from Venue
    }

}
