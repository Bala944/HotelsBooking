using Microsoft.AspNetCore.Mvc;

namespace Booking.Areas.FrontOffice.Controllers
{
    [Area("FrontOffice")]
    public class BookMyRoomController : Controller
    {
        [Route("/home-page")]
        public IActionResult Homepage()
        {
            return View();
        }

        public IActionResult AvailableRooms()
        {
            return View();
        }
    }
}
