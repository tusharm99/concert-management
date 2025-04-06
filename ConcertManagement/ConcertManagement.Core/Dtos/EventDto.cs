using System.ComponentModel.DataAnnotations;

namespace ConcertManagement.Core.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [StringLength(100)]
        [Required]
        public string ContactName { get; set; }
        [StringLength(50)]
        [Required]
        public string ContactPhone { get; set; }
        [StringLength(50)]
        public string? ContactEmail { get; set; }
        [Required]
        public int VenueId { get; set; }
        [StringLength(100)]
        public string VenueName { get; set; } // Flattened from Venue
        public List<TicketTypeDto> TicketTypes { get; set; }
    }

}
