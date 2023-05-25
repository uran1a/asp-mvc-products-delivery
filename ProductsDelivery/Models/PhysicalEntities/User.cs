using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsDelivery.Models
{
    public class User : Person
    {
        public string Address { get; set; } = null!;
        [Required]
        public virtual List<Order> Orders { get; set; } = null!;
    }
}
