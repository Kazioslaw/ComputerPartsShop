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

		public async Task<List<PaymentProviderResponse>> GetListAsync()
		{
			var paymentProviderList = await _providerRepository.GetListAsync();

			return paymentProviderList.Select(pp => new PaymentProviderResponse(pp.ID, pp.Name, pp.CustomerPayments.Select(x => x.ID).ToList())).ToList();
		}

		public async Task<DetailedPaymentProviderResponse> GetAsync(int id)
		{
			var paymentProvider = await _providerRepository.GetAsync(id);

			var cps = paymentProvider.CustomerPayments;

			return paymentProvider == null ? null! :
				new DetailedPaymentProviderResponse(paymentProvider.ID, paymentProvider.Name,
				cps.Select(cps => new CustomerPaymentSystemResponse(cps.ID, cps.Customer.Username, cps.Customer.Email, cps.Provider.Name, cps.PaymentReference)).ToList());
		}

		public async Task<PaymentProviderResponse> CreateAsync(PaymentProviderRequest paymentProvider)
		{
			var newPaymentProvider = new PaymentProvider()
			{
				Name = paymentProvider.Name
			};

			var paymentProviderID = await _providerRepository.CreateAsync(newPaymentProvider);

			return new PaymentProviderResponse(paymentProviderID, paymentProvider.Name, new List<Guid>());
		}

		public async Task<PaymentProviderResponse> UpdateAsync(int id, PaymentProviderRequest updatedPaymentProvider)
		{
			var paymentProvider = new PaymentProvider()
			{
				Name = updatedPaymentProvider.Name,
			};

			await _providerRepository.UpdateAsync(id, paymentProvider);

			return new PaymentProviderResponse(id, updatedPaymentProvider.Name, new List<Guid>());
		}

		public async Task DeleteAsync(int id)
		{
			await _providerRepository.DeleteAsync(id);
			return;
		}
	}
}
