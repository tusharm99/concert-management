using System.ComponentModel.DataAnnotations;

namespace ConcertManagement.Core.Entities
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
