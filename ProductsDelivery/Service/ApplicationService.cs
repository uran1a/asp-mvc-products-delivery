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
                .Where(a => a.ProviderId == id).ToList();
        }
    }
}
