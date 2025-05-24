using ComputerPartsShop.Domain.DTOs;

namespace ComputerPartsShop.Services
{
	public class PaymentProviderService : ICRUDService<PaymentProviderRequest, PaymentProviderResponse, DetailedPaymentProviderResponse, int>
	{
		public List<PaymentProviderResponse> GetList()
		{
			throw new NotImplementedException();
		}

		public DetailedPaymentProviderResponse Get(int id)
		{
			throw new NotImplementedException();
		}

		public PaymentProviderResponse Create(PaymentProviderRequest request)
		{
			throw new NotImplementedException();
		}

		public PaymentProviderResponse Update(int id, PaymentProviderRequest request)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{
			return;
		}
	}
}
