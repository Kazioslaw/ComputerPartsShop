using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Enums;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IPaymentRepository _paymentRepository;
		private readonly IMapper _mapper;

		public PaymentService(IPaymentRepository paymentRepository, IMapper mapper)
		{
			_paymentRepository = paymentRepository;
			_mapper = mapper;
		}

		public async Task<List<PaymentResponse>> GetListAsync(CancellationToken ct)
		{
			var result = await _paymentRepository.GetListAsync(ct);

			var paymentList = _mapper.Map<IEnumerable<PaymentResponse>>(result);

			return paymentList.ToList();
		}

		public async Task<PaymentResponse> GetAsync(Guid id, CancellationToken ct)
		{
			var result = await _paymentRepository.GetAsync(id, ct);

			if (result == null)
			{
				return null;
			}

			var payment = _mapper.Map<PaymentResponse>(result);

			return payment;
		}

		public async Task<PaymentResponse> CreateAsync(PaymentRequest entity, CancellationToken ct)
		{
			var newPayment = _mapper.Map<Payment>(entity);

			var result = await _paymentRepository.CreateAsync(newPayment, ct);

			if (result == null)
			{
				return null;
			}

			var createdPayment = _mapper.Map<PaymentResponse>(result);

			return createdPayment;
		}

		public async Task<PaymentResponse> UpdateStatusAsync(Guid id, UpdatePaymentRequest entity, CancellationToken ct)
		{
			var existingPayment = await _paymentRepository.GetAsync(id, ct);

			switch (entity.Status)
			{
				case PaymentStatus.Pending:
				case PaymentStatus.Authorized:
				case PaymentStatus.Failed:
				case PaymentStatus.Cancelled:
					existingPayment.Status = entity.Status;
					break;
				case PaymentStatus.Completed:
					existingPayment.Status = entity.Status;
					existingPayment.PaidAt = DateTime.Now;
					break;
				case PaymentStatus.Refunded:
					existingPayment.Status = entity.Status;
					break;
			}

			var result = await _paymentRepository.UpdateStatusAsync(id, existingPayment, ct);

			if (result == null)
			{
				return null;
			}

			var updatedPayment = _mapper.Map<PaymentResponse>(result);

			return updatedPayment;
		}

		public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
		{
			return await _paymentRepository.DeleteAsync(id, ct);
		}
	}
}
