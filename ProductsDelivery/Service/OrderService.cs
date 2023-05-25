using Microsoft.EntityFrameworkCore;
using ProductsDelivery.Models;

namespace ProductsDelivery.Service
{
    public class OrderService
    {
        private readonly ApplicationContext _db;
        private readonly ILogger<OrderService> _logger;
        public OrderService(ApplicationContext dbContext, ILogger<OrderService> logger)
        {
            _db = dbContext;
            _logger = logger;
        }
        public async Task<Order> OrderFindByUserIdAsync(int userid)
        {
            return await _db.Orders.
                Include(o => o.Products).SingleOrDefaultAsync(o => o.UserId == userid);
        }
        public async Task<Order> CreateOrderAsync(int userId) 
        {
            var newOrder = new Order()
            {
                DateCreated = DateTime.Now,
                UserId = userId
            };
            var order = _db.Orders.Add(newOrder);
            await _db.SaveChangesAsync();
            if(order != null)
            {
                _logger.LogInformation("Create new order with id = {0}", order.Entity.Id);
            }

            return _db.Orders
                .Include(o => o.Products).SingleOrDefault(o => o.Id == order.Entity.Id);
        }
    }
}
