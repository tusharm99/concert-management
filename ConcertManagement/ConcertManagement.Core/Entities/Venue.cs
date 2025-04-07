using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConcertManagement.Core.Entities;

public partial class Venue : EntityBase
{
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Address1 { get; set; } = null!;

    [StringLength(100)]
    public string? Address2 { get; set; }

    [StringLength(100)]
    public string City { get; set; } = null!;

    [StringLength(5)]
    public string State { get; set; } = null!;

    [StringLength(100)]
    public string Zip { get; set; } = null!;

    public int Capacity { get; set; }

    [StringLength(100)]
    public string TimeZone { get; set; } = null!;

    [StringLength(100)]
    public string AdminName { get; set; } = null!;

    [StringLength(50)]
    public string AdminPhone { get; set; } = null!;

    [StringLength(50)]
    public string? AdminEmail { get; set; }

    [StringLength(100)]
    public string CreatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [StringLength(100)]
    public string UpdatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime UpdatedDate { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("Venue")]
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
