using Booking.Areas.FrontOffice.Data.Interface;
using Booking.Areas.FrontOffice.Models.Input;
using Booking.DBEngine;
using Dapper;
using System.Collections.Generic;
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
                parameters.Add("CheckInDate", roomFilterDTO.CheckInDate, DbType.DateTime, ParameterDirection.Input);
                parameters.Add("CheckOutDate", roomFilterDTO.CheckOutDate, DbType.DateTime, ParameterDirection.Input);
                parameters.Add("Adults", roomFilterDTO.Adults, DbType.Int16, ParameterDirection.Input);
                parameters.Add("Children", roomFilterDTO.Children, DbType.Int16, ParameterDirection.Input);
                parameters.Add("Rooms", roomFilterDTO.Rooms, DbType.Int16, ParameterDirection.Input);
                using (_dbHandler.Connection)
                {
                    roomsList = (await _dbHandler.QueryAsync<RoomsDetailsDTO>(_dbHandler.Connection, "dbo.GetFilteredRoomsDetails", CommandType.StoredProcedure, parameters)).AsList();
                }
            }
            catch (Exception)
            {
                //new ErrorLog().WriteLog(ex);
            }

            return roomsList;
        }
        
        public async Task<RoomsDetailsDTO> GetRoomsById(Int64 roomId)
        {
            RoomsDetailsDTO roomsList = new RoomsDetailsDTO();
            var parameters = new DynamicParameters();
            try
            {
                parameters.Add("RoomId", roomId, DbType.DateTime, ParameterDirection.Input);
                using (_dbHandler.Connection)
                {
                    roomsList = await _dbHandler.QuerySingleAsync<RoomsDetailsDTO>(_dbHandler.Connection, "dbo.GetFilteredRoomsDetailsById", CommandType.StoredProcedure, parameters);
                }
            }
            catch (Exception)
            {
                //new ErrorLog().WriteLog(ex);
            }

            return roomsList;
        }
    }
}
