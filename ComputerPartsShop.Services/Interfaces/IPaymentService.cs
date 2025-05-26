using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IPaymentService : IService<PaymentRequest, PaymentResponse, DetailedPaymentResponse, int>
	{
	}
}
