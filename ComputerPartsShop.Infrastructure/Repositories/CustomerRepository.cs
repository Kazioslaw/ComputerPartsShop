using ComputerPartsShop.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ComputerPartsShop.Infrastructure
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly DBContext _dbContext;

		public CustomerRepository(DBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Customer>> GetListAsync(CancellationToken ct)
		{
			var query = "SELECT ID, FirstName, LastName, Username, Email, PhoneNumber FROM Customer";

			using (var connection = await _dbContext.CreateConnection())
			{
				var result = await connection.QueryAsync<Customer>(query);

				return result.ToList();
			}
		}

		public async Task<Customer> GetAsync(Guid id, CancellationToken ct)
		{
			var query = "SELECT Customer.ID, Customer.FirstName, Customer.LastName, Customer.Username, Customer.Email, Customer.PhoneNumber, " +
				"Address.ID, Address.Street, Address.City, Address.Region, Address.ZipCode, Country.Alpha3, CustomerPaymentSystem.ID, CustomerPaymentSystem.PaymentReference, " +
				"PaymentProvider.Name, Review.ID, Review.Rating, Review.Description, Product.Name FROM Customer " +
				"LEFT JOIN CustomerAddress ON CustomerAddress.CustomerID = Customer.ID " +
				"LEFT JOIN Address ON CustomerAddress.AddressID = Address.ID " +
				"LEFT JOIN CustomerPaymentSystem ON CustomerPaymentSystem.CustomerID = Customer.ID " +
				"LEFT JOIN Review ON Review.CustomerID = Customer.ID " +
				"LEFT JOIN Country ON Address.CountryID = Country.ID " +
				"LEFT JOIN PaymentProvider ON PaymentProvider.ID = CustomerPaymentSystem.ProviderID " +
				"LEFT JOIN Product ON Product.ID = Review.ProductID WHERE Customer.ID = @Id";

			using (var connection = await _dbContext.CreateConnection())
			{
				var result = await connection.QueryAsync<Customer, Address, Country, CustomerPaymentSystem, PaymentProvider, Review, Product, Customer>(query,
						(customer, address, country, payment, provider, review, product) =>
						{
							var customerDictionary = new Dictionary<Guid, Customer>();
							if (!customerDictionary.TryGetValue(customer.Id, out var currentCustomer))
							{
								currentCustomer = customer;
								currentCustomer.CustomersAddresses = new List<CustomerAddress>();
								currentCustomer.PaymentInfoList = new List<CustomerPaymentSystem>();
								currentCustomer.Reviews = new List<Review>();
								customerDictionary.Add(customer.Id, currentCustomer);
							}

							if (address != null && address.Id != Guid.Empty && !currentCustomer.CustomersAddresses.Any(ca => ca.Address.Id == address.Id))
							{
								address.Country = country;
								currentCustomer.CustomersAddresses.Add(new CustomerAddress
								{
									CustomerId = currentCustomer.Id,
									AddressId = address.Id,
									Address = address
								});
							}

							if (payment != null && payment.Id != Guid.Empty && !currentCustomer.PaymentInfoList.Any(pi => pi.Id == payment.Id))
							{
								payment.Provider = provider;
								currentCustomer.PaymentInfoList.Add(payment);
							}

							if (review != null && review.Id != 0 && !currentCustomer.Reviews.Any(r => r.Id == review.Id))
							{
								review.Product = product;
								currentCustomer.Reviews.Add(review);
							}

							return currentCustomer;
						}, new { Id = id }, splitOn: "ID, Alpha3, ID, Name, ID, Name");

				return result.Distinct().FirstOrDefault();
			}
		}

		public async Task<Customer> GetByUsernameOrEmailAsync(string input, CancellationToken ct)
		{
			var query = "SELECT Customer.ID, Customer.FirstName, Customer.LastName, Customer.Username, Customer.Email, Customer.PhoneNumber, " +
				"Address.ID, Address.Street, Address.City, Address.Region, Address.ZipCode, Country.Alpha3 FROM Customer " +
				"LEFT JOIN CustomerAddress ON CustomerAddress.CustomerID = Customer.ID " +
				"LEFT JOIN Address ON CustomerAddress.AddressID = Address.ID " +
				"LEFT JOIN Country ON Address.CountryID = Country.ID " +
				"WHERE Username = @Input OR Email = @Input";

			var customerDictionary = new Dictionary<Guid, Customer>();

			using (var connection = await _dbContext.CreateConnection())
			{
				var result = await connection.QueryAsync<Customer, Address, Country, Customer>(query,
					(customer, address, country) =>
					{
						if (!customerDictionary.TryGetValue(customer.Id, out var currentCustomer))
						{
							currentCustomer = customer;
							customer.CustomersAddresses = new List<CustomerAddress>();
							customerDictionary.Add(customer.Id, currentCustomer);
						}

						if (address != null && address.Id != Guid.Empty && !currentCustomer.CustomersAddresses.Any(ca => ca.Address.Id == address.Id))
						{
							address.Country = country;
							currentCustomer.CustomersAddresses.Add(new CustomerAddress
							{
								CustomerId = currentCustomer.Id,
								AddressId = address.Id,
								Address = address,
							});
						}

						return currentCustomer;
					},
					splitOn: "ID, Alpha3", param: new { Input = input });

				return result.FirstOrDefault();
			}
		}

		public async Task<Customer> GetByAddressIDAsync(Guid addressID, CancellationToken ct)
		{
			var query = "SELECT Customer.Username, Customer.Email FROM Customer JOIN CustomerAddress ON Customer.ID = CustomerAddress.CustomerID " +
				"WHERE CustomerAddress.AddressID = @AddressID";

			using (var connection = await _dbContext.CreateConnection())
			{
				var result = await connection.QueryAsync<Customer>(query, new { AddressID = addressID });

				return result.FirstOrDefault();
			}
		}

		public async Task<Customer> CreateAsync(Customer request, CancellationToken ct)
		{
			var query = "INSERT INTO Customer (ID, FirstName, LastName, Username, Email, PhoneNumber) VALUES (@Id, @FirstName, @LastName, @Username, @Email, @PhoneNumber)";
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
					catch (SqlException)
					{
						transaction.Rollback();

						return null;
					}
				}
			}
		}

		public async Task<Customer> UpdateAsync(Guid id, Customer request, CancellationToken ct)
		{
			var query = "UPDATE Customer SET FirstName = @FirstName, LastName = @LastName, Username = @Username, Email = @Email, PhoneNumber = @PhoneNumber WHERE ID = @Id";
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
					catch (SqlException)
					{
						transaction.Rollback();

						return null;

					}
				}
			}
		}

		public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
		{
			var query = "DELETE FROM Customer WHERE ID = @Id";

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(query, new { id }, transaction);
						transaction.Commit();

						return true;
					}
					catch (SqlException)
					{
						transaction.Rollback();

						return false;
					}
				}
			}
		}
	}
}
