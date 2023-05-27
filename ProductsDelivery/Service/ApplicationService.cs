using Microsoft.EntityFrameworkCore;
using ProductsDelivery.Models;

namespace ProductsDelivery.Service
{
    public class ApplicationService
    {
        private readonly ApplicationContext _db;
        private readonly ILogger<OrderService> _logger;
        public ApplicationService(ApplicationContext dbContext, ILogger<OrderService> logger)
        {
            _db = dbContext;
            _logger = logger;
        }
        public List<Application> ApplicationsFindByProviderId(int id)
        {
            return _db.Applications
                .Include(a => a.Product)
                .Where(a => a.ProviderId == id).ToList();
        }

        internal async Task CreateApplicationAsync(Application application)
        {
            _db.Applications.Add(application);
            await _db.SaveChangesAsync();
        }

        internal async Task DeleteAsync(int v)
        {
            var app = await _db.Applications.SingleOrDefaultAsync(a => a.ProviderId == v);
            _db.Applications.Remove(app);
            await _db.SaveChangesAsync();
        }
    }
}
