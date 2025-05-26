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

		public async Task<List<Customer>> GetList()
		{
			return _dbContext.CustomerList;
		}

		public async Task<Customer> Get(Guid id)
		{
			var customer = _dbContext.CustomerList.FirstOrDefault(x => x.ID == id);

			return customer;
		}

		public async Task<Customer> GetByUsernameOrEmail(string input)
		{
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

			return customer;
		}

		public async Task<Guid> Create(Customer request)
		{
			request.ID = Guid.NewGuid();

			_dbContext.CustomerList.Add(request);

			return request.ID;
		}

		public async Task<Customer> Update(Guid id, Customer request)
		{
			var customer = _dbContext.CustomerList.FirstOrDefault(x => x.ID == id);

			if (customer != null)
			{
				customer.FirstName = request.FirstName;
				customer.LastName = request.LastName;
				customer.Username = request.Username;
				customer.Email = request.Email;
				customer.PhoneNumber = request.PhoneNumber;
			}

			return customer;
		}

		public async Task Delete(Guid id)
		{
			var customer = _dbContext.CustomerList.FirstOrDefault(x => x.ID == id);

			if (customer != null)
			{
				_dbContext.CustomerList.Remove(customer);
			}
		}
	}
}
