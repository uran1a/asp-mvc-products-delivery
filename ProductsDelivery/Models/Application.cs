using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsDelivery.Models
{
    [Table("Application")]
    public class Application
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public Product Product { get; set; } = null!;
        [Required]
        public int ProviderId { get; set; }
        [Required]
        public Provider Provider { get; set; } = null!;
        [Required]
        public int Count { get; set; }
    }
}
