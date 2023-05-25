using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsDelivery.Models
{
    [Table("Product")]
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SerialCode { get; set; }
        public string Title { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public int Price { get; set; }
        [NotMapped]
        public int Count { get; set; }
        [Required]
        public int ProviderId { get; set; }
        [Required]
        public Provider Provider { get; set; } = null!;
        public int? OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
