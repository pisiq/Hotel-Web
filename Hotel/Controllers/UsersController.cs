using Hotel.Services;
using Hotel.ViewsModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly SignInManager<IdentityUser> _signInManager;


        public UsersController(IUserService userService, SignInManager<IdentityUser> signInManager)
        {
            _userService = userService;
            _signInManager = signInManager;
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
                // Sign in the user using SignInManager
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
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
    }
}