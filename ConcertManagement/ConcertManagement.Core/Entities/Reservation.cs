using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ConcertManagement.Core.Entities;

public partial class Reservation : EntityBase
{
    [Key]
    public int Id { get; set; }

    public int EventId { get; set; }

    public int TicketTypeId { get; set; }

    public int Quantity { get; set; }

    public bool IsConfirmed { get; set; }

    [StringLength(100)]
    public string CreatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [StringLength(100)]
    public string UpdatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime UpdatedDate { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("Reservations")]
    public virtual Event Event { get; set; } = null!;

    [ForeignKey("TicketTypeId")]
    [InverseProperty("Reservations")]
    public virtual TicketType TicketType { get; set; } = null!;

    [InverseProperty("Reservation")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
