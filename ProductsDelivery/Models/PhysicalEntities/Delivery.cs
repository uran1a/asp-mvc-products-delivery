using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsDelivery.Models
{
    public class Delivery : Person
    {
        public string TypeTransport { get; set; } = null!;
    }
}
