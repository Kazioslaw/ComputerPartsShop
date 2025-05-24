using ComputerPartsShop.Domain.DTOs;

namespace ComputerPartsShop.Services
{
	public class PaymentService : ICRUDService<PaymentRequest, PaymentResponse, DetailedPaymentResponse, int>
	{
		public List<PaymentResponse> GetList()
		{
			throw new NotImplementedException();
		}

		public DetailedPaymentResponse Get(int id)
		{
			throw new NotImplementedException();
		}

		public PaymentResponse Create(PaymentRequest request)
		{
			throw new NotImplementedException();
		}

		public PaymentResponse Update(int id, PaymentRequest request)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{
			throw new NotImplementedException();
		}
	}
}
