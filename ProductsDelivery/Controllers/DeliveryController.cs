using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsDelivery.Models;
using ProductsDelivery.Service;

namespace ProductsDelivery.Controllers
{
    [Authorize]
    public class DeliveryController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly PersonService _personService;

        public DeliveryController(ApplicationContext context, ProductService productService, OrderService orderService, PersonService personService)
        {
            _context = context;
            _productService = productService;
            _orderService = orderService;
            _personService = personService;
        }
        public async Task<IActionResult> Orders()
        {
            List<Order> orders = await _orderService.OrdersFindByIdAndNotDeliveried(int.Parse(User.Identity!.Name!));
            Dictionary<int, List<Product>> uniqueProducts = new Dictionary<int, List<Product>>();
            foreach (var order in orders)
                uniqueProducts.Add(order.Id, _productService.UniqueProducts(order.Products));
            ViewBag.UniqueProducts = uniqueProducts;
            return View(orders);
        }

        public async Task<IActionResult> DeliveriedOrder(int id)
        {
            var order = await _orderService.OrderFindByIdAsync(id);
            if (order != null)
            {
                order.IsDelivered = true;
                await _orderService.UpdateAsync(order);
                return RedirectToAction("Orders");
            }
            else
            {
                return RedirectToAction("Orders");
            }
        }
        public async Task<IActionResult> DeliveriedOrders()
        {
            List<Order> orders = await _orderService.OrdersFindByIdAndDeliveried(int.Parse(User.Identity!.Name!));
            Dictionary<int, List<Product>> uniqueProducts = new Dictionary<int, List<Product>>();
            foreach (var order in orders)
                uniqueProducts.Add(order.Id, _productService.UniqueProducts(order.Products));
            ViewBag.UniqueProducts = uniqueProducts;
            return View(orders);
        }
        public async Task<IActionResult> Profile() => View(await _personService.DeliveryFindById(int.Parse(User.Identity!.Name!)));
        [HttpPost]
        public async Task<IActionResult> UpdateDelivery(Delivery model)
        {
            await _personService.UpdateDeliveryAsync(model);
            return RedirectToAction("Profile");
        }
    }
}
