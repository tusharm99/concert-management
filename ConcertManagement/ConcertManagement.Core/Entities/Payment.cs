using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConcertManagement.Core.Entities;

public partial class Payment
{
    [Key]
    public int Id { get; set; }

    public int ReservationId { get; set; }

    [StringLength(50)]
    public string PaymentMethod { get; set; } = null!;

    [StringLength(50)]
    public string PaymentStatus { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime PaymentDate { get; set; }

    [StringLength(100)]
    public string TransactionId { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal AmountPaid { get; set; }

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
    [InverseProperty("Payments")]
    public virtual Reservation Reservation { get; set; } = null!;
}
