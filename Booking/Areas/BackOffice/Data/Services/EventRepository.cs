using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;
using Booking.DBEngine;
using Booking.Models;
using Dapper;
using System.Data;

namespace Booking.Areas.BackOffice.Data.Services
{
    public class EventRepository : IEventRepository
    {
        public readonly IDBHandler _dbHandler;

        /// <summary>
        /// Constructor to initialize the object
        /// </summary>
        public EventRepository(IDBHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        /// <summary>
        /// To get the rooms
        /// </summary>
        /// <returns></returns>
        public async Task<List<Event>> GetEvent()
        {
            List<Event> events = new List<Event>();
            var parameters = new DynamicParameters();
            try
            {
                parameters.Add("ActionId", 1, DbType.Int64, ParameterDirection.Input);
                using (_dbHandler.Connection)
                {
                    events = (await _dbHandler.QueryAsync<Event>(_dbHandler.Connection, "[dbo].[ManageEventDetails]", CommandType.StoredProcedure, parameters)).ToList();
                }
            }
            catch (Exception)
            {
                //new ErrorLog().WriteLog(ex);
            }

            return events;
        }

        public async Task<Event> GetEventDetailsById(Int64 EventId)
        {
            Event events = new Event();
            var parameters = new DynamicParameters();
            try
            {
                parameters.Add("ActionId", 3, DbType.Int64, ParameterDirection.Input);
                parameters.Add("EventId", EventId, DbType.Int64, ParameterDirection.Input);
                using (_dbHandler.Connection)
                {
                    events = await _dbHandler.QuerySingleAsync<Event>(_dbHandler.Connection, "dbo.ManageEventDetails", CommandType.StoredProcedure, parameters);
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }

            return events;
        }

       

        public async Task<int> SaveEventDetails(Event events)
        {
            int result = 0;
            var parameters = new DynamicParameters();
            try
            {
                parameters.Add("ActionId", 2, DbType.Int64, ParameterDirection.Input);
                parameters.Add("EventId", events.EventID, DbType.Int64, ParameterDirection.Input);
                parameters.Add("EventType", events.EventType, DbType.String, ParameterDirection.Input);
                parameters.Add("Description", events.Description, DbType.String, ParameterDirection.Input);
                parameters.Add("TotalSeats", events.TotalSeats, DbType.Int64, ParameterDirection.Input);
                parameters.Add("StartDate", ConversionHelper.ToSQLlDatetime(events.StartDate) , DbType.DateTime, ParameterDirection.Input);
                parameters.Add("Price", events.Price, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("EndDate", ConversionHelper.ToSQLlDatetime(events.EndDate), DbType.DateTime, ParameterDirection.Input);
                parameters.Add("Images", events.Images, DbType.String, ParameterDirection.Input);
            
                using (_dbHandler.Connection)
                {
                    result = await _dbHandler.ExecuteScalarAsync<int>(_dbHandler.Connection, "[dbo].[ManageEventDetails]", CommandType.StoredProcedure, parameters);
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }


            return result;
        }

        public async Task<int> DeleteEventDetails(Int64 EventId)
        {
            int result = 0;
            var parameters = new DynamicParameters();
            try
            {
                parameters.Add("ActionId", 4, DbType.Int64, ParameterDirection.Input);
                parameters.Add("EventId", EventId, DbType.Int64, ParameterDirection.Input);

                using (_dbHandler.Connection)
                {
                    result = await _dbHandler.ExecuteScalarAsync<int>(_dbHandler.Connection, "dbo.ManageEventDetails", CommandType.StoredProcedure, parameters);
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