using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;
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

        [Route("~/add-room-details")]
        public async Task<IActionResult> AddRoomDetails()
        {

			AddRoomDTO roomsDTOs = new AddRoomDTO();
			//roomsDTOs = await _roomsRepository.GetAddRoomDetails();
			return View(roomsDTOs);
		}

        [Route("~/view-room-details")]
        public async Task<IActionResult> ViewRoomDetails()
        {

			List<RoomsDTO> roomsDTOs = new List<RoomsDTO>();

			//roomsDTOs = await _roomsRepository.GetRooms();

			return View(roomsDTOs);
		}
    }
}
