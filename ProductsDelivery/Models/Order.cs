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
        public bool IsGenerated { get; set; } = false;
        [Required]
        public bool IsPaid { get; set; } = false;
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
        public int? CollectorId { get; set; }
        public Collector? Collector { get; set; }
        public int? DeliveryId { get; set; }
        public Delivery? Delivery { get; set; }
        [Required]
        public virtual List<Product> Products { get; set; } = null!;
        [Required]
        public int Amount { get; set; }
    }
}
