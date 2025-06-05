using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Net;

namespace ComputerPartsShop.Services
{
	public class PaymentProviderService : IPaymentProviderService
	{
		private readonly IPaymentProviderRepository _paymentProviderRepository;
		private readonly IMapper _mapper;

		public PaymentProviderService(IPaymentProviderRepository providerRepository, IMapper mapper)
		{
			_paymentProviderRepository = providerRepository;
			_mapper = mapper;
		}

		public async Task<List<PaymentProviderResponse>> GetListAsync(CancellationToken ct)
		{
			try
			{
				var result = await _paymentProviderRepository.GetListAsync(ct);

				var paymentProviderList = _mapper.Map<IEnumerable<PaymentProviderResponse>>(result);

				return paymentProviderList.ToList();
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task<DetailedPaymentProviderResponse> GetAsync(int id, CancellationToken ct)
		{
			try
			{
				var result = await _paymentProviderRepository.GetAsync(id, ct);

				if (result == null)
				{
					throw new DataErrorException(HttpStatusCode.NotFound, "Payment provider not found");
				}

				var paymentProvider = _mapper.Map<DetailedPaymentProviderResponse>(result);

				return paymentProvider;
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task<DetailedPaymentProviderResponse> GetByNameAsync(string name, CancellationToken ct)
		{
			try
			{
				var result = await _paymentProviderRepository.GetByNameAsync(name, ct);

				if (result == null)
				{
					throw new DataErrorException(HttpStatusCode.NotFound, "Payment provider not found");
				}

				var paymentProvider = _mapper.Map<DetailedPaymentProviderResponse>(result);

				return paymentProvider;
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task<PaymentProviderResponse> CreateAsync(PaymentProviderRequest request, CancellationToken ct)
		{
			try
			{
				var newPaymentProvider = _mapper.Map<PaymentProvider>(request);

				var result = await _paymentProviderRepository.CreateAsync(newPaymentProvider, ct);

				var paymentProvider = _mapper.Map<PaymentProviderResponse>(result);

				return paymentProvider;
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task<PaymentProviderResponse> UpdateAsync(int id, PaymentProviderRequest request, CancellationToken ct)
		{
			try
			{
				var paymentProvider = await _paymentProviderRepository.GetAsync(id, ct);

				if (paymentProvider == null)
				{
					throw new DataErrorException(HttpStatusCode.NotFound, "Payment provider not found");
				}

				var paymentProviderToUpdate = _mapper.Map<PaymentProvider>(request);

				var result = await _paymentProviderRepository.UpdateAsync(id, paymentProviderToUpdate, ct);

				var updatedPaymentProvider = _mapper.Map<PaymentProviderResponse>(result);

				return updatedPaymentProvider;
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			try
			{
				var paymentProvider = await _paymentProviderRepository.GetAsync(id, ct);

				if (paymentProvider == null)
				{
					throw new DataErrorException(HttpStatusCode.NotFound, "Payment provider not found");
				}

				await _paymentProviderRepository.DeleteAsync(id, ct);
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}
	}
}
