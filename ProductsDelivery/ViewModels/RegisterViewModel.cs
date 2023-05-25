using System.ComponentModel.DataAnnotations;

namespace ProductsDelivery.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        public string Login { get; set; } = "";
        [Required]
        [DataType(DataType.Password)]
        [StringLength(50)]
        public string Password { get; set; } = "";
        [StringLength(50)]
        public string Surname { get; set; } = null!;
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [StringLength(50)]
        public string? Patronymic { get; set; }
        public DateTime Data_Of_Birth { get; set; }
        [StringLength(100)] 
        public string Address { get; set; } = null!;
        [StringLength(50)]
        public string? Mobile_Phone { get; set; }
    }
}
