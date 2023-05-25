using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsDelivery.Models
{
    [Table("Order")]
    public class Order
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public bool IsDelivered { get; set; } = false;
        [Required]
        public bool IsCollected { get; set; } = false;
        [Required]
        public bool IsManaged { get; set; } = false;
        [Required]
        public int UserId { get; set; }
        [Required]
        public User User { get; set; } = null!;
        [Required]
        public virtual List<Product> Products { get; set; } = null!;
    }
}
