using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcertManagement.Core.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int TicketTypeId { get; set; }

        public string ReservationCode { get; set; } = null!;

        public int Quantity { get; set; }

        public bool IsConfirmed { get; set; }

        public string ContactName { get; set; } = null!;

        public string ContactPhone { get; set; } = null!;

        public string? ContactEmail { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool IsActive { get; set; }

        // Optionally include related entities as nested DTOs:
        public EventDto? Event { get; set; }             // Optional, if you want to include Event details
        public TicketTypeDto? TicketType { get; set; }   // Optional, if you want to include TicketType details
        public List<PaymentDto>? Payments { get; set; }  // Optional
        public List<TicketDto>? Tickets { get; set; }    // Optional
    }
}
