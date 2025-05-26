using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IPaymentProviderService : IService<PaymentProviderRequest, PaymentProviderResponse, DetailedPaymentProviderResponse, int>
	{
	}
}
