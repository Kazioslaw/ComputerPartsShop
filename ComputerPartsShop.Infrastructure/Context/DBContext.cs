using Microsoft.Data.SqlClient;
using System.Data;

namespace ComputerPartsShop.Infrastructure
{
	public class DBContext
	{
		private readonly string _connectionString;
		public DBContext(string connectionString)
		{
			_connectionString = connectionString;
		}

		public IDbConnection CreateConnection()
		{
			var connection = new SqlConnection(_connectionString);
			connection.Open();
			return connection;
		}
	}
}
