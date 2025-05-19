using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Hotel.Models;
using Hotel.ViewModels;
using Hotel.Services;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContactSettingsService _contactSettingsService;
        private readonly IAboutService _aboutService;

        public HomeController(
            ILogger<HomeController> logger,
            IContactSettingsService contactSettingsService,
            IAboutService aboutService)
        {
            _logger = logger;
            _contactSettingsService = contactSettingsService;
            _aboutService = aboutService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Booking()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Sign()
        {
            return View();
        }

        public async Task<IActionResult> Contact()
        {
            ViewBag.ContactSettings = await _contactSettingsService.GetContactSettingsAsync();
            return View(new ContactViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ContactSettings = await _contactSettingsService.GetContactSettingsAsync();
                return View(model);
            }

            // Process the contact form (email sending logic would go here)

            TempData["SuccessMessage"] = "Your message has been sent successfully. We will get back to you soon.";
            return RedirectToAction(nameof(Contact));
        }

        public async Task<IActionResult> About()
        {
            var aboutContent = await _aboutService.GetAboutAsync();
            return View(aboutContent);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
