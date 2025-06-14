﻿using Microsoft.Data.SqlClient;
using System.Data;

namespace ComputerPartsShop.Infrastructure
{
	public class DBContext
	{
		private readonly string _connectionString;
		private SqlConnection _connection;
		public DBContext(string connectionString)
		{
			_connectionString = connectionString;

		}

		public async Task<IDbConnection> CreateConnection()
		{
			_connection = new SqlConnection(_connectionString);
			if (_connection.State != ConnectionState.Open)
			{
				await _connection.OpenAsync();
			}

			return _connection;
		}
	}
}
