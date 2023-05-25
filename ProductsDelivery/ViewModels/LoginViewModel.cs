using System.ComponentModel.DataAnnotations;
namespace ProductsDelivery.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указан Login")]
        public string Login { get; set; } = null!;
        [Required(ErrorMessage = "Не указан Password")]
        public string Password { get; set; } = null!;
    }
}