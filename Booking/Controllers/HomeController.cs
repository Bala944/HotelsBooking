using Booking.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Booking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }


        public async Task<IActionResult> AuthUser(AuthDTO authDTO)
        {
            if (!string.IsNullOrEmpty(authDTO.UserName) && !string.IsNullOrEmpty(authDTO.Password))
            {
                if (authDTO.UserName == "admin" && authDTO.Password == "admin")
                {
                    var claims = new List<Claim>
                    {
                        new Claim("Role", "Admin"), // Add roles or claims as needed
                        // Other claims
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "custom");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(claimsPrincipal);
                    return RedirectToAction("Homepage", "BookMyRoom", new { Area = "FrontOffice" });
                }
            }
            return RedirectToAction("Login");
        }


    }
}