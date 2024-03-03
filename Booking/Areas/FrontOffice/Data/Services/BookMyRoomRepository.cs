using Booking.Areas.FrontOffice.Data.Interface;
using Booking.Areas.FrontOffice.Models.Input;
using Booking.DBEngine;
using Booking.Models;
using Dapper;
using System.Data;

namespace Booking.Areas.FrontOffice.Data.Services
{
    public class BookMyRoomRepository : IBookMyRoomRepository
    {
        public readonly IDBHandler _dbHandler;

        /// <summary>
        /// Constructor to initialize the object
        /// </summary>
        public BookMyRoomRepository(IDBHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task<List<RoomsDetailsDTO>> GetRooms(RoomFilterDTO roomFilterDTO)
        {
            List<RoomsDetailsDTO> roomsList = new List<RoomsDetailsDTO>();
            var parameters = new DynamicParameters();
            try
            {
                parameters.Add("CheckInDate",ConversionHelper.ToSQLlDatetime(roomFilterDTO.CheckInDate), DbType.String, ParameterDirection.Input);
                parameters.Add("CheckOutDate", ConversionHelper.ToSQLlDatetime(roomFilterDTO.CheckOutDate), DbType.String, ParameterDirection.Input);
                parameters.Add("Adults", roomFilterDTO.Adults, DbType.Int16, ParameterDirection.Input);
                parameters.Add("Children", roomFilterDTO.Children, DbType.Int16, ParameterDirection.Input);
                parameters.Add("Rooms", roomFilterDTO.Rooms, DbType.Int16, ParameterDirection.Input);
                parameters.Add("RoomType", roomFilterDTO.RoomType, DbType.Int16, ParameterDirection.Input);
                using (_dbHandler.Connection)
                {
                    roomsList = (await _dbHandler.QueryAsync<RoomsDetailsDTO>(_dbHandler.Connection, "dbo.GetFilteredRoomsDetails", CommandType.StoredProcedure, parameters)).AsList();
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }


            return roomsList;
        }

        public async Task<RoomsDetailsDTO> GetRoomsById(BookingQueryDTO  bookingQueryDTO)
        {
            RoomsDetailsDTO roomsList = new RoomsDetailsDTO();
            var parameters = new DynamicParameters();
            try
            {
                parameters.Add("RoomId", bookingQueryDTO.SelectedRoomId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("CheckInDate", ConversionHelper.ToSQLlDatetime(bookingQueryDTO.CheckInDate), DbType.DateTime, ParameterDirection.Input);
                parameters.Add("CheckOutDate", ConversionHelper.ToSQLlDatetime(bookingQueryDTO.CheckOutDate), DbType.DateTime, ParameterDirection.Input);

                using (_dbHandler.Connection)
                {
                    roomsList = await _dbHandler.QuerySingleAsync<RoomsDetailsDTO>(_dbHandler.Connection, "dbo.GetFilteredRoomsDetailsById", CommandType.StoredProcedure, parameters);
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }


            return roomsList;
        }

        /// <summary>
        /// Confirm Booking 
        /// </summary>
        /// <param name="registrationDetails"></param>
        /// <returns></returns>
        public async Task<string> ConfirmBooking(RegistrationDetails registrationDetails)
        {
            var parameters = new DynamicParameters();
            string result = string.Empty;
            try
            {
                if (registrationDetails != null)
                {
                    parameters.Add("CheckIn", ConversionHelper.ToSQLlDatetime(registrationDetails?.CheckIn), DbType.DateTime, ParameterDirection.Input);
                    parameters.Add("CheckOut", ConversionHelper.ToSQLlDatetime(registrationDetails?.CheckOut), DbType.DateTime, ParameterDirection.Input);
                    parameters.Add("FirstName", registrationDetails?.FirstName, DbType.String, ParameterDirection.Input);
                    parameters.Add("LastName", registrationDetails?.LastName, DbType.String, ParameterDirection.Input);
                    parameters.Add("Email", registrationDetails?.EmailAddress, DbType.String, ParameterDirection.Input);
                    parameters.Add("MobileNumber", registrationDetails?.MobileNumber, DbType.String, ParameterDirection.Input);
                    parameters.Add("TotalAmount", registrationDetails?.TotalAmount, DbType.Decimal, ParameterDirection.Input);
                    parameters.Add("TotalCount", registrationDetails?.TotalCount, DbType.Int16, ParameterDirection.Input);
                    parameters.Add("RoomIds", registrationDetails?.RoomId, DbType.String, ParameterDirection.Input);
                    parameters.Add("Counts", registrationDetails?.Count, DbType.String, ParameterDirection.Input);
                    parameters.Add("Amounts", registrationDetails?.Amount, DbType.String, ParameterDirection.Input);
                    parameters.Add("DiscountAmount", registrationDetails?.DiscountAmount, DbType.Decimal, ParameterDirection.Input);
                    parameters.Add("DiscountId", registrationDetails?.DiscountId, DbType.Int64, ParameterDirection.Input);

                    parameters.Add("EventIds", registrationDetails?.EventId, DbType.String, ParameterDirection.Input);
                    parameters.Add("EventCounts", registrationDetails?.EventCount, DbType.String, ParameterDirection.Input);
                    parameters.Add("EventAmounts", registrationDetails?.EventAmount, DbType.String, ParameterDirection.Input);


                    using (_dbHandler.Connection)
                    {
                        result = await _dbHandler.ExecuteScalarAsync<string>(_dbHandler.Connection, "dbo.InsertBookingDetails", CommandType.StoredProcedure, parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }


            return result;
        }

		public async Task<FinalConfirmationData> GetRoomConfirmationDetails(BookingSelectedDTO bookingSelectedDTO)
		{
			FinalConfirmationData finalConfirmationData = new FinalConfirmationData();

			var parameters = new DynamicParameters();
			parameters.Add("RoomId", bookingSelectedDTO.RoomId, DbType.String, ParameterDirection.Input);
			parameters.Add("Count", bookingSelectedDTO.Count, DbType.String, ParameterDirection.Input);
			parameters.Add("EventIds", bookingSelectedDTO.EventIds, DbType.String, ParameterDirection.Input);
			parameters.Add("StartDate", bookingSelectedDTO.StartDate, DbType.String, ParameterDirection.Input);
			parameters.Add("EndDate", bookingSelectedDTO.EndDate, DbType.String, ParameterDirection.Input);

			try
			{
				using (_dbHandler.Connection)
				{
					var multiResult = await _dbHandler.QueryMultipleAsync(_dbHandler.Connection, "dbo.FetchConfirmationDetails", CommandType.StoredProcedure, parameters);
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

        /// <summary>
        /// To get the bookings
        /// </summary>
        /// <returns></returns>
        public async Task<EventDTO> GetEventDetailsById(long EventId)
        {
            EventDTO events = new EventDTO();

            var Parameters = new DynamicParameters();
            try
            {
                Parameters.Add("EventId", EventId, DbType.Int64, ParameterDirection.Input);

                using (_dbHandler.Connection)
                {
                    events = await _dbHandler.QuerySingleAsync<EventDTO>(_dbHandler.Connection, "[dbo].[GetEventDetailsById]", CommandType.StoredProcedure, Parameters);
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }


            return events;
        }
    }
}