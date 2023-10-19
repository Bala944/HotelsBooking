using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;
using Booking.DBEngine;
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
    }
}