using Booking.Areas.FrontOffice.Data.Interface;
using Booking.Areas.FrontOffice.Models.Input;
using Booking.Areas.FrontOffice.Models.Output;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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


        /// <summary>
        /// Step 1 - View the Home Page With Date Filter 
        /// </summary>
        /// <param name="roomFilterDTO"></param>
        /// <returns></returns>
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
                string json = JsonConvert.SerializeObject(bookingQueryDTO);
                string paramsEncrypted = EncryptionHelper.Encrypt(json);

                bookMyRoomResultDTO.roomFilterDTO.Params = paramsEncrypted;
                if (roomFilterDTO.IsViewMore)
                {
                    return RedirectToAction("ViewMoreRooms", bookMyRoomResultDTO.roomFilterDTO);
                }
            }

            return View(bookMyRoomResultDTO);

        }

        /// <summary>
        /// Step 2 - View all the available room to choose and book
        /// </summary>
        /// <param name="selectedRoomDTO"></param>
        /// <returns></returns>
        [Route("/view-room")]
        public IActionResult ViewMoreRooms(RoomFilterDTO roomFilterDTO)
        {
            BookMyRoomResultDTO bookMyRoomResultDTO = new BookMyRoomResultDTO();
            BookingQueryDTO bookingQueryDTO1 = new BookingQueryDTO();
            if (!string.IsNullOrEmpty(roomFilterDTO.Params ))
            {

                try
                {
                    string decryptedData = EncryptionHelper.Decrypt(roomFilterDTO.Params);
                    bookingQueryDTO1 = JsonConvert.DeserializeObject<BookingQueryDTO>(decryptedData);

                    roomFilterDTO.CheckInDate = bookingQueryDTO1.CheckInDate;
                    roomFilterDTO.CheckOutDate = bookingQueryDTO1.CheckOutDate;
                    roomFilterDTO.Adults = bookingQueryDTO1.Adults;
                    roomFilterDTO.Children = bookingQueryDTO1.Children;

                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Error parsing JSON: " + ex.Message);
                }
            }

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
                    string json = JsonConvert.SerializeObject(bookingQueryDTO);
                    string paramsEncrypted = EncryptionHelper.Encrypt(json);

                    bookMyRoomResultDTO.roomFilterDTO.Params = paramsEncrypted;
                    ViewBag.FParams = paramsEncrypted;
                }
            return View(bookMyRoomResultDTO);
        }

        /// <summary>
        /// To view the Room Details
        /// </summary>
        /// <param name="selectedRoomDTO"></param>
        /// <returns></returns>
        [Route("/room-details-page")]
        public async Task<IActionResult> SingleRoomDetails(SelectedRoomDTO selectedRoomDTO)
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

                    bookingQueryDTO.SelectedRoomId = selectedRoomDTO.RoomId;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Error parsing JSON: " + ex.Message);
                }
            }

            ViewBag.BParams = selectedRoomDTO.Params;
            roomDetails.bookingQueryData = bookingQueryDTO;
            roomDetails.roomDetails = await _bookMyRoomRepository.GetRoomsById(selectedRoomDTO.RoomId);
            return View(roomDetails);
        }

        /// <summary>
        /// To make encrypt data for booKing 
        /// </summary>
        /// <param name="finalBookingDetails"></param>
        /// <returns></returns>
        [Route("/process-data")]
        [HttpPost]
        public IActionResult ProcessData([FromBody] FinalBookingDetailsDTO finalBookingDetails)
        {
            // Return the view with the processed data
            string json = JsonConvert.SerializeObject(finalBookingDetails); // Corrected method name
            string paramsEncrypted = EncryptionHelper.Encrypt(json);

            return Ok(paramsEncrypted);
        }

        /// <summary>
        /// Step 3 - View the Registration page and Allow user to Pay
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="FilterParams"></param>
        /// <returns></returns>
        [Route("/register")]
        public IActionResult RegisterCustomerDetails(string BParams, string FilterParams)
        {
            BookingRegistrationDTO result = new BookingRegistrationDTO();

            try
            {
                if (!string.IsNullOrEmpty(BParams))
                {
                    string decryptedData = EncryptionHelper.Decrypt(BParams);
                    if (!string.IsNullOrEmpty(decryptedData))
                    {
                        result.finalBookingDetails = JsonConvert.DeserializeObject<FinalBookingDetailsDTO>(decryptedData)?.finalBookingDetails;
                    }
                }

                if (!string.IsNullOrEmpty(FilterParams))
                {
                    string decryptedData2 = EncryptionHelper.Decrypt(FilterParams);
                    if (!string.IsNullOrEmpty(decryptedData2))
                    {
                        result.bookingQueryDTO = JsonConvert.DeserializeObject<BookingQueryDTO>(decryptedData2);
                    }
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Error parsing JSON: " + ex.Message);
            }

            ViewBag.BParams = BParams;
            ViewBag.FParams = FilterParams;
            return View("RegisterCustomer", result);
        }

        /// <summary>
        /// Confirm Booking
        /// </summary>
        /// <param name="customerAndBookingDetails"></param>
        /// <returns></returns>
        [Route("/confirmBooking")]
        public async Task<IActionResult> ConfirmBooking(CustomerAndBookingDetails customerAndBookingDetails)
        {

            try
            {
                if (customerAndBookingDetails != null)
                {
                    string decryptedData = EncryptionHelper.Decrypt(customerAndBookingDetails.BookingParams);

                    if (!string.IsNullOrEmpty(decryptedData))
                    {
                        FinalBookingDetailsDTO bookingDetailsDTO = new FinalBookingDetailsDTO
                        {
                            finalBookingDetails = JsonConvert.DeserializeObject<FinalBookingDetailsDTO>(decryptedData).finalBookingDetails
                        };

                        if (bookingDetailsDTO != null && bookingDetailsDTO.finalBookingDetails.Count > 0 && customerAndBookingDetails != null)
                        {
                            RegistrationDetails registrationDetails = new RegistrationDetails
                            {
                                CheckIn = customerAndBookingDetails.CheckIn,
                                CheckOut = customerAndBookingDetails.CheckOut,
                                FirstName = customerAndBookingDetails.FirstName,
                                LastName = customerAndBookingDetails.LastName,
                                MobileNumber = customerAndBookingDetails.MobileNumber,
                                EmailAddress = customerAndBookingDetails.EmailAddress,
                                TotalAmount = (decimal)bookingDetailsDTO.finalBookingDetails.Sum(bd => bd.Amount),
                                RoomId = string.Join("$", bookingDetailsDTO.finalBookingDetails.Select(bd => bd.RoomId)),
                                Count = string.Join("$", bookingDetailsDTO.finalBookingDetails.Select(bd => bd.Count)),
                                Amount = string.Join("$", bookingDetailsDTO.finalBookingDetails.Select(bd => bd.Amount))
                            };

                            var result = await _bookMyRoomRepository.ConfirmBooking(registrationDetails);

                           return RedirectToAction("ConfirmedBookingStatus", "BookMyRoom", new {BBparams=EncryptionHelper.Encrypt("200")});
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return View();
        }

        [Route("/booking-us")]
        public IActionResult ConfirmedBookingStatus(string BBparams)
        {
            if(BBparams !=null)
            {
                var status =EncryptionHelper.Decrypt(BBparams);
                ViewBag.BookingStatus = status;
            }
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