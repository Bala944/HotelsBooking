using Booking.Areas.FrontOffice.Data.Interface;
using Booking.Areas.FrontOffice.Models.Input;
using Booking.Areas.FrontOffice.Models.Output;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Areas.FrontOffice.Controllers
{
    [Area("FrontOffice")]
    public class BookMyRoomController : Controller
    {
        private readonly IBookMyRoomRepository _bookMyRoomRepository;

        public BookMyRoomController(IBookMyRoomRepository bookMyRoomRepository)
        {
            _bookMyRoomRepository = bookMyRoomRepository;
        }

        [Route("/home-page")]
        public IActionResult Homepage(RoomFilterDTO roomFilterDTO)
        {
            BookMyRoomResultDTO bookMyRoomResultDTO = new BookMyRoomResultDTO();
            if (roomFilterDTO.CheckInDate != null && roomFilterDTO.CheckOutDate != null)
            {
                var rooms = _bookMyRoomRepository.GetRooms(roomFilterDTO);
                if (rooms != null)
                {
                    bookMyRoomResultDTO.listRoomsDetailsDTO = rooms.Result;
                }
            }
            bookMyRoomResultDTO.roomFilterDTO = roomFilterDTO;
            return View(bookMyRoomResultDTO);
        }

        [Route("/room-details-page")]
        public async Task<IActionResult> SingleRoomDetails(Int64 roomId)
        {
            RoomsDetailsDTO result = new RoomsDetailsDTO();
            result = await _bookMyRoomRepository.GetRoomsById(roomId);
            return View(result);
        }
    }
}