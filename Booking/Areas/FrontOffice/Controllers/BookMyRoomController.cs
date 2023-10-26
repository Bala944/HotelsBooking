using Booking.Areas.FrontOffice.Data.Interface;
using Booking.Areas.FrontOffice.Models.Input;
using Booking.Areas.FrontOffice.Models.Output;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            if (string.IsNullOrEmpty(roomFilterDTO.CheckInDate))
                roomFilterDTO.CheckInDate = DateTime.Now.ToString("dd/MM/yyyy");
            if (string.IsNullOrEmpty(roomFilterDTO.CheckOutDate))
                roomFilterDTO.CheckOutDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
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
        public async Task<IActionResult> SingleRoomDetails(SelectedRoomDTO selectedRoomDTO)
        {
            RoomsDetailsDTO result = new RoomsDetailsDTO();
            ViewBag.CheckIn = selectedRoomDTO.CheckIn;
            ViewBag.CheckOut=selectedRoomDTO.CheckOut;
            result = await _bookMyRoomRepository.GetRoomsById(selectedRoomDTO.RoomId);
            return View(result);
        }


        [Route("/register")]
        public async Task<IActionResult> RegisterCustomerDetails(RoomRegisterDTO roomRegisterDTO)
        {
           return View("RegisterCustomer");
        }

        [Route("/confirmBooking")]
        public async Task<IActionResult> ConfirmBooking(CustomerAndBookingDetails customerAndBookingDetails)
        {
            return View();
        }

        [Route("/view-room")]
        public IActionResult ViewMoreRooms(RoomFilterDTO roomFilterDTO)
        {
            BookMyRoomResultDTO bookMyRoomResultDTO = new BookMyRoomResultDTO();
            if (string.IsNullOrEmpty(roomFilterDTO.CheckInDate))
                roomFilterDTO.CheckInDate = DateTime.Now.ToString("dd/MM/yyyy");
            if (string.IsNullOrEmpty(roomFilterDTO.CheckOutDate))
                roomFilterDTO.CheckOutDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
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
    }
}