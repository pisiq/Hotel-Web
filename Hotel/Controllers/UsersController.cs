using Hotel.Services;
using Hotel.ViewsModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isAuthenticated = await _userService.AuthenticateUserAsync(model);

            if (isAuthenticated)
            {
                // Store user email in session
                HttpContext.Session.SetString("UserEmail", model.Email);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }

        [HttpGet]
        public IActionResult Sign()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Sign(SignModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _userService.RegisterUserAsync(model);

            if (result)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Email already in use");
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}