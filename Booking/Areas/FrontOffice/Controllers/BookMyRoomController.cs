using Booking.Areas.FrontOffice.Data.Interface;
using Booking.Areas.FrontOffice.Models.Input;
using Booking.Areas.FrontOffice.Models.Output;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Newtonsoft.Json;
using Razorpay.Api;
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
        public IActionResult ViewMoreRooms(RoomFilterDTO roomFilterDTO)
        {
            BookMyRoomResultDTO bookMyRoomResultDTO = new BookMyRoomResultDTO();
            BookingQueryDTO bookingQueryDTO1 = new BookingQueryDTO();
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
            string paramsEncrypted = string.Empty;
            try
            {
                // Return the view with the processed data
                string json = JsonConvert.SerializeObject(finalBookingDetails); // Corrected method name
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
        public IActionResult RegisterCustomer(string BParams, string FilterParams)
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
                        result.finalBookingDetails = JsonConvert.DeserializeObject<FinalBookingDetailsDTO>(decryptedData)?.finalBookingDetails;
                        TotalAmount = (decimal)result.finalBookingDetails.Sum(bd => bd.TotalAmount);
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

            //string Key = "rzp_test_CxRq0CbjDqDcpI";
            //string secret = "U96zupO4NVVgKbpnk0Ul19AI";

            //Random _random = new Random();
            //string TransactionId = _random.Next(0, 10000).ToString();

            //// Convert the amount to the smallest currency unit (e.g., paise for INR)

            //int amountInPaise = (int)(TotalAmount * 100);

            //Dictionary<string, object> input = new Dictionary<string, object>();
            //input.Add("amount", amountInPaise); // Use the converted amount
            //input.Add("currency", "INR");
            //input.Add("receipt", TransactionId);

            //RazorpayClient client = new RazorpayClient(Key, secret);
            //Razorpay.Api.Order order = client.Order.Create(input);
            //string OrderId = order["id"].ToString();
            ViewBag.OrderId = "test";

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


            try
            {
                if (customerAndBookingDetails != null)
                {

                    //Dictionary<string, string> attributes = new Dictionary<string, string>();
                    //attributes.Add("razorpay_payment_id", customerAndBookingDetails.paymentId);
                    //attributes.Add("razorpay_order_id", customerAndBookingDetails.Orderid);
                    //attributes.Add("razorpay_signature", customerAndBookingDetails.sign);

                    //Utils.verifyPaymentSignature(attributes);

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
                                TotalAmount = (decimal)bookingDetailsDTO.finalBookingDetails.Sum(bd => bd.TotalAmount),
                                RoomId = string.Join("$", bookingDetailsDTO.finalBookingDetails.Select(bd => bd.RoomId)),
                                Count = string.Join("$", bookingDetailsDTO.finalBookingDetails.Select(bd => bd.Count)),
                                Amount = string.Join("$", bookingDetailsDTO.finalBookingDetails.Select(bd => bd.Amount))
                            };

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
                        TotalAmount = (decimal)bookingDetailsDTO.finalBookingDetails.Sum(bd => bd.Amount),
                        RoomId = string.Join("$", bookingDetailsDTO.finalBookingDetails.Select(bd => bd.RoomId)),
                        Count = string.Join("$", bookingDetailsDTO.finalBookingDetails.Select(bd => bd.Count)),
                        Amount = string.Join("$", bookingDetailsDTO.finalBookingDetails.Select(bd => bd.Amount))
                    };

                    result = await _bookMyRoomRepository.ConfirmBooking(registrationDetails);


                }
            }

            return Ok(result);
        }


    }
}