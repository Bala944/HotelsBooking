using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Areas.BackOffice.Data.Interface
{
    public interface IDiscountRepository
    {
         Task<List<OrderDiscount>> GetDiscount();
         Task<OrderDiscount> GetDiscountDetailsById(Int64 DiscountId);
         Task<List<RoomDTO>> GetAddDiscountDetails();
        Task<int> SaveDiscount(OrderDiscount orderDiscount);
         Task<int> DeleteDiscountDetailsById(Int64 DiscountId);
    }
}
