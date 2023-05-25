using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsDelivery.Models;
using ProductsDelivery.Service;
using System.Diagnostics;

namespace ProductsDelivery.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context, ProductService productService, OrderService orderService)
        {
            _logger = logger;
            _context = context;
            _productService = productService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.AllProductsAsync();
            return View(products);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(int serialCode, int count)
        {
            var order = await _orderService.OrderFindByUserIdAsync(int.Parse(User.Identity!.Name!));
            if (order == null)
                order = await _orderService.CreateOrderAsync(int.Parse(User.Identity!.Name!));
            await _productService.UpdateOrderIdAsync(order.Id, serialCode, count);
            return Redirect("~/Home/Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}