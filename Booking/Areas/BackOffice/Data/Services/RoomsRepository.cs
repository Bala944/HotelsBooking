using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;
using Booking.DBEngine;
using Dapper;
using System.Data;

namespace Booking.Areas.BackOffice.Data.Services
{
    public class RoomsRepository : IRoomsRepository
    {
        public readonly IDBHandler _dbHandler;

        /// <summary>
        /// Constructor to initialize the object
        /// </summary>
        public RoomsRepository(IDBHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        /// <summary>
        /// To get the rooms
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoomsDTO>> GetRooms()
        {
            List<RoomsDTO> rooms = new List<RoomsDTO>();

            try
            {
                using (_dbHandler.Connection)
                {
                    rooms = (await _dbHandler.QueryAsync<RoomsDTO>(_dbHandler.Connection, "dbo.FetchRooms", CommandType.StoredProcedure, null)).ToList();
                }
            }
            catch (Exception)
            {
                //new ErrorLog().WriteLog(ex);
            }

            return rooms;
        }

        /// <summary>
        /// To get the rooms
        /// </summary>
        /// <returns></returns>
        public async Task<AddRoomDTO> GetAddRoomDetails()
        {
            AddRoomDTO addRoomDetails = new AddRoomDTO();

            try
            {
                using (_dbHandler.Connection)
                {
                    var multiResult = await _dbHandler.QueryMultipleAsync(_dbHandler.Connection, "dbo.FetchRoomAddData", CommandType.StoredProcedure, null);
                    if (multiResult != null)
                    {
                        addRoomDetails.roomTypeModel = (await multiResult.ReadAsync<RoomTypeModel>()).ToList();
                        addRoomDetails.bedTypeModel = (await multiResult.ReadAsync<BedTypeModel>()).ToList();
                    }
                }
            }
            catch (Exception)
            {
                //new ErrorLog().WriteLog(ex);
            }

            return addRoomDetails;
        }
        
        public async Task<int> SaveRoomDetails(RoomsDetailsDTO roomsDetailsDTO)
        {
            int result = 0;
            var parameters = new DynamicParameters();
            try
            {
                parameters.Add("@RoomId", roomsDetailsDTO.RoomId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@RoomTypeId", roomsDetailsDTO.RoomTypeId, DbType.Int16, ParameterDirection.Input);
                parameters.Add("@BedTypeId", roomsDetailsDTO.BedTypeId, DbType.Int16, ParameterDirection.Input);
                parameters.Add("@Number", roomsDetailsDTO.RoomNumber, DbType.String, ParameterDirection.Input);
                parameters.Add("@Price", roomsDetailsDTO.Price, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("@Description", roomsDetailsDTO.Description, DbType.String, ParameterDirection.Input);
                parameters.Add("@Status", roomsDetailsDTO.Status, DbType.Int16, ParameterDirection.Input);
                parameters.Add("@IsActive", roomsDetailsDTO.IsActive, DbType.Boolean, ParameterDirection.Input);

                using (_dbHandler.Connection)
                {
                    result = await _dbHandler.ExecuteScalarAsync<int>(_dbHandler.Connection, "dbo.FetchRooms", CommandType.StoredProcedure, parameters);
                }
            }
            catch (Exception)
            {
                //new ErrorLog().WriteLog(ex);
            }

            return result;
        }
        public async Task<int> DeleteRoomDetails(int RoomId)
        {
            int result = 0;
            var parameters = new DynamicParameters();
            try
            {
                parameters.Add("@RoomId", RoomId, DbType.Int64, ParameterDirection.Input);
               
                using (_dbHandler.Connection)
                {
                    result = await _dbHandler.ExecuteScalarAsync<int>(_dbHandler.Connection, "dbo.FetchRooms", CommandType.StoredProcedure, parameters);
                }
            }
            catch (Exception)
            {
                //new ErrorLog().WriteLog(ex);
            }

            return result;
        }
    }
}