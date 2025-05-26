using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface ICustomerPaymentSystemService : IService<CustomerPaymentSystemRequest, CustomerPaymentSystemResponse, DetailedCustomerPaymentSystemResponse, Guid>
	{
	}
}
