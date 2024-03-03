using Booking.DBEngine;
using Booking.Models;
using Dapper;
using System.Data;

namespace Booking.Data
{
	public interface IMailing
	{
		Task<List<MailDetailsDTO>> GetMailDetails(Int16 MailStatus);
	}

	public class Mailing:IMailing
	{

		public readonly IDBHandler _dbHandler;



		public Mailing(IDBHandler dbHandler)
		{
			_dbHandler = dbHandler;
		}


		public async Task<List<MailDetailsDTO>> GetMailDetails(Int16 MailStatus)
		{
			List<MailDetailsDTO> mailDetails = new List<MailDetailsDTO>();
			var parameters = new DynamicParameters();
			try
			{
				parameters.Add("MailType", MailStatus, DbType.Int16, ParameterDirection.Input);

				using (_dbHandler.Connection)
				{
					mailDetails = (await _dbHandler.QueryAsync<MailDetailsDTO>(_dbHandler.Connection, "dbo.GetMailDetails", CommandType.StoredProcedure, parameters)).ToList();
				}
			}
			catch (Exception ex)
			{
				new ErrorLog().WriteLog(ex);
			}


			return mailDetails;
		}
	}
}
