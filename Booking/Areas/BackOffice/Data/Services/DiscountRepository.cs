using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Models.Output;
using Booking.DBEngine;
using Booking.Models;
using Dapper;
using System.Data;

namespace Booking.Areas.BackOffice.Data.Services
{
    public class DiscountRepository : IDiscountRepository
    {
        public readonly IDBHandler _dbHandler;

        /// <summary>
        /// Constructor to initialize the object
        /// </summary>
        public DiscountRepository(IDBHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        /// <summary>
        /// To get the rooms
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrderDiscount>> GetDiscount()
        {
            List<OrderDiscount> discount = new List<OrderDiscount>();
            var parameters = new DynamicParameters();
            try
            {
                parameters.Add("ActionId", 1, DbType.Int64, ParameterDirection.Input);
                using (_dbHandler.Connection)
                {
                    discount = (await _dbHandler.QueryAsync<OrderDiscount>(_dbHandler.Connection, "[dbo].[ManageDiscountDetails]", CommandType.StoredProcedure, parameters)).ToList();
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }


            return discount;
        }

        public async Task<OrderDiscount> GetDiscountDetailsById(Int64 DiscountId)
        {
            OrderDiscount discount = new OrderDiscount();
            var parameters = new DynamicParameters();
            try
            {
                parameters.Add("ActionId", 3, DbType.Int64, ParameterDirection.Input);
                parameters.Add("DiscountId", DiscountId, DbType.Int64, ParameterDirection.Input);
                using (_dbHandler.Connection)
                {
                    discount = await _dbHandler.QuerySingleAsync<OrderDiscount>(_dbHandler.Connection, "dbo.ManageDiscountDetails", CommandType.StoredProcedure, parameters);
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }


            return discount;
        }

        /// <summary>
        /// To get the rooms
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoomDTO>> GetAddDiscountDetails()
        {
            List<RoomDTO> RoomDetails = new List<RoomDTO>();

            try
            {
                using (_dbHandler.Connection)
                {
                     RoomDetails = (await _dbHandler.QueryAsync<RoomDTO>(_dbHandler.Connection, "dbo.FetchRooms", CommandType.StoredProcedure, null)).ToList();
                   
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }


            return RoomDetails;
        }

        public async Task<int> SaveDiscount(OrderDiscount orderDiscount)
        {
            int result = 0;
            var parameters = new DynamicParameters();
            try
            {
                parameters.Add("ActionId", 2, DbType.Int64, ParameterDirection.Input);
                parameters.Add("RoomId", orderDiscount.RoomId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("DiscountID", orderDiscount.DiscountID, DbType.Int64, ParameterDirection.Input);
                parameters.Add("DiscountPercentage", orderDiscount.DiscountPercentage, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("StartDate", !string.IsNullOrEmpty(orderDiscount.StartDate)? ConversionHelper.ToSQLlDatetime(orderDiscount.StartDate):DateTime.Now, DbType.DateTime, ParameterDirection.Input);
                parameters.Add("ExpirationDate", ConversionHelper.ToSQLlDatetime(orderDiscount.ExpirationDate), DbType.DateTime, ParameterDirection.Input);
               
                using (_dbHandler.Connection)
                {
                    result = await _dbHandler.ExecuteScalarAsync<int>(_dbHandler.Connection, "[dbo].[ManageDiscountDetails]", CommandType.StoredProcedure, parameters);
                }
            }
            catch (Exception ex)
            {
                new ErrorLog().WriteLog(ex);
            }


            return result;
        }

        public async Task<int> DeleteDiscountDetailsById(Int64 DiscountId)
        {
            int result = 0;
            var parameters = new DynamicParameters();
            try
            {
                parameters.Add("ActionId", 4, DbType.Int64, ParameterDirection.Input);
                parameters.Add("DiscountId", DiscountId, DbType.Int64, ParameterDirection.Input);

                using (_dbHandler.Connection)
                {
                    result = await _dbHandler.ExecuteScalarAsync<int>(_dbHandler.Connection, "dbo.ManageDiscountDetails", CommandType.StoredProcedure, parameters);
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