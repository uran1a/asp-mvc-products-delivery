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
        public async Task<Order?> OrderFindByUserIdAsync(int userid)
        {
            return await _db.Orders
                .Include(o => o.User)
                .Include(o => o.Products).SingleOrDefaultAsync(o => o.UserId == userid);
        }
        public async Task<Order?> OrderFindByIdAsync(int id)
        {
            return await _db.Orders
                .Include(o => o.User)
                .Include(o => o.Products).SingleOrDefaultAsync(o => o.Id == id);
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

        internal async Task<List<Order>> OrdersForManagerAsync()
        {
            return await _db.Orders
                .Include(o => o.Collector)
                .Include(o => o.Delivery)
                .Include(o => o.User)
                .Include(o => o.Products)
                .Where(o => o.IsGenerated)
                .Where(o => o.IsPaid).ToListAsync();
        }

        public async Task DeleteProductInOrderAsync(Order order, int serial)
        {
            var product = order.Products.FirstOrDefault(p => p.SerialCode == serial);
            product.OrderId = null;
            product.Order = null;
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
        }

        public async Task AddCollectorAndDeliveryAsync(int orderId, int collectorId, int deliveryId)
        {
            var order = await this.OrderFindByIdAsync(orderId);
            if(order != null)
            {
                order.CollectorId = collectorId;
                order.DeliveryId = deliveryId;
                order.IsManaged = true;
                _db.Orders.Update(order);
                await _db.SaveChangesAsync();
            }
        }
    }
}
