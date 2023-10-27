using Booking.Areas.FrontOffice.Data.Interface;
using Booking.Areas.FrontOffice.Models.Input;
using Booking.Areas.FrontOffice.Models.Output;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.Xml;

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

                bookMyRoomResultDTO.roomFilterDTO = roomFilterDTO;

                BookingQueryDTO bookingQueryDTO = new BookingQueryDTO();
                bookingQueryDTO.CheckInDate = bookMyRoomResultDTO.roomFilterDTO.CheckInDate;
                bookingQueryDTO.CheckOutDate = bookMyRoomResultDTO.roomFilterDTO.CheckOutDate;
                bookingQueryDTO.Adults = bookMyRoomResultDTO.roomFilterDTO.Adults;
                bookingQueryDTO.Children = bookMyRoomResultDTO.roomFilterDTO.Children;
                bookingQueryDTO.Rooms = bookMyRoomResultDTO.roomFilterDTO.Rooms;
                string json = JsonConvert.SerializeObject(bookingQueryDTO); // Corrected method name
                string paramsEncrypted = EncryptionHelper.Encrypt(json);

                bookMyRoomResultDTO.roomFilterDTO.Params = paramsEncrypted;
                return RedirectToAction("ViewMoreRooms", new { Params = paramsEncrypted });
            }
            
            return View(bookMyRoomResultDTO);
           
        }

        [Route("/room-details-page")]
        public async Task<IActionResult> SingleRoomDetails(SelectedRoomDTO selectedRoomDTO)
        {
            SingleRoomDetails roomDetails = new SingleRoomDetails();
            RoomsDetailsDTO result = new RoomsDetailsDTO();
            BookingQueryDTO bookingQueryDTO= new BookingQueryDTO();
            if (selectedRoomDTO.Params != null)
            {
                try
                {
                    string decryptedData = EncryptionHelper.Decrypt(selectedRoomDTO.Params);
                     bookingQueryDTO = JsonConvert.DeserializeObject<BookingQueryDTO>(decryptedData);

                    bookingQueryDTO.SelectedRoomId = selectedRoomDTO.RoomId;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Error parsing JSON: " + ex.Message);
                }
            }

            ViewBag.BParams= selectedRoomDTO.Params;
            roomDetails.bookingQueryData = bookingQueryDTO;
            roomDetails.roomDetails = await _bookMyRoomRepository.GetRoomsById(selectedRoomDTO.RoomId);
            return View(roomDetails);
        }

        [Route("/view-room")]
        public IActionResult ViewMoreRooms(SelectedRoomDTO selectedRoomDTO)
        {
            BookMyRoomResultDTO bookMyRoomResultDTO = new BookMyRoomResultDTO();
            if (selectedRoomDTO.Params != null)
            {
                try
                {
                    string decryptedData = EncryptionHelper.Decrypt(selectedRoomDTO.Params);
                    BookingQueryDTO bookingQueryDTO = JsonConvert.DeserializeObject<BookingQueryDTO>(decryptedData);

                    //if (string.IsNullOrEmpty(bookingQueryDTO.CheckInDate))
                    //    bookingQueryDTO.CheckInDate = DateTime.Now.ToString("dd/MM/yyyy");
                    //if (string.IsNullOrEmpty(bookingQueryDTO.CheckOutDate))
                    //    bookingQueryDTO.CheckOutDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    if (bookingQueryDTO.CheckInDate != null && bookingQueryDTO.CheckOutDate != null)
                    {
                        RoomFilterDTO roomFilterDTO = new RoomFilterDTO();
                        roomFilterDTO.CheckInDate = bookingQueryDTO.CheckInDate;
                        roomFilterDTO.CheckOutDate = bookingQueryDTO.CheckOutDate;
                        roomFilterDTO.Adults = bookingQueryDTO.Adults;
                        roomFilterDTO.Children = bookingQueryDTO.Children;
                        roomFilterDTO.Rooms = bookingQueryDTO.Rooms;
                        var rooms = _bookMyRoomRepository.GetRooms(roomFilterDTO);
                        if (rooms != null)
                        {
                            bookMyRoomResultDTO.listRoomsDetailsDTO = rooms.Result;
                        }
                        bookMyRoomResultDTO.roomFilterDTO = roomFilterDTO;

                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Error parsing JSON: " + ex.Message);
                }
            }
            
            return View(bookMyRoomResultDTO);
        }

        [Route("/register")]
        public async Task<IActionResult> RegisterCustomerDetails(SelectedRoomDTO selectedRoomDTO)
        {
            SingleRoomDetails roomDetails = new SingleRoomDetails();
            RoomsDetailsDTO result = new RoomsDetailsDTO();
            BookingQueryDTO bookingQueryDTO = new BookingQueryDTO();
            if (selectedRoomDTO.Params != null)
            {
                try
                {
                    string decryptedData = EncryptionHelper.Decrypt(selectedRoomDTO.Params);
                    bookingQueryDTO = JsonConvert.DeserializeObject<BookingQueryDTO>(decryptedData);

                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Error parsing JSON: " + ex.Message);
                }
            }

            ViewBag.BParams = selectedRoomDTO.Params;
            roomDetails.bookingQueryData = bookingQueryDTO;
            roomDetails.roomDetails = await _bookMyRoomRepository.GetRoomsById(bookingQueryDTO.SelectedRoomId);
            return View("RegisterCustomer", roomDetails);
        }

        [Route("/confirmBooking")]
        public async Task<IActionResult> ConfirmBooking(CustomerAndBookingDetails customerAndBookingDetails)
        {
            return View();
        }
        [Route("/about-us")]
        public IActionResult AboutUs()
        {
            return View();
        }
        [Route("/contact-us")]
        public IActionResult ContactUs()
        {
            return View();
        }
        [Route("/services")]
        public IActionResult Services()
        {
            return View();
        }
    }
}