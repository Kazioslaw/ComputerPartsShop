using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Enums;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
using System.Data;

namespace ComputerPartsShop.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IPaymentRepository _paymentRepository;

		public PaymentService(IPaymentRepository paymentRepository)
		{
			_paymentRepository = paymentRepository;
		}

		public async Task<List<PaymentResponse>> GetListAsync(CancellationToken ct)
		{
			var paymentList = await _paymentRepository.GetListAsync(ct);

			return paymentList.Select(p => new PaymentResponse(p.Id, p.CustomerPaymentSystemId, p.OrderId, p.Total, p.Method, p.Status, p.PaymentStartAt, p.PaidAt)).ToList();
		}

		public async Task<PaymentResponse> GetAsync(Guid id, CancellationToken ct)
		{
			var payment = await _paymentRepository.GetAsync(id, ct);

			if (payment == null)
			{
				return null;
			}

			return new PaymentResponse(payment.Id, payment.CustomerPaymentSystemId, payment.OrderId, payment.Total, payment.Method, payment.Status, payment.PaymentStartAt, payment.PaidAt);
		}

		public async Task<PaymentResponse> CreateAsync(PaymentRequest entity, CancellationToken ct)
		{
			var newPayment = new Payment()
			{
				CustomerPaymentSystemId = entity.CustomerPaymentSystemId,
				OrderId = entity.OrderId,
				Total = entity.Total,
				Method = entity.Method,
				Status = PaymentStatus.Pending,
				PaymentStartAt = DateTime.Now
			};

			var payment = await _paymentRepository.CreateAsync(newPayment, ct);

			if (payment == null)
			{
				return null;
			}

			return new PaymentResponse(payment.Id, payment.CustomerPaymentSystemId, payment.OrderId, payment.Total,
				payment.Method, payment.Status, payment.PaymentStartAt, null);
		}

		public async Task<PaymentResponse> UpdateStatusAsync(Guid id, UpdatePaymentRequest entity, CancellationToken ct)
		{
			Payment payment = await _paymentRepository.GetAsync(id, ct);



			switch (entity.Status)
			{
				case PaymentStatus.Pending:
				case PaymentStatus.Authorized:
				case PaymentStatus.Failed:
				case PaymentStatus.Cancelled:
					payment.Status = entity.Status;
					break;
				case PaymentStatus.Completed:
					payment.Status = entity.Status;
					payment.PaidAt = DateTime.Now;
					break;
				case PaymentStatus.Refunded:
					payment.Status = entity.Status;
					break;
			}

			await _paymentRepository.UpdateStatusAsync(id, payment, ct);

			return new PaymentResponse(payment.Id, payment.CustomerPaymentSystemId, payment.OrderId, payment.Total, payment.Method,
				payment.Status, payment.PaymentStartAt, payment.PaidAt);
		}

		public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
		{
			return await _paymentRepository.DeleteAsync(id, ct);
		}
	}
}
