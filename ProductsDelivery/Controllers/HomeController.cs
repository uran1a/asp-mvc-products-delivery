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
        private readonly PersonService _personService;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context, ProductService productService, OrderService orderService, PersonService personService)
        {
            _logger = logger;
            _context = context;
            _productService = productService;
            _orderService = orderService;
            _personService = personService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.AllProductsAsync();
            return View(products);
        }
        public async Task<IActionResult> Processes()
        {
            var order = await _orderService.OrderFindByUserIdAndNotFinishedAsync(int.Parse(User.Identity!.Name!));
            if(order != null)
            {
                if(order.IsGenerated == false)
                {
                    order.IsGenerated = true;
                    order.IsPaid = true;
                    order.Amount = _productService.AmountProducts(order.Products);
                    await _orderService.UpdateAsync(order);
                }
            }
            return View(order);
        }
        public async Task<IActionResult> Cart()
        {
            var order = await _orderService.OrderFindByUserIdAndNotFinishedAsync(int.Parse(User.Identity!.Name!));
            if (order != null)
            {       
                if (order.IsGenerated == true)
                {
                    return RedirectToAction("Processes");
                }
                else
                {
                    if (order.Products.Count >= 0)
                    {
                        ViewBag.Amount = _productService.AmountProducts(order.Products);
                        ViewBag.UniqueProducts = _productService.UniqueProducts(order.Products);
                        return View(order);
                    }
                }
               
            }
            return Redirect("~/Home/Index");
        }
        public async Task<IActionResult> Profile()
        {
            var user = await _personService.UserFindById(int.Parse(User.Identity!.Name!));
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(int serialCode, int count)
        {
            var order = await _orderService.OrderFindByUserIdAndNotFinishedAsync(int.Parse(User.Identity!.Name!));
            if (order == null)
                order = await _orderService.CreateOrderAsync(int.Parse(User.Identity!.Name!));
            await _productService.UpdateOrderIdAsync(order.Id, serialCode, count);
            return Redirect("~/Home/Index");
        }
        public async Task<IActionResult> DeleteProductInOrder (int serial)
        {
            var order = await _orderService.OrderFindByUserIdAsync(int.Parse(User.Identity!.Name!));
            await _orderService.DeleteProductInOrderAsync(order, serial);
            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> FinishedOrder(int id)
        {
            var order = await _orderService.OrderFindByIdAsync(id);
            if (order != null)
            {
                order.IsFinished = true;
                await _orderService.UpdateAsync(order);
            }
            return RedirectToAction("Index");
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