using System.ComponentModel.DataAnnotations;

namespace ConcertManagement.Core.Dtos
{
    public class ReservationRequest
    {
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int TicketTypeId { get; set; }

        public string ReservationCode { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }

        public bool IsConfirmed { get; set; }

        [Required]
        public string ContactName { get; set; } = null!;

        [Required]
        public string ContactPhone { get; set; } = null!;

        public string? ContactEmail { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
