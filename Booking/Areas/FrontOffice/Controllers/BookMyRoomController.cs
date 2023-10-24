using Microsoft.AspNetCore.Mvc;

namespace Booking.Areas.FrontOffice.Controllers
{
    public class BookMyRoomController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
