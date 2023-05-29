using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductsDelivery.Models;
using ProductsDelivery.Service;

namespace ProductsDelivery.Controllers
{
    [Authorize]
    public class ManagerController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly PersonService _personService;
        private readonly ApplicationService _applicationService;

        public ManagerController(ApplicationContext context, ProductService productService, OrderService orderService, PersonService personService, ApplicationService applicationService)
        {
            _context = context;
            _productService = productService;
            _orderService = orderService;
            _personService = personService;
            _applicationService = applicationService;
        }
        public async Task<IActionResult> Orders()
        {
            List<Order> orders = await _orderService.OrdersForManagerAsync();
            Dictionary<int, List<Product>> uniqueProducts = new Dictionary<int, List<Product>>();
            foreach(var order in orders)
            {
                uniqueProducts.Add(order.Id, _productService.UniqueProducts(order.Products));
            }
            ViewBag.UniqueProducts = uniqueProducts;

            List<SelectListItem> collectorItems = new List<SelectListItem>();
            var collectors = _personService.AllCollectors();
            foreach(var collector in collectors)
            {
                collectorItems.Add(new SelectListItem() { 
                    Text = collector.Name + " " + collector.Surname + " " + collector.Patronymic, 
                    Value = collector.Id.ToString() });
            }
            ViewBag.CollectorItems = collectorItems;

            List<SelectListItem> devileryItems = new List<SelectListItem>();
            var deliveries = _personService.AllDeliveries();
            foreach (var delivery in deliveries)
            {
                devileryItems.Add(new SelectListItem()
                {
                    Text = delivery.Name + " " + delivery.Surname + " " + delivery.Patronymic,
                    Value = delivery.Id.ToString()
                });
            }
            ViewBag.DevileryItems = devileryItems;
            return View(orders);
        }
        public async Task<IActionResult> Products()
        {
            List<Product> products = await _productService.AllProductsWithCountZeroAsync();
            List<SelectListItem> productsItems = new List<SelectListItem>();
            foreach (var product in products)
            {
                productsItems.Add(new SelectListItem()
                {
                    Text = product.Title + " " + product.Brand,
                    Value = product.Id.ToString()
                });
            }
            ViewBag.ProductsItems = productsItems;
            return View(products);
        }
        [HttpPost]
        public async Task<IActionResult> CreateApplication(int productId, int count)
        {
            var id = _personService.ProviderFindByProductId(productId);
            await _applicationService.CreateApplicationAsync(new Application() { 
                ProductId = productId, 
                Count = count, 
                ProviderId = id });
            return Redirect("Products");
        }
        [HttpPost]
        public async Task<IActionResult> AcceptOrder(int orderId, int collectorId, int deliveryId)
        {
            await _orderService.AddCollectorAndDeliveryAsync(orderId, collectorId, deliveryId);
            return RedirectToAction("Orders");
        }

        public async Task<IActionResult> Profile() => View(await _personService.ManagerFindByIdAsync(int.Parse(User.Identity!.Name!)));
        [HttpPost]
        public async Task<IActionResult> UpdateManager(Manager model)
        {
            await _personService.UpdateManagerAsync(model);
            return RedirectToAction("Profile");
        }
        //Collector
        public IActionResult Collectors()
        {
            var collectors = _personService.AllCollectors();
            return View(collectors);
        }
        public IActionResult CreateCollector()
        {
            return View("Partial/Person/CreateCollectorPartial");
        }
        [HttpPost]
        public async Task<IActionResult> CreateCollector(Collector model)
        {
            await _personService.CreateCollectorAsync(model);
            return RedirectToAction("Collectors");
        }
        public async Task<IActionResult> DeleteCollector(int id)
        {
            await _personService.DeleteCollectorAsync(id);
            return RedirectToAction("Collectors");
        }
       
        //Delivery
        public IActionResult Deliveries()
        {
            var deliveries = _personService.AllDeliveries();
            return View(deliveries);
        }
        public IActionResult CreateDelivery()
        {
            return View("Partial/Person/CreateDeliveryPartial");
        }
        [HttpPost]
        public async Task<IActionResult> CreateDelivery(Delivery model)
        {
            await _personService.CreateDeliveryAsync(model);
            return RedirectToAction("Deliveries");
        }
        public async Task<IActionResult> DeleteDelivery(int id)
        {
            await _personService.DeleteDeliveryAsync(id);
            return RedirectToAction("Deliveries");
        }

        //Provider
        public IActionResult Providers()
        {
            var deliveries = _personService.AllProviders ();
            return View(deliveries);
        }
        public IActionResult CreateProvider()
        {
            return View("Partial/Person/CreateProviderPartial");
        }
        [HttpPost]
        public async Task<IActionResult> CreateProvider(Provider model)
        {
            await _personService.CreateProviderAsync(model);
            return RedirectToAction("Providers");
        }
        public async Task<IActionResult> DeleteProvider(int id)
        {
            await _personService.DeleteProviderAsync(id);
            return RedirectToAction("Providers");
        }
    }
}
