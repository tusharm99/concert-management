using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ConcertManagement.Core.Entities;

[Index("ReservationCode", Name = "UQ_Reservations_ReservationCode", IsUnique = true)]
public partial class Reservation
{
    [Key]
    public int Id { get; set; }

    public int EventId { get; set; }

    public int TicketTypeId { get; set; }

    [StringLength(50)]
    public string ReservationCode { get; set; } = null!;

    public int Quantity { get; set; }

    public bool IsConfirmed { get; set; }

    [StringLength(100)]
    public string ContactName { get; set; } = null!;

    [StringLength(50)]
    public string ContactPhone { get; set; } = null!;

    [StringLength(50)]
    public string? ContactEmail { get; set; }

    [StringLength(100)]
    public string CreatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [StringLength(100)]
    public string UpdatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime UpdatedDate { get; set; }

    public bool IsActive { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    [ForeignKey("EventId")]
    [InverseProperty("Reservations")]
    public virtual Event Event { get; set; } = null!;

    [InverseProperty("Reservation")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("TicketTypeId")]
    [InverseProperty("Reservations")]
    public virtual TicketType TicketType { get; set; } = null!;

    [InverseProperty("Reservation")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
