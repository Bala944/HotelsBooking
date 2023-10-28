using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Models.Output;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
    [Authorize(Policy = Policies.RequireAdminClaim)]
    public class DiscountController : Controller
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        [Route("~/view-discount")]
        public async Task<IActionResult> ViewDiscount()
        {
            List<OrderDiscount> discountList = new List<OrderDiscount>();

            discountList = await _discountRepository.GetDiscount();

            return View(discountList);
        }
        [Route("~/add-discount")]
        public async Task<IActionResult> AddDiscount(Int64 DiscountId)
        {
            List<RoomDTO> rooms = new List<RoomDTO>();

            rooms = await _discountRepository.GetAddDiscountDetails();

            ViewBag.DiscountId = DiscountId;
            return View(rooms);
        }

        [Route("~/save-discount-details")]
        [HttpPost]
        public async Task<IActionResult> SaveDiscount([FromBody] OrderDiscount orderDiscount)
        {
            int result = await _discountRepository.SaveDiscount(orderDiscount);

            return Ok(result);
        }

        [Route("~/get-discount-details-byId")]
        [HttpGet]
        public async Task<IActionResult> GetDiscountDetailsById([FromQuery] Int64 DiscountId)
        {
            OrderDiscount orderDiscount = new OrderDiscount();

            orderDiscount = await _discountRepository.GetDiscountDetailsById(DiscountId);
            return Ok(orderDiscount);
        }

        [Route("~/delete-room-details")]
        [HttpGet]
        public async Task<IActionResult> DeleteDiscountDetailsById([FromQuery] Int64 DiscountId)
        {
            int result = await _discountRepository.DeleteDiscountDetailsById(DiscountId);
            return Ok(result);
        }
    }
}