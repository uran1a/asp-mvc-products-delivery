using Microsoft.EntityFrameworkCore;
using ProductsDelivery.Models;

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
                .Where(p => p.Discriminator == "User")
                .FirstOrDefaultAsync(a => a.Login == login && a.Password == password);
            if (person != null)
                _logger.LogInformation("Get user with login = {0} and password {1}", person.Login, person.Password);
            return person;
        }
    }
}
