using ComputerPartsShop.Domain.DTOs;

namespace ComputerPartsShop.Services
{
	public class CustomerService : ICRUDService<CustomerRequest, CustomerResponse, DetailedCustomerResponse, Guid>
	{
		public List<CustomerResponse> GetList()
		{
			throw new NotImplementedException();
		}

		public DetailedCustomerResponse Get(Guid id)
		{
			throw new NotImplementedException();
		}

		public CustomerResponse Create(CustomerRequest request)
		{
			throw new NotImplementedException();
		}

		public CustomerResponse Update(Guid id, CustomerRequest request)
		{
			throw new NotImplementedException();
		}

		public void Delete(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
