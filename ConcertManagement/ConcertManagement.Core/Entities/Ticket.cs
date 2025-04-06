using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConcertManagement.Core.Entities;

public partial class Ticket
{
    [Key]
    public int Id { get; set; }

    public int ReservationId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PurchaseDate { get; set; }

    [StringLength(100)]
    public string TicketCode { get; set; } = null!;

    [StringLength(20)]
    public string? SeatNo { get; set; }

    [StringLength(100)]
    public string CreatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [StringLength(100)]
    public string UpdatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime UpdatedDate { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("ReservationId")]
    [InverseProperty("Tickets")]
    public virtual Reservation Reservation { get; set; } = null!;
}
