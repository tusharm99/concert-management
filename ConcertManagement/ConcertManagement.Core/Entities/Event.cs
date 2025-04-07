using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConcertManagement.Core.Entities;

public partial class Event
{
    [Key]
    public int Id { get; set; }

    public int VenueId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndDate { get; set; }

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

    [InverseProperty("Event")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    [InverseProperty("Event")]
    public virtual ICollection<TicketType> TicketTypes { get; set; } = new List<TicketType>();

    [ForeignKey("VenueId")]
    [InverseProperty("Events")]
    public virtual Venue Venue { get; set; } = null!;
}
