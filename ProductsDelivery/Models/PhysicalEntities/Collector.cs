using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsDelivery.Models
{
    public class Collector : Person
    {
        public int HourShift { get; set; }
        [Required]
        public virtual List<Order> Orders { get; set; } = null!;
    }
}
