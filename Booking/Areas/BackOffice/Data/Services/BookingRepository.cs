using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;
using Booking.Areas.FrontOffice.Models.Input;
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
        
        

        /// <summary>
        /// To Update bookings  Status 
        /// </summary>
        /// <returns></returns>
        public async Task<Int16> UpdateBookingStatus(BookingStatusDTO bookingStatusDTO)
        {

            Int16 result = 0;
          var Parameters = new DynamicParameters();
            try
            {

                Parameters.Add("BookingId",EncryptionHelper.Decrypt(bookingStatusDTO.BookingId), DbType.Int16, ParameterDirection.Input);
                Parameters.Add("BookingStatusId", bookingStatusDTO.BookingStatus, DbType.Int16, ParameterDirection.Input);

                using (_dbHandler.Connection)
                {
                    result = await _dbHandler.ExecuteScalarAsync<Int16>(_dbHandler.Connection, "[dbo].[ManageBookingStatus]", CommandType.StoredProcedure, Parameters);
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }


            return result;
        }
        public async Task<FinalConfirmationData> GetConfirmStatus(long BookingId)
        {
            FinalConfirmationData finalConfirmationData = new FinalConfirmationData();

            var parameters = new DynamicParameters();
            parameters.Add("BookingId", BookingId, DbType.Int64, ParameterDirection.Input);
           
            try
            {
                using (_dbHandler.Connection)
                {
                    var multiResult = await _dbHandler.QueryMultipleAsync(_dbHandler.Connection, "dbo.FetchBookingStatusConfirm", CommandType.StoredProcedure, parameters);
                    if (multiResult != null)
                    {
                        finalConfirmationData.roomConfirmationDetailsDTO = (await multiResult.ReadAsync<RoomConfirmationDetailsDTO>()).ToList();

                        finalConfirmationData.eventConfirmationDetailsDTO = (await multiResult.ReadAsync<EventConfirmationDetailsDTO>()).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }

            return finalConfirmationData;
        }
    }
}