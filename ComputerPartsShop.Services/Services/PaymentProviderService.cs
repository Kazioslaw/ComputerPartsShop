using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class PaymentProviderService : IService<PaymentProviderRequest, PaymentProviderResponse, DetailedPaymentProviderResponse, int>
	{
		private readonly IPaymentProviderRepository _providerRepository;

		public PaymentProviderService(IPaymentProviderRepository providerRepository)
		{
			_providerRepository = providerRepository;
		}

		public async Task<List<PaymentProviderResponse>> GetList()
		{
			var paymentProviderList = await _providerRepository.GetList();

			return paymentProviderList.Select(pp => new PaymentProviderResponse(pp.ID, pp.Name, pp.CustomerPayments.Select(x => x.ID).ToList())).ToList();
		}

		public async Task<DetailedPaymentProviderResponse> Get(int id)
		{
			var paymentProvider = await _providerRepository.Get(id);

			var cps = paymentProvider.CustomerPayments;

			return paymentProvider == null ? null! :
				new DetailedPaymentProviderResponse(paymentProvider.ID, paymentProvider.Name,
				cps.Select(cps => new CustomerPaymentSystemResponse(cps.ID, cps.Customer.Username, cps.Customer.Email, cps.Provider.Name, cps.PaymentReference)).ToList());
		}

		public async Task<PaymentProviderResponse> Create(PaymentProviderRequest paymentProvider)
		{
			var newPaymentProvider = new PaymentProvider()
			{
				Name = paymentProvider.Name
			};

			var paymentProviderID = await _providerRepository.Create(newPaymentProvider);

			return new PaymentProviderResponse(paymentProviderID, paymentProvider.Name, new List<Guid>());
		}

		public async Task<PaymentProviderResponse> Update(int id, PaymentProviderRequest updatedPaymentProvider)
		{
			var paymentProvider = new PaymentProvider()
			{
				Name = updatedPaymentProvider.Name,
			};

			await _providerRepository.Update(id, paymentProvider);

			return new PaymentProviderResponse(id, updatedPaymentProvider.Name, new List<Guid>());
		}

		public Task Delete(int id)
		{
			return Task.CompletedTask;
		}
	}
}
