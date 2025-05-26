using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CustomerRepository : ICustomerRepository
	{
		readonly TempData _dbContext;

		public CustomerRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Customer>> GetListAsync(CancellationToken ct)
		{
			await Task.Delay(500, ct);
			return _dbContext.CustomerList;
		}

		public async Task<Customer> GetAsync(Guid id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var customer = _dbContext.CustomerList.FirstOrDefault(x => x.Id == id);

			return customer!;
		}

		public async Task<Customer> GetByUsernameOrEmailAsync(string input, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			Customer? customer = null;

			var emailCustomer = _dbContext.CustomerList.FirstOrDefault(x => x.Email == input);
			if (emailCustomer != null)
			{
				customer = emailCustomer;
				return customer;
			}

			var usernameCustomer = _dbContext.CustomerList.FirstOrDefault(x => x.Username == input);
			if (usernameCustomer != null)
			{
				customer = usernameCustomer;
				return customer;
			}

			return customer!;
		}

		public async Task<Guid> CreateAsync(Customer request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			request.Id = Guid.NewGuid();

			_dbContext.CustomerList.Add(request);

			return request.Id;
		}

		public async Task<Customer> UpdateAsync(Guid id, Customer request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var customer = _dbContext.CustomerList.FirstOrDefault(x => x.Id == id);

			if (customer != null)
			{
				customer.FirstName = request.FirstName;
				customer.LastName = request.LastName;
				customer.Username = request.Username;
				customer.Email = request.Email;
				customer.PhoneNumber = request.PhoneNumber;
			}

			return customer!;
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var customer = _dbContext.CustomerList.FirstOrDefault(x => x.Id == id);

			if (customer != null)
			{
				_dbContext.CustomerList.Remove(customer);
			}
		}
	}
}
