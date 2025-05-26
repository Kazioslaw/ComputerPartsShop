using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
using System.Data;

namespace ComputerPartsShop.Services
{
	public class PaymentService : IService<PaymentRequest, PaymentResponse, DetailedPaymentResponse, int>
	{
		private readonly IRepository<Payment, int> _paymentRepository;

		public PaymentService(IRepository<Payment, int> paymentRepository)
		{
			_paymentRepository = paymentRepository;
		}

		public async Task<List<PaymentResponse>> GetListAsync()
		{
			var paymentList = await _paymentRepository.GetListAsync();

			return paymentList.Select(p => new PaymentResponse(p.ID, p.CustomerPaymentSystemID, p.OrderID, p.Total, p.Method, p.Status, p.PaymentStartAt, p.PaidAt)).ToList();
		}

		public async Task<DetailedPaymentResponse> GetAsync(int id)
		{
			var payment = await _paymentRepository.GetAsync(id);

			return payment == null ? null! : new DetailedPaymentResponse(payment.ID,
				new CustomerPaymentSystemResponse(payment.CustomerPaymentSystem.ID, payment.CustomerPaymentSystem.Customer.Username,
				payment.CustomerPaymentSystem.Customer.Email, payment.CustomerPaymentSystem.Provider.Name, payment.CustomerPaymentSystem.PaymentReference),
				new OrderInPaymentResponse(payment.OrderID, payment.Order.Customer.Username, payment.Order.Customer.Email,
				payment.Order.OrdersProducts.Select(x => new ProductInPaymentResponse(x.Product.Name, x.Quantity)).ToList(),
				new AddressResponse(payment.Order.DeliveryAddress.ID, payment.Order.DeliveryAddress.Street, payment.Order.DeliveryAddress.City,
				payment.Order.DeliveryAddress.Region, payment.Order.DeliveryAddress.ZipCode, payment.Order.DeliveryAddress.Country.Alpha3),
				payment.Order.Status, payment.Order.OrderedAt, payment.Order.SendAt),
				payment.Total, payment.Method, payment.Status, payment.PaymentStartAt, payment.PaidAt);
		}

		public async Task<PaymentResponse> CreateAsync(PaymentRequest payment)
		{
			var newPayment = new Payment()
			{
				CustomerPaymentSystemID = payment.CustomerPaymentSystemID,
				OrderID = payment.OrderID,
				Total = payment.Total,
				Method = payment.Method,
				Status = "Pending",
				PaymentStartAt = DateTime.Now
			};

			var paymentID = await _paymentRepository.CreateAsync(newPayment);

			return new PaymentResponse(paymentID, payment.CustomerPaymentSystemID, payment.OrderID, payment.Total,
				payment.Method, newPayment.Status, newPayment.PaymentStartAt, null);
		}

		public async Task<PaymentResponse> UpdateAsync(int id, PaymentRequest updatedPayment)
		{
			Payment payment = await _paymentRepository.GetAsync(id);

			payment.CustomerPaymentSystemID = updatedPayment.CustomerPaymentSystemID;
			payment.OrderID = updatedPayment.OrderID;
			payment.Total = updatedPayment.Total;
			payment.Method = updatedPayment.Method;
			payment.Status = updatedPayment.Status!;

			switch (updatedPayment.Status)
			{
				case "Pending":
				case "Authorized":
				case "Failed":
				case "Cancelled":
					payment.PaymentStartAt = (DateTime)updatedPayment.PaymentStartAt!;
					break;
				case "Completed":
					payment.PaymentStartAt = (DateTime)updatedPayment.PaymentStartAt!;
					payment.PaidAt = DateTime.Now;
					break;
				case "Refunded":
					payment.PaymentStartAt = (DateTime)updatedPayment.PaymentStartAt!;
					payment.PaidAt = (DateTime)updatedPayment.PaidAt!;
					break;
			}

			await _paymentRepository.UpdateAsync(id, payment);

			return new PaymentResponse(id, updatedPayment.CustomerPaymentSystemID, updatedPayment.OrderID, updatedPayment.Total, updatedPayment.Method,
				updatedPayment.Status!, (DateTime)updatedPayment.PaymentStartAt!, updatedPayment.PaidAt);
		}

		public async Task DeleteAsync(int id)
		{
			await _paymentRepository.DeleteAsync(id);
		}
	}
}
