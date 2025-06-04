using ComputerPartsShop.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ComputerPartsShop.Infrastructure
{
	public class ShopUserRepository : IShopUserRepository
	{
		private readonly DBContext _dbContext;

		public ShopUserRepository(DBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<ShopUser>> GetListAsync(CancellationToken ct)
		{
			var query = "SELECT ID, FirstName, LastName, Username, Email, PhoneNumber FROM ShopUser";

			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var result = await connection.QueryAsync<ShopUser>(query);

					return result.ToList();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<ShopUser> GetAsync(Guid id, CancellationToken ct)
		{
			var query = "SELECT ShopUser.ID, USer.FirstName, ShopUser.LastName, ShopUser.Username, ShopUser.Email, ShopUser.PhoneNumber, " +
				"Address.ID, Address.Street, Address.City, Address.Region, Address.ZipCode, Country.Alpha3, UserPaymentSystem.ID, UserPaymentSystem.PaymentReference, " +
				"PaymentProvider.Name, Review.ID, Review.Rating, Review.Description, Product.Name FROM ShopUser " +
				"LEFT JOIN UserAddress ON UserAddress.UserID = ShopUser.ID " +
				"LEFT JOIN Address ON UserAddress.AddressID = Address.ID " +
				"LEFT JOIN UserPaymentSystem ON UserPaymentSystem.UserID = ShopUser.ID " +
				"LEFT JOIN Review ON Review.UserID = ShopUser.ID " +
				"LEFT JOIN Country ON Address.CountryID = Country.ID " +
				"LEFT JOIN PaymentProvider ON PaymentProvider.ID = UserPaymentSystem.ProviderID " +
				"LEFT JOIN Product ON Product.ID = Review.ProductID WHERE ShopUser.ID = @Id";

			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var result = await connection.QueryAsync<ShopUser, Address, Country, UserPaymentSystem, PaymentProvider, Review, Product, ShopUser>(query,
							(user, address, country, payment, provider, review, product) =>
							{
								var userDictionary = new Dictionary<Guid, ShopUser>();
								if (!userDictionary.TryGetValue(user.Id, out var currentUser))
								{
									currentUser = user;
									currentUser.UserAddresses = new List<UserAddress>();
									currentUser.PaymentInfoList = new List<UserPaymentSystem>();
									currentUser.Reviews = new List<Review>();
									userDictionary.Add(user.Id, currentUser);
								}

								if (address != null && address.Id != Guid.Empty && !currentUser.UserAddresses.Any(ca => ca.Address.Id == address.Id))
								{
									address.Country = country;
									currentUser.UserAddresses.Add(new UserAddress
									{
										UserId = currentUser.Id,
										AddressId = address.Id,
										Address = address
									});
								}

								if (payment != null && payment.Id != Guid.Empty && !currentUser.PaymentInfoList.Any(pi => pi.Id == payment.Id))
								{
									payment.Provider = provider;
									currentUser.PaymentInfoList.Add(payment);
								}

								if (review != null && review.Id != 0 && !currentUser.Reviews.Any(r => r.Id == review.Id))
								{
									review.Product = product;
									currentUser.Reviews.Add(review);
								}

								return currentUser;
							}, new { Id = id }, splitOn: "ID, Alpha3, ID, Name, ID, Name");

					return result.Distinct().FirstOrDefault();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<ShopUser> GetByUsernameOrEmailAsync(string input, CancellationToken ct)
		{
			var query = "SELECT ShopUser.ID, ShopUser.FirstName, ShopUser.LastName, ShopUser.Username, ShopUser.Email, ShopUser.PhoneNumber, " +
				"Address.ID, Address.Street, Address.City, Address.Region, Address.ZipCode, Country.Alpha3 FROM ShopUser " +
				"LEFT JOIN UserAddress ON UserAddress.UserID = ShopUser.ID " +
				"LEFT JOIN Address ON UserAddress.AddressID = Address.ID " +
				"LEFT JOIN Country ON Address.CountryID = Country.ID " +
				"WHERE Username = @Input OR Email = @Input";

			var userDictionary = new Dictionary<Guid, ShopUser>();

			using (var connection = await _dbContext.CreateConnection())
			{

				try
				{
					var result = await connection.QueryAsync<ShopUser, Address, Country, ShopUser>(query,
						(user, address, country) =>
						{
							if (!userDictionary.TryGetValue(user.Id, out var currentUser))
							{
								currentUser = user;
								user.UserAddresses = new List<UserAddress>();
								userDictionary.Add(user.Id, currentUser);
							}

							if (address != null && address.Id != Guid.Empty && !currentUser.UserAddresses.Any(ca => ca.Address.Id == address.Id))
							{
								address.Country = country;
								currentUser.UserAddresses.Add(new UserAddress
								{
									UserId = currentUser.Id,
									AddressId = address.Id,
									Address = address,
								});
							}

							return currentUser;
						},
						splitOn: "ID, Alpha3", param: new { Input = input });

					return result.FirstOrDefault();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<ShopUser> GetByAddressIDAsync(Guid addressID, CancellationToken ct)
		{
			var query = "SELECT ShopUser.Username, ShopUser.Email FROM ShopUser JOIN UserAddress ON ShopUser.ID = UserAddress.UserID " +
				"WHERE UserAddress.AddressID = @AddressID";

			using (var connection = await _dbContext.CreateConnection())
			{

				try
				{
					var result = await connection.QueryAsync<ShopUser>(query, new { AddressID = addressID });

					return result.FirstOrDefault();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<ShopUser> CreateAsync(ShopUser request, CancellationToken ct)
		{
			var query = "INSERT INTO ShopUser (ID, FirstName, LastName, Username, Email, PhoneNumber) VALUES (@Id, @FirstName, @LastName, @Username, @Email, @PhoneNumber)";
			request.Id = Guid.NewGuid();

			var parameters = new DynamicParameters();
			parameters.Add("Id", request.Id, DbType.Guid, ParameterDirection.Input);
			parameters.Add("FirstName", request.FirstName, DbType.String, ParameterDirection.Input);
			parameters.Add("LastName", request.LastName, DbType.String, ParameterDirection.Input);
			parameters.Add("Username", request.Username, DbType.String, ParameterDirection.Input);
			parameters.Add("Email", request.Email, DbType.String, ParameterDirection.Input);
			parameters.Add("PhoneNumber", request.PhoneNumber ?? (object)DBNull.Value, DbType.String, ParameterDirection.Input);


			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(query, parameters, transaction);
						transaction.Commit();

						return request;
					}
					catch (SqlException ex)
					{
						transaction.Rollback();
						Console.WriteLine(ex.Message);

						throw;
					}
				}
			}
		}

		public async Task<ShopUser> UpdateAsync(Guid id, ShopUser request, CancellationToken ct)
		{
			var query = "UPDATE ShopUser SET FirstName = @FirstName, LastName = @LastName, Username = @Username, Email = @Email, PhoneNumber = @PhoneNumber WHERE ID = @Id";
			request.Id = id;

			var parameters = new DynamicParameters();
			parameters.Add("Id", request.Id, DbType.Guid, ParameterDirection.Input);
			parameters.Add("FirstName", request.FirstName, DbType.String, ParameterDirection.Input);
			parameters.Add("LastName", request.LastName, DbType.String, ParameterDirection.Input);
			parameters.Add("Username", request.Username, DbType.String, ParameterDirection.Input);
			parameters.Add("Email", request.Email, DbType.String, ParameterDirection.Input);
			parameters.Add("PhoneNumber", request.PhoneNumber ?? (object)DBNull.Value, DbType.String, ParameterDirection.Input);

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(query, parameters, transaction);

						transaction.Commit();

						return request;
					}
					catch (SqlException ex)
					{
						transaction.Rollback();
						Console.WriteLine(ex.Message);

						throw;
					}
				}
			}
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct)
		{
			var query = "DELETE FROM ShopUser WHERE ID = @Id";

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(query, new { id }, transaction);
						transaction.Commit();
					}
					catch (SqlException ex)
					{
						transaction.Rollback();
						Console.WriteLine(ex.Message);

						throw;
					}
				}
			}
		}
	}
}
