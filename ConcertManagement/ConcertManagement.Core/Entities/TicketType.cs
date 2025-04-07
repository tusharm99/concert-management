using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConcertManagement.Core.Entities;

public partial class TicketType
{
    [Key]
    public int Id { get; set; }

    public int EventId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Description { get; set; } = null!;

    public int TotalSeats { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

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
    [InverseProperty("TicketTypes")]
    public virtual Event Event { get; set; } = null!;

    [InverseProperty("TicketType")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
