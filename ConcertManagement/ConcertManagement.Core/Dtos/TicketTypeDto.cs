using System.ComponentModel.DataAnnotations;

namespace ConcertManagement.Core.Dtos
{
    public class TicketTypeDto
    {
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public int TotalSeats { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
