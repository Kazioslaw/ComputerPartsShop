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
			var customerPaymentsSystem = paymentProviderList.Select(x => x.CustomerPayments);

			return paymentProviderList.Select(pp => new PaymentProviderResponse(pp.Id, pp.Name, pp.CustomerPayments.Select(x => x.Id).ToList())).ToList();
		}

		public async Task<DetailedPaymentProviderResponse> GetAsync(int id, CancellationToken ct)
		{
			var paymentProvider = await _providerRepository.GetAsync(id, ct);

			if (paymentProvider == null)
			{
				return null;
			}

			var customerPaymentSystem = paymentProvider.CustomerPayments;

			return
				new DetailedPaymentProviderResponse(paymentProvider.Id, paymentProvider.Name,
				customerPaymentSystem.Select(customerPaymentSystem => new CustomerPaymentSystemResponse(customerPaymentSystem.Id, customerPaymentSystem.Customer.Username,
				customerPaymentSystem.Customer.Email, paymentProvider.Name, customerPaymentSystem.PaymentReference)).ToList());
		}

		public async Task<DetailedPaymentProviderResponse> GetByNameAsync(string name, CancellationToken ct)
		{
			var paymentProvider = await _providerRepository.GetByNameAsync(name, ct);

			if (paymentProvider == null)
			{
				return null;
			}

			var customerPaymentSystemList = paymentProvider.CustomerPayments;

			return new DetailedPaymentProviderResponse(paymentProvider.Id, paymentProvider.Name,
				customerPaymentSystemList.Select(cps => new CustomerPaymentSystemResponse(cps.Id, cps.Customer.Username,
				cps.Customer.Email, paymentProvider.Name, cps.PaymentReference)).ToList());

		}

		public async Task<PaymentProviderResponse> CreateAsync(PaymentProviderRequest entity, CancellationToken ct)
		{
			var newPaymentProvider = new PaymentProvider()
			{
				Name = entity.Name
			};

			var paymentProvider = await _providerRepository.CreateAsync(newPaymentProvider, ct);

			return new PaymentProviderResponse(paymentProvider.Id, entity.Name, new List<Guid>());
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

		public async Task<bool> DeleteAsync(int id, CancellationToken ct)
		{
			return await _providerRepository.DeleteAsync(id, ct);
		}
	}
}
