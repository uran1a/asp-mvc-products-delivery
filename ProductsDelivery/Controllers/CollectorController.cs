using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsDelivery.Models;
using ProductsDelivery.Service;

namespace ProductsDelivery.Controllers
{
    [Authorize]
    public class CollectorController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly PersonService _personService;

        public CollectorController(ApplicationContext context, ProductService productService, OrderService orderService, PersonService personService)
        {
            _context = context;
            _productService = productService;
            _orderService = orderService;
            _personService = personService;
        }
        public async Task<IActionResult> Orders()
        {
            List<Order> orders = await _orderService.OrdersFindByIdAndNotCollected(int.Parse(User.Identity!.Name!));
            Dictionary<int, List<Product>> uniqueProducts = new Dictionary<int, List<Product>>();
            foreach (var order in orders)
            {
                uniqueProducts.Add(order.Id, _productService.UniqueProducts(order.Products));
            }
            ViewBag.UniqueProducts = uniqueProducts;
            return View(orders);
        }
        public async Task<IActionResult> CollectedOrders()
        {
            List<Order> orders = await _orderService.OrdersFindByIdAndCollected(int.Parse(User.Identity!.Name!));
            Dictionary<int, List<Product>> uniqueProducts = new Dictionary<int, List<Product>>();
            foreach (var order in orders)
            {
                uniqueProducts.Add(order.Id, _productService.UniqueProducts(order.Products));
            }
            ViewBag.UniqueProducts = uniqueProducts;
            return View(orders);
        }
        [HttpPost]
        public async Task<IActionResult> CollectOrder(int id, int count, List<string> products)
        {
            if(products.Count != count)
            {
                ModelState.AddModelError("", "Необходимо собрать все продукты в заказе");
                return RedirectToAction("Orders");
            }
            else
            {
                var order = await _orderService.OrderFindByIdAsync(id);
                if (order != null)
                {
                    order.IsCollected = true;
                    await _orderService.UpdateAsync(order);
                    return RedirectToAction("Orders");
                }
                else
                    return RedirectToAction("Orders");
            }
        }

        public async Task<IActionResult> Profile() => View(await _personService.CollectorFindById(int.Parse(User.Identity!.Name!)));
        [HttpPost]
        public async Task<IActionResult> UpdateCollector(Collector model)
        {
            await _personService.UpdateCollectorAsync(model);
            return RedirectToAction("Profile");
        }
    }
}
