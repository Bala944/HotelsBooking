using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Models;
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
                    rooms = (await _dbHandler.QueryAsync<RoomsDTO>(_dbHandler.Connection, "[dbo].[FetchRooms]", CommandType.StoredProcedure, null)).ToList();
                }
            }
            catch (Exception)
            {
                //new ErrorLog().WriteLog(ex);
            }

            return rooms;
        }
    }
}