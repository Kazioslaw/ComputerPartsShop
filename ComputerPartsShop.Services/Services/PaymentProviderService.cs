using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class PaymentProviderService : IPaymentProviderService
	{
		private readonly IPaymentProviderRepository _providerRepository;
		private readonly IMapper _mapper;

		public PaymentProviderService(IPaymentProviderRepository providerRepository, IMapper mapper)
		{
			_providerRepository = providerRepository;
			_mapper = mapper;
		}

		public async Task<List<PaymentProviderResponse>> GetListAsync(CancellationToken ct)
		{
			var result = await _providerRepository.GetListAsync(ct);

			var paymentProviderList = _mapper.Map<IEnumerable<PaymentProviderResponse>>(result);

			return paymentProviderList.ToList();
		}

		public async Task<DetailedPaymentProviderResponse> GetAsync(int id, CancellationToken ct)
		{
			var result = await _providerRepository.GetAsync(id, ct);

			if (result == null)
			{
				return null;
			}

			var paymentProvider = _mapper.Map<DetailedPaymentProviderResponse>(result);

			return paymentProvider;
		}

		public async Task<DetailedPaymentProviderResponse> GetByNameAsync(string name, CancellationToken ct)
		{
			var result = await _providerRepository.GetByNameAsync(name, ct);

			if (result == null)
			{
				return null;
			}

			var paymentProvider = _mapper.Map<DetailedPaymentProviderResponse>(result);

			return paymentProvider;
		}

		public async Task<PaymentProviderResponse> CreateAsync(PaymentProviderRequest entity, CancellationToken ct)
		{
			var newPaymentProvider = _mapper.Map<PaymentProvider>(entity);

			var result = await _providerRepository.CreateAsync(newPaymentProvider, ct);

			if (result == null)
			{
				return null;
			}

			var paymentProvider = _mapper.Map<PaymentProviderResponse>(result);

			return paymentProvider;
		}

		public async Task<PaymentProviderResponse> UpdateAsync(int id, PaymentProviderRequest entity, CancellationToken ct)
		{
			var paymentProviderToUpdate = _mapper.Map<PaymentProvider>(entity);

			var result = await _providerRepository.UpdateAsync(id, paymentProviderToUpdate, ct);

			if (result == null)
			{
				return null;
			}

			var updatedPaymentProvider = _mapper.Map<PaymentProviderResponse>(result);

			return updatedPaymentProvider;
		}

		public async Task<bool> DeleteAsync(int id, CancellationToken ct)
		{
			return await _providerRepository.DeleteAsync(id, ct);
		}
	}
}
