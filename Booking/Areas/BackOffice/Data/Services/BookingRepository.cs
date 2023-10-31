using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;
using Booking.DBEngine;
using Booking.Models;
using Dapper;
using System.Data;

namespace Booking.Areas.BackOffice.Data.Services
{
    public class BookingRepository : IBookingRepository
    {
        public readonly IDBHandler _dbHandler;

        /// <summary>
        /// Constructor to initialize the object
        /// </summary>
        public BookingRepository(IDBHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        /// <summary>
        /// To get the bookings
        /// </summary>
        /// <returns></returns>
        public async Task<List<BookingDTO>> GetBookings()
        {
            List<BookingDTO> listBooking = new List<BookingDTO>();
          
            try
            {
                using (_dbHandler.Connection)
                {
                    listBooking = (await _dbHandler.QueryAsync<BookingDTO>(_dbHandler.Connection, "[dbo].[GetBookings]", CommandType.StoredProcedure, null)).ToList();
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }


            return listBooking;
        }

    }
}