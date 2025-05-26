using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class PaymentProviderService : IPaymentProviderService
	{
		private readonly IPaymentProviderRepository _providerRepository;

		public PaymentProviderService(IPaymentProviderRepository providerRepository)
		{
			_providerRepository = providerRepository;
		}

		public async Task<List<PaymentProviderResponse>> GetListAsync(CancellationToken ct)
		{
			var paymentProviderList = await _providerRepository.GetListAsync(ct);

			return paymentProviderList.Select(pp => new PaymentProviderResponse(pp.ID, pp.Name, pp.CustomerPayments.Select(x => x.ID).ToList())).ToList();
		}

		public async Task<DetailedPaymentProviderResponse> GetAsync(int id, CancellationToken ct)
		{
			var paymentProvider = await _providerRepository.GetAsync(id, ct);

			var cps = paymentProvider.CustomerPayments;

			return paymentProvider == null ? null! :
				new DetailedPaymentProviderResponse(paymentProvider.ID, paymentProvider.Name,
				cps.Select(cps => new CustomerPaymentSystemResponse(cps.ID, cps.Customer.Username, cps.Customer.Email, cps.Provider.Name, cps.PaymentReference)).ToList());
		}

		public async Task<PaymentProviderResponse> CreateAsync(PaymentProviderRequest entity, CancellationToken ct)
		{
			var newPaymentProvider = new PaymentProvider()
			{
				Name = entity.Name
			};

			var paymentProviderID = await _providerRepository.CreateAsync(newPaymentProvider, ct);

			return new PaymentProviderResponse(paymentProviderID, entity.Name, new List<Guid>());
		}

		public async Task<PaymentProviderResponse> UpdateAsync(int id, PaymentProviderRequest entity, CancellationToken ct)
		{
			var paymentProvider = new PaymentProvider()
			{
				Name = entity.Name,
			};

			await _providerRepository.UpdateAsync(id, paymentProvider, ct);

			return new PaymentProviderResponse(id, entity.Name, new List<Guid>());
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			await _providerRepository.DeleteAsync(id, ct);
		}
	}
}
