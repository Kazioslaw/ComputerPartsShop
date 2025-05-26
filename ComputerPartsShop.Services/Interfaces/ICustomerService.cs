using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface ICustomerService : IService<CustomerRequest, CustomerResponse, DetailedCustomerResponse, Guid>
	{
	}
}
