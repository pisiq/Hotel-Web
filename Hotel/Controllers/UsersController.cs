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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProfilePicture(ProfileViewModel model)
        {
            if (model.ProfilePicture == null)
            {
                return RedirectToAction("Profile");
            }

            var email = User.Identity.Name;
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            // Process uploaded file
            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                // Get file extension
                var fileExtension = Path.GetExtension(model.ProfilePicture.FileName).ToLowerInvariant();

                // Only accept certain file types
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("ProfilePicture", "Only image files (.jpg, .jpeg, .png, .gif) are allowed");
                    return RedirectToAction("Profile");
                }

                // Create a unique filename
                var fileName = $"{user.Id}_{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles", fileName);

                // Ensure directory exists
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles"));

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(fileStream);
                }

                // Delete old profile picture if exists
                if (!string.IsNullOrEmpty(user.ProfilePicturePath))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfilePicturePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Update database with new path
                user.ProfilePicturePath = $"/images/profiles/{fileName}";
                await _userService.UpdateUserAsync(user);
            }

            return RedirectToAction("Profile");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Get the current user's email from claims
            var email = User.Identity.Name;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            // Fetch user data from your service
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            // Map user data to ProfileViewModel
            var viewModel = new ProfileViewModel
            {
                Email = user.Email,
                FirstName = user.Name?.Split(' ').FirstOrDefault() ?? "",
                LastName = user.Name?.Split(' ').Skip(1).FirstOrDefault() ?? "",
                ProfilePicturePath = user.ProfilePicturePath
            };

            return View(viewModel);
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