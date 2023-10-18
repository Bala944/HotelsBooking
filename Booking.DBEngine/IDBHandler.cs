using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using static Dapper.SqlMapper;

namespace Booking.DBEngine
{
    public interface IDBHandler
    {
		IDbConnection Connection { get; }

		Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, CommandType commandType, object parameters = null);  // return the Single row Data table values

		Task<T> QuerySingleAsync<T>(IDbConnection connection, string sql, CommandType commandType, object parameters = null); // return the Data table values

		Task<T> ExecuteScalarAsync<T>(IDbConnection connection, string sql, CommandType commandType, object parameters = null);  // return the object values

		Task ExecuteAsync(IDbConnection connection, string sql, CommandType commandType, object parameters = null); // Insert, Update and Delete

		Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, CommandType commandType, object parameters = null);   // return the Single data table

		Task<GridReader> QueryMultipleAsync(IDbConnection connection, string sql, CommandType commandType, object parameters = null);  // return the Data Set  values

		Task<IDataReader> ExecuteReaderAsync(IDbConnection connection, string sql, CommandType commandType, object parameters = null);  // return the Data Set  values
	}

	public class DBHandler : IDBHandler
    {
		private readonly IConfiguration _configuration;

		public DBHandler(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public IDbConnection Connection
		{
			get
			{
				var sqlConnection = new SqlConnection(_configuration.GetConnectionString("BookingConString"));
				return sqlConnection;
			}
		}

		public async Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, CommandType commandType, object? parameters = null)
		{
			
				using (connection)
				{
					return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters, commandType: commandType);
				}
		}

		public async Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, CommandType commandType, object? parameters = null)
		{
			using (connection)
			{
				return await connection.QueryAsync<T>(sql, parameters, commandType: commandType, commandTimeout: 600);
			}
		}

		public async Task<T> QuerySingleAsync<T>(IDbConnection connection, string sql, CommandType commandType, object parameters = null)
		{
			using (connection)
			{
				return await connection.QuerySingleAsync<T>(sql, parameters, commandType: commandType);
			}
		}

		public async Task<T> ExecuteScalarAsync<T>(IDbConnection connection, string sql, CommandType commandType, object parameters = null)
		{
			using (connection)
			{
				return await connection.ExecuteScalarAsync<T>(sql, parameters, commandType: commandType);
			}
		}

		public async Task ExecuteAsync(IDbConnection connection, string sql, CommandType commandType, object? parameters = null)
		{
			using (connection)
			{
				await connection.ExecuteAsync(sql, parameters, commandType: commandType);
			}
		}

		public async Task<GridReader> QueryMultipleAsync(IDbConnection connection, string sql, CommandType commandType, object parameters = null)
		{
			return await connection.QueryMultipleAsync(sql, parameters, commandType: commandType, commandTimeout: 180);
		}

		public async Task<IDataReader> ExecuteReaderAsync(IDbConnection connection, string sql, CommandType commandType, object parameters = null)
		{
			return await connection.ExecuteReaderAsync(sql, parameters, commandType: commandType, commandTimeout: 180);
		}
	}
}