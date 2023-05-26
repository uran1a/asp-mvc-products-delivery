using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsDelivery.Models;
using ProductsDelivery.Service;

namespace ProductsDelivery.Controllers
{
    [Authorize]
    public class ProviderController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;
        private readonly ProductService _productService;
        private readonly ApplicationService _applicationService;

        public ProviderController(ILogger<HomeController> logger, ApplicationContext context, ProductService productService, ApplicationService applicationService)
        {
            _logger = logger;
            _context = context;
            _productService = productService;
            _applicationService = applicationService;
        }
        public IActionResult Applications()
        {
            return View(_applicationService.ApplicationsFindByProviderId(int.Parse(User.Identity!.Name!)));
        }
        public async Task<IActionResult> Products()
        {
            return View(await _productService.ProductsFindByProviderIdAsync(int.Parse(User.Identity!.Name!)));
        }
        public IActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product model, IFormFile image)
        {
            if (model != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(image.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)image.Length);
                }
                model.Content = imageData;
                model.ProviderId = int.Parse(User.Identity!.Name!);
                await _productService.AddProductAsync(model);
            }
            return RedirectToAction("Products");
        }
    }
}
