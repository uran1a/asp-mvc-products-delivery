        using Microsoft.AspNetCore.Mvc;
using ProductsDelivery.Service;
using ProductsDelivery.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using ProductsDelivery.ViewModels;

namespace ProductsDelivery.Controllers
{
    public class PersonController : Controller
    {
        private readonly PersonService _personService;
        public PersonController(PersonService personService)
        {
            _personService = personService;
        }
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var person = await _personService.PersonByLoginAndPasswordAsync(model.Login, model.Password);
            if (person != null)
            {
                await Authenticate(person);
                if (person.Discriminator == "User") return RedirectToAction("Index", "Home");
                else if (person.Discriminator == "Manager") return RedirectToAction("Orders", "Manager");
                else if (person.Discriminator == "Collector") return RedirectToAction("Orders", "Collector");
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(User model)
        {
            if (model != null)
            {
                var person = await _personService.PersonWithLoginAsync(model.Login);
                if (person == null)
                {
                    var newUser = await _personService.CreateUserAsync(model);
                    await Authenticate(newUser);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логин уже существует");
                }
            }
            return RedirectToAction("Register", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(User model)
        {
            if (model == null)
                return RedirectToAction("Profile", "Home");
            else
                await _personService.UpdateUser(model);
            return RedirectToAction("Profile", "Home");
        }
        private async Task Authenticate(Person person)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, person.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, person.Discriminator)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Person");
        }
    }
}
