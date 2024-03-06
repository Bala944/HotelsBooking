using Booking.Areas.FrontOffice.Data.Interface;
using Booking.Areas.FrontOffice.Models.Input;
using Booking.Areas.FrontOffice.Models.Output;
using Booking.Data;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Razorpay.Api;
using System.Data;
using System.Security.Cryptography.Xml;

namespace Booking.Areas.FrontOffice.Controllers
{
    [Area("FrontOffice")]
    public class BookMyRoomController : Controller
    {
        private readonly IBookMyRoomRepository _bookMyRoomRepository;
        private readonly IMailing _mailing;

        public BookMyRoomController(IBookMyRoomRepository bookMyRoomRepository, IMailing mailing)
        {
            _bookMyRoomRepository = bookMyRoomRepository;
            _mailing = mailing;
        }


        public IActionResult HomeNew(RoomFilterDTO roomFilterDTO)
        {
            BookMyRoomResultDTO bookMyRoomResultDTO = new BookMyRoomResultDTO();
            try
            {
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
                    bookingQueryDTO.RoomType = bookMyRoomResultDTO.roomFilterDTO.RoomType;
                    string json = JsonConvert.SerializeObject(bookingQueryDTO);
                    string paramsEncrypted = EncryptionHelper.Encrypt(json);

                    bookMyRoomResultDTO.roomFilterDTO.Params = paramsEncrypted;
                    if (roomFilterDTO.IsViewMore)
                    {
                        return RedirectToAction("ViewMoreRooms", bookMyRoomResultDTO.roomFilterDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }

            return View(bookMyRoomResultDTO);
        }
        [Route("/view-more-rooms-new")]
        public IActionResult ViewMoreRoomsNew()
        {
            return View();
        }
        [Route("/view-rooms-details")]
        public IActionResult ViewRoomDetails()
        {
            return View();
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
            try
            {
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
                    bookingQueryDTO.RoomType = bookMyRoomResultDTO.roomFilterDTO.RoomType;
                    string json = JsonConvert.SerializeObject(bookingQueryDTO);
                    string paramsEncrypted = EncryptionHelper.Encrypt(json);

                    bookMyRoomResultDTO.roomFilterDTO.Params = paramsEncrypted;
                    if (roomFilterDTO.IsViewMore)
                    {
                        return RedirectToAction("ViewMoreRooms", bookMyRoomResultDTO.roomFilterDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }

            return View(bookMyRoomResultDTO);

        }

        /// <summary>
        /// Step 2 - View all the available room to choose and book
        /// </summary>
        /// <param name="selectedRoomDTO"></param>
        /// <returns></returns>
        [Route("/view-room")]
        public async Task<IActionResult> ViewMoreRooms(RoomFilterDTO roomFilterDTO)
        {
            BookMyRoomResultDTO bookMyRoomResultDTO = new BookMyRoomResultDTO();
            BookingQueryDTO bookingQueryDTO1 = new BookingQueryDTO();
            RoomandEventDetailsDTO roomandeventdetails = new RoomandEventDetailsDTO();

            if (!string.IsNullOrEmpty(roomFilterDTO.Params))
            {

                try
                {
                    string decryptedData = EncryptionHelper.Decrypt(roomFilterDTO.Params);
                    bookingQueryDTO1 = JsonConvert.DeserializeObject<BookingQueryDTO>(decryptedData);

                    roomFilterDTO.CheckInDate = bookingQueryDTO1.CheckInDate;
                    roomFilterDTO.CheckOutDate = bookingQueryDTO1.CheckOutDate;
                    roomFilterDTO.Adults = bookingQueryDTO1.Adults;
                    roomFilterDTO.Children = bookingQueryDTO1.Children;
                    roomFilterDTO.RoomType = bookingQueryDTO1.RoomType;

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

                roomandeventdetails = await _bookMyRoomRepository.GetRoomsanEvents(roomFilterDTO);


                if (roomandeventdetails != null)
                {
                    roomandeventdetails.RoomDetails = roomandeventdetails.RoomDetails;
                    roomandeventdetails.events = roomandeventdetails.events;
                }

                roomandeventdetails.roomFilterDTO = roomFilterDTO;

                BookingQueryDTO bookingQueryDTO = new BookingQueryDTO();
                bookingQueryDTO.CheckInDate = roomandeventdetails.roomFilterDTO.CheckInDate;
                bookingQueryDTO.CheckOutDate = roomandeventdetails.roomFilterDTO.CheckOutDate;
                bookingQueryDTO.Adults = roomandeventdetails.roomFilterDTO.Adults;
                bookingQueryDTO.Children = roomandeventdetails.roomFilterDTO.Children;
                bookingQueryDTO.Rooms = roomandeventdetails.roomFilterDTO.Rooms;
                bookingQueryDTO.RoomType = roomandeventdetails.roomFilterDTO.RoomType;
                string json = JsonConvert.SerializeObject(bookingQueryDTO);
                string paramsEncrypted = EncryptionHelper.Encrypt(json);

                roomandeventdetails.roomFilterDTO.Params = paramsEncrypted;
                ViewBag.FParams = paramsEncrypted;
            }
            return View(roomandeventdetails);
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
            roomDetails.roomDetails = await _bookMyRoomRepository.GetRoomsById(bookingQueryDTO);
            return View(roomDetails);
        }

        /// <summary>
        /// To make encrypt data for booKing 
        /// </summary>
        /// <param name="finalBookingDetails"></param>
        /// <returns></returns>
        [Route("/process-data")]
        [HttpPost]
        public async Task<IActionResult> ProcessData([FromBody] FinalBookingDetailsDTO finalBookingDetails)
        {
            string paramsEncrypted = string.Empty;
            FinalConfirmationData finalConfirmationData = new FinalConfirmationData();
            BookingQueryDTO bookingQueryDTO = new BookingQueryDTO();

            try
            {
                if (!string.IsNullOrEmpty(finalBookingDetails.FilterParams))
                {
                    string decryptedData2 = EncryptionHelper.Decrypt(finalBookingDetails.FilterParams);
                    if (!string.IsNullOrEmpty(decryptedData2))
                    {
                        bookingQueryDTO = JsonConvert.DeserializeObject<BookingQueryDTO>(decryptedData2);
                    }
                }


                BookingSelectedDTO bookingSelectedDTO = new BookingSelectedDTO();
                bookingSelectedDTO.RoomId = finalBookingDetails.finalBookingDetails[0].RoomId;
                bookingSelectedDTO.Count = (long)finalBookingDetails.finalBookingDetails[0].Count;
                bookingSelectedDTO.StartDate = bookingQueryDTO.CheckInDate;
                bookingSelectedDTO.EndDate = bookingQueryDTO.CheckOutDate;
                bookingSelectedDTO.EventIds = finalBookingDetails.Events;


                finalConfirmationData = await _bookMyRoomRepository.GetRoomConfirmationDetails(bookingSelectedDTO);

                // Return the view with the processed data
                string json = JsonConvert.SerializeObject(finalConfirmationData); // Corrected method name
                paramsEncrypted = EncryptionHelper.Encrypt(json);
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }
            return Ok(paramsEncrypted);
        }

        /// <summary>
        /// Step 3 - View the Registration page and Allow user to Pay
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="FilterParams"></param>
        /// <returns></returns>
        [Route("/register")]
        public async Task<IActionResult> RegisterCustomer(string BParams, string FilterParams)
        {
            new ErrorLog().WriteLog("Register");
            BookingRegistrationDTO result = new BookingRegistrationDTO();
            decimal TotalAmount = 0;
            try
            {
                if (!string.IsNullOrEmpty(BParams))
                {
                    string decryptedData = EncryptionHelper.Decrypt(BParams);
                    if (!string.IsNullOrEmpty(decryptedData))
                    {
                        result.finalBookingDetails = JsonConvert.DeserializeObject<FinalConfirmationData>(decryptedData);
                        TotalAmount = (decimal)result.finalBookingDetails.roomConfirmationDetailsDTO.Sum(bd => bd.TotalAmount);
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
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }


            ViewBag.BParams = BParams;
            ViewBag.FParams = FilterParams;



            return View(result);
        }

        /// <summary>
        /// Confirm Booking
        /// </summary>
        /// <param name="customerAndBookingDetails"></param>
        /// <returns></returns>
        [Route("/confirmBooking")]
        public async Task<IActionResult> ConfirmBooking(CustomerAndBookingDetails customerAndBookingDetails)
        {

            FinalConfirmationData bookingDetailsDTO = new FinalConfirmationData();

            try
            {
                if (customerAndBookingDetails != null)
                {

                    string decryptedData = EncryptionHelper.Decrypt(customerAndBookingDetails.BookingParams);

                    if (!string.IsNullOrEmpty(decryptedData))
                    {
                        bookingDetailsDTO = JsonConvert.DeserializeObject<FinalConfirmationData>(decryptedData);


                        if (bookingDetailsDTO != null && bookingDetailsDTO.roomConfirmationDetailsDTO.Count > 0 && customerAndBookingDetails != null)
                        {
                            RegistrationDetails registrationDetails = new RegistrationDetails
                            {
                                CheckIn = customerAndBookingDetails.CheckIn,
                                CheckOut = customerAndBookingDetails.CheckOut,
                                FirstName = customerAndBookingDetails.FirstName,
                                LastName = customerAndBookingDetails.LastName,
                                MobileNumber = customerAndBookingDetails.MobileNumber,
                                EmailAddress = customerAndBookingDetails.EmailAddress,
                                TotalAmount = ((decimal)bookingDetailsDTO.roomConfirmationDetailsDTO.Sum(bd => bd.Amount) + (decimal)bookingDetailsDTO.eventConfirmationDetailsDTO.Sum(bd => bd.Amount)- (decimal)bookingDetailsDTO.roomConfirmationDetailsDTO.Sum(bd => bd.DiscountAmount)),
                                DiscountId = (long)bookingDetailsDTO.roomConfirmationDetailsDTO.Sum(bd => bd.DiscountId),
                                DiscountAmount = (long)bookingDetailsDTO.roomConfirmationDetailsDTO.Sum(bd => bd.DiscountAmount),
                                TotalCount = (int)bookingDetailsDTO.roomConfirmationDetailsDTO.Sum(bd => bd.Count),
                                RoomId = string.Join("$", bookingDetailsDTO.roomConfirmationDetailsDTO.Select(bd => bd.RoomId)),
                                Count = string.Join("$", bookingDetailsDTO.roomConfirmationDetailsDTO.Select(bd => bd.Count)),
                                Amount = string.Join("$", bookingDetailsDTO.roomConfirmationDetailsDTO.Select(bd => bd.Amount))
                            };

                            if (bookingDetailsDTO.eventConfirmationDetailsDTO != null)
                            {
                                registrationDetails.EventId = string.Join("$", bookingDetailsDTO.eventConfirmationDetailsDTO.Select(bd => bd.EventId));

                                registrationDetails.EventCount = string.Join("$", bookingDetailsDTO.eventConfirmationDetailsDTO.Select(bd => bd.Count));

                                registrationDetails.EventAmount = string.Join("$", bookingDetailsDTO.eventConfirmationDetailsDTO.Select(bd => bd.Amount));
                            }

                            var result = await _bookMyRoomRepository.ConfirmBooking(registrationDetails);

                            OrderDTO orderDTO = new OrderDTO();
                            if (result == "-99")
                            {
                                orderDTO.BookingStatus = "-99";
                            }
                            else if (result != null)
                            {
                                orderDTO.BookingStatus = "200";
                                orderDTO.BookingOrderId = result;

                                List<MailDetailsDTO> mailDetails = new List<MailDetailsDTO>();
                                mailDetails = await _mailing.GetMailDetails(1);

                                MailingService mailingService = new MailingService();

                                for (int i = 0; i < mailDetails.Count; i++)
                                {
                                    string Booking = "<table style='width:100%;'>";
                                    decimal? totalAmount = 0;

                                    for (int j = 0; j < bookingDetailsDTO.roomConfirmationDetailsDTO.Count; j++)
                                    {
                                        Booking += $"<tr><td style='padding: 8px;'>{bookingDetailsDTO.roomConfirmationDetailsDTO[j].Name}</td><td style='padding: 8px;'>X {bookingDetailsDTO.roomConfirmationDetailsDTO[j].Count}</td><td style='padding: 8px;'>   {bookingDetailsDTO.roomConfirmationDetailsDTO[j].Amount}</td></tr>";
                                        totalAmount += bookingDetailsDTO.roomConfirmationDetailsDTO[j].Amount;
                                    }

                                    if (bookingDetailsDTO.eventConfirmationDetailsDTO != null)
                                    {
                                        for (int j = 0; j < bookingDetailsDTO.eventConfirmationDetailsDTO.Count; j++)
                                        {
                                            Booking += $"<tr><td style='padding: 8px;'>             {bookingDetailsDTO.eventConfirmationDetailsDTO[j].Name}</td><td style='padding: 8px;'></td><td style='padding: 8px;'>   {bookingDetailsDTO.eventConfirmationDetailsDTO[j].Amount}</td></tr>";
                                            totalAmount += bookingDetailsDTO.eventConfirmationDetailsDTO[j].Amount;
                                        }
                                    }

                                    Booking += $"<tfoot><tr><td colspan='2' style='text-align:right;padding: 8px;'>Total Amount:</td><td style='padding: 8px;'>{totalAmount}</td></tr></tfoot>";
                                    Booking += "</table>";
                                    string formattedHtmlContent = string.Empty;
                                    string Email = null;
                                    var image = "<img src=\"  \\\\aaralogo.png\" width=\"100\" />";
                                    if (mailDetails[i].MailType == 1)
                                    {
                                         formattedHtmlContent = string.Format(mailDetails[i].Content, image,DateTime.Now, result, Booking,string.Concat(customerAndBookingDetails.FirstName, customerAndBookingDetails.LastName,"<br/>", customerAndBookingDetails.MobileNumber));
                                         Email = customerAndBookingDetails.EmailAddress;
                                    }
                                    else
                                    {
                                        formattedHtmlContent = string.Format(mailDetails[i].Content, image, DateTime.Now, result, Booking, string.Concat(customerAndBookingDetails.FirstName, customerAndBookingDetails.LastName, "<br/>", customerAndBookingDetails.MobileNumber));

                                    }

                                    mailingService.SendEmail(mailDetails[i].Subject, formattedHtmlContent, Email);
                                }
                            }
                            return RedirectToAction("ConfirmedBookingStatus", new { BBparams = EncryptionHelper.Encrypt(JsonConvert.SerializeObject(orderDTO)) });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }


            return RedirectToAction("Index", "Home");
        }

        [Route("/booking-us")]
        public IActionResult ConfirmedBookingStatus(string BBparams)
        {
            try
            {
                OrderDTO bookingDetailsDTO = new OrderDTO();

                if (BBparams != null)
                {
                    string status = EncryptionHelper.Decrypt(BBparams);

                    if (!string.IsNullOrEmpty(status))
                    {

                        bookingDetailsDTO = JsonConvert.DeserializeObject<OrderDTO>(status);

                    }

                    ViewBag.BookingStatus = bookingDetailsDTO.BookingStatus;
                    ViewBag.BookingOrderId = bookingDetailsDTO.BookingOrderId;
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
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


        [Route("/Create-order")]
        public async Task<IActionResult> CreatePaymentOrder([FromBody] CustomerAndBookingDetails customerAndBookingDetails)
        {
            string result = string.Empty;
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
                        TotalAmount = (decimal)bookingDetailsDTO?.finalBookingDetails?.Sum(bd => bd?.Amount),
                        TotalCount = (int)bookingDetailsDTO?.finalBookingDetails?.Sum(bd => bd?.Count),
                        RoomId = string.Join("$", bookingDetailsDTO.finalBookingDetails.Select(bd => bd.RoomId)),
                        Count = string.Join("$", bookingDetailsDTO.finalBookingDetails.Select(bd => bd.Count)),
                        Amount = string.Join("$", bookingDetailsDTO.finalBookingDetails.Select(bd => bd.Amount))
                    };

                    result = await _bookMyRoomRepository.ConfirmBooking(registrationDetails);


                }
            }

            return Ok(result);
        }

        [Route("/event-details")]
        public async Task<IActionResult> EventDetails(string EventId)
        {
            long Event = 1;
            EventDTO eventDTO = new EventDTO();

            try
            {
                long.TryParse(EventId, out Event);
             

                eventDTO = await _bookMyRoomRepository.GetEventDetailsById(Event);
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }
            return View(eventDTO);
        }


        [Route("/feedback")]
        public  IActionResult FeedBack(string BookingId)
        {
         

            try
            {
               ViewBag.BookId = BookingId;
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }

            return View();
        }

        [Route("/save-feedback")]
        [HttpPost]
        public async Task<IActionResult> SaveFeedback([FromBody] Feedback feedback)
        {
            int result = 0;
            if(!string.IsNullOrEmpty(feedback.BookId))
            {
                feedback.BookingID = Convert.ToInt64(EncryptionHelper.Decrypt(feedback.BookId));

            }
            else
            {
                feedback.BookingID = 1;

            }

            try
            {
                result = await _bookMyRoomRepository.SaveFeedback(feedback);
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }
            return Ok(result);
        }
    }
}