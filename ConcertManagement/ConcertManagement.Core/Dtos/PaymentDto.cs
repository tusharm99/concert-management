﻿namespace ConcertManagement.Core.Dtos
{
    public class PaymentDto
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public string PaymentStatus { get; set; } = null!;

        public DateTime PaymentDate { get; set; }

        public string TransactionId { get; set; } = null!;

        public decimal AmountPaid { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool IsActive { get; set; }
    }

}
