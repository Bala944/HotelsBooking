﻿using Booking.Areas.FrontOffice.Data.Interface;
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
    }
}