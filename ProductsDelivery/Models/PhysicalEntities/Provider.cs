using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsDelivery.Models
{
    public class Provider : Person
    {
        public string LicenseNumber { get; set; } = null!;
        [Required]
        public virtual List<Product> Products { get; set; } = null!;
        [Required]
        public virtual List<Application> Applications { get; set; } = null!;
    }
}   
