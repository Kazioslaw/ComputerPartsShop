using ComputerPartsShop.Domain.DTO;
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

		public async Task<DetailedPaymentResponse> GetAsync(int id, CancellationToken ct)
		{
			var payment = await _paymentRepository.GetAsync(id, ct);

			return payment == null ? null! : new DetailedPaymentResponse(payment.Id,
				new CustomerPaymentSystemResponse(payment.CustomerPaymentSystem.Id, payment.CustomerPaymentSystem.Customer.Username,
				payment.CustomerPaymentSystem.Customer.Email, payment.CustomerPaymentSystem.Provider.Name, payment.CustomerPaymentSystem.PaymentReference),
				new OrderInPaymentResponse(payment.OrderId, payment.Order.Customer.Username, payment.Order.Customer.Email,
				payment.Order.OrdersProducts.Select(x => new ProductInPaymentResponse(x.Product.Name, x.Quantity)).ToList(),
				new AddressResponse(payment.Order.DeliveryAddress.Id, payment.Order.DeliveryAddress.Street, payment.Order.DeliveryAddress.City,
				payment.Order.DeliveryAddress.Region, payment.Order.DeliveryAddress.ZipCode, payment.Order.DeliveryAddress.Country.Alpha3),
				payment.Order.Status, payment.Order.OrderedAt, payment.Order.SendAt),
				payment.Total, payment.Method, payment.Status, payment.PaymentStartAt, payment.PaidAt);
		}

		public async Task<PaymentResponse> CreateAsync(PaymentRequest entity, CancellationToken ct)
		{
			var newPayment = new Payment()
			{
				CustomerPaymentSystemId = entity.CustomerPaymentSystemId,
				OrderId = entity.OrderId,
				Total = entity.Total,
				Method = entity.Method,
				Status = "Pending",
				PaymentStartAt = DateTime.Now
			};

			var paymentId = await _paymentRepository.CreateAsync(newPayment, ct);

			return new PaymentResponse(paymentId, entity.CustomerPaymentSystemId, entity.OrderId, entity.Total,
				entity.Method, newPayment.Status, newPayment.PaymentStartAt, null);
		}

		public async Task<PaymentResponse> UpdateAsync(int id, PaymentRequest entity, CancellationToken ct)
		{
			Payment payment = await _paymentRepository.GetAsync(id, ct);

			payment.CustomerPaymentSystemId = entity.CustomerPaymentSystemId;
			payment.OrderId = entity.OrderId;
			payment.Total = entity.Total;
			payment.Method = entity.Method;
			payment.Status = entity.Status!;

			switch (entity.Status)
			{
				case "Pending":
				case "Authorized":
				case "Failed":
				case "Cancelled":
					payment.PaymentStartAt = (DateTime)entity.PaymentStartAt!;
					break;
				case "Completed":
					payment.PaymentStartAt = (DateTime)entity.PaymentStartAt!;
					payment.PaidAt = DateTime.Now;
					break;
				case "Refunded":
					payment.PaymentStartAt = (DateTime)entity.PaymentStartAt!;
					payment.PaidAt = (DateTime)entity.PaidAt!;
					break;
			}

			await _paymentRepository.UpdateAsync(id, payment, ct);

			return new PaymentResponse(id, entity.CustomerPaymentSystemId, entity.OrderId, entity.Total, entity.Method,
				entity.Status!, entity.PaymentStartAt, entity.PaidAt);
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			await _paymentRepository.DeleteAsync(id, ct);
		}
	}
}
