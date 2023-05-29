using Microsoft.EntityFrameworkCore;
using ProductsDelivery.Models;
using System.Linq;

namespace ProductsDelivery.Service
{
    public class PersonService
    {
        private readonly ApplicationContext _db;
        private readonly ILogger<PersonService> _logger; 
        public PersonService(ApplicationContext dbContext, ILogger<PersonService> logger)
        {
            _db = dbContext;
            _logger = logger;
        }
        //User
        public async Task<User?> UserFindById(int id)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user != null)
                return user;
            return null;
        }
        public async Task<Person> CreateUserAsync(User user)
        {
            var newUser = _db.People.Add(user);
            await _db.SaveChangesAsync();
            if (newUser != null)
            {
                _logger.LogInformation("Create user with id = {0}", newUser.Entity.Id);
                return newUser.Entity;
            }
            return null;
        }
        public async Task UpdateUser(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        //Collector
        public List<Collector> AllCollectors() => _db.Collectors.ToList();
        public async Task<Collector> CreateCollectorAsync(Collector collector)
        {
            var newCollector = _db.Collectors.Add(collector);
            await _db.SaveChangesAsync();
            if (newCollector != null)
            {
                _logger.LogInformation("Create collector with id = {0}", newCollector.Entity.Id);
                return newCollector.Entity;
            }
            return null;
        }
        public async Task DeleteCollectorAsync(int id)
        {
            var collect = await _db.Collectors.Where(c => c.Id == id).SingleOrDefaultAsync();
            _db.Collectors.Remove(collect);
            await _db.SaveChangesAsync();
        }

        //Delivery
        public List<Delivery> AllDeliveries() => _db.Deliveries.ToList();
        public async Task<Delivery> CreateDeliveryAsync(Delivery delivery)
        {
            var newDelivery = _db.Deliveries.Add(delivery);
            await _db.SaveChangesAsync();
            if (newDelivery != null)
            {
                _logger.LogInformation("Create delivery with id = {0}", newDelivery.Entity.Id);
                return newDelivery.Entity;
            }
            return null;
        }
        public async Task DeleteDeliveryAsync(int id)
        {
            var delivery = await _db.Deliveries.Where(d => d.Id == id).SingleOrDefaultAsync();
            _db.Deliveries.Remove(delivery);
            await _db.SaveChangesAsync();
        }
        public async Task<Collector> CollectorFindById(int id)
        {
            var collector = await _db.Collectors.SingleOrDefaultAsync(c => c.Id == id);
            if (collector != null) return collector;
            else throw new ArgumentNullException();
        }
        public async Task<Delivery> DeliveryFindById(int id)
        {
            var delivery = await _db.Deliveries.SingleOrDefaultAsync(d => d.Id == id);
            if (delivery != null) return delivery;
            else throw new ArgumentNullException();
        }
        public async Task UpdateCollectorAsync(Collector model)
        {
            _db.Collectors.Update(model);
            await _db.SaveChangesAsync();
        }
        public async Task UpdateDeliveryAsync(Delivery model)
        {
            _db.Deliveries.Update(model);
            await _db.SaveChangesAsync();
        }
        public int ProviderFindByProductId(int productId)
        {
            var product = _db.Products.Where(p => p.Id == productId).FirstOrDefault();
            return product.ProviderId;
        }

        //Delivery
        public List<Provider> AllProviders() => _db.Providers.ToList();
        internal async Task UpdateManagerAsync(Manager model)
        {
            _db.Managers.Update(model);
            await _db.SaveChangesAsync();
        }
        public async Task<Provider> CreateProviderAsync(Provider provider)
        {
            var newProvider = _db.Providers.Add(provider);
            await _db.SaveChangesAsync();
            if (newProvider != null)
            {
                _logger.LogInformation("Create provider with id = {0}", newProvider.Entity.Id);
                return newProvider.Entity;
            }
            return null;
        }
        public async Task DeleteProviderAsync(int id)
        {
            var provider = await _db.Providers.Where(d => d.Id == id).SingleOrDefaultAsync();
            _db.Providers.Remove(provider);
            await _db.SaveChangesAsync();
        }

        //Manager
        public async Task<Manager> ManagerFindByIdAsync(int id)
        {
            var manager = await _db.Managers.SingleOrDefaultAsync(m => m.Id == id);
            if (manager != null)
                return manager;
            else 
                throw new ArgumentNullException();
        }
        public async Task<Person> PersonWithLoginAsync(string login)
        {
            var person = await _db.People.FirstOrDefaultAsync(p => p.Login == login);
            if (person != null)
                _logger.LogInformation("Get users with login = {0}", login);
            else
                _logger.LogError("Can't get user with login = {0}", login);
            return person;
        }
        public async Task<Person> PersonByLoginAndPasswordAsync(string login, string password)
        {
            var person = await _db.People
                .FirstOrDefaultAsync(a => a.Login == login && a.Password == password);
            if (person != null)
                _logger.LogInformation("Get person with login = {0} and password {1}", person.Login, person.Password);
            return person;
        }
    }
}
