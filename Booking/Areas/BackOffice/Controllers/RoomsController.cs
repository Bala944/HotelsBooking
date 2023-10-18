using Booking.Areas.BackOffice.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
    public class RoomsController : Controller
    {
        public required IRoomsRepository _roomsRepository;

        /// <summary>
        /// Constructor to initialize the object
        /// </summary>
        /// <param name="roomsRepository"></param>
        public RoomsController(IRoomsRepository roomsRepository)
        {
            _roomsRepository = roomsRepository;
        }

        /// <summary>
        /// To view the rooms
        /// </summary>
        /// <returns></returns>
        public IActionResult ViewRooms()
        {
            return View();
        }
    }
}
