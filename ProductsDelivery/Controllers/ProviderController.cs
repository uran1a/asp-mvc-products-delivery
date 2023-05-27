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
        public async Task<IActionResult> AcceptApplication(int id, int count)
        {
            var product = await _productService.ProductFindByIdAsync(id);
            List<Product> list = new List<Product>();
            for(int i = 0; i < count; i++)
            {
                list.Add(new Product
                {
                    SerialCode = product.SerialCode,
                    Title = product.Title,
                    Brand = product.Brand,
                    Price = product.Price,
                    Content = product.Content,
                    ProviderId = product.ProviderId
                });
            }
            /*int serail = _productService.SerialCodeFindByProduct(product);
            product.SerialCode = serail;*/
            await _productService.AddProductsAsync(list);
            await _applicationService.DeleteAsync(int.Parse(User.Identity!.Name!));
            return Redirect("Applications");
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
