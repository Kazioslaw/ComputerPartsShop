using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Enums;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Net;

namespace ComputerPartsShop.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IPaymentRepository _paymentRepository;
		private readonly IUserPaymentSystemRepository _userPaymentSystemRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly IMapper _mapper;

		public PaymentService(IPaymentRepository paymentRepository, IUserPaymentSystemRepository userPaymentSystemRepository,
			IOrderRepository orderRepository, IMapper mapper)
		{
			_paymentRepository = paymentRepository;
			_userPaymentSystemRepository = userPaymentSystemRepository;
			_orderRepository = orderRepository;
			_mapper = mapper;

		}

		public async Task<List<PaymentResponse>> GetListAsync(CancellationToken ct)
		{
			try
			{
				var result = await _paymentRepository.GetListAsync(ct);

				var paymentList = _mapper.Map<IEnumerable<PaymentResponse>>(result);

				return paymentList.ToList();
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task<PaymentResponse> GetAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var result = await _paymentRepository.GetAsync(id, ct);

				if (result == null)
				{
					throw new DataErrorException(HttpStatusCode.NotFound, "Payment not found");
				}

				var payment = _mapper.Map<PaymentResponse>(result);

				return payment;
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task<PaymentResponse> CreateAsync(PaymentRequest request, CancellationToken ct)
		{
			try
			{
				var userPaymentSystem = await _userPaymentSystemRepository.GetAsync(request.UserPaymentSystemId, ct);

				if (userPaymentSystem == null)
				{
					throw new DataErrorException(HttpStatusCode.BadRequest, "Invalid user payment system");
				}

				var order = await _orderRepository.GetAsync(request.OrderId, ct);

				if (order == null)
				{
					throw new DataErrorException(HttpStatusCode.BadRequest, "Invalid order");
				}

				var newPayment = _mapper.Map<Payment>(request);
				newPayment.UserPaymentSystemId = userPaymentSystem.Id;
				newPayment.UserPaymentSystem = userPaymentSystem;


				var result = await _paymentRepository.CreateAsync(newPayment, ct);

				var createdPayment = _mapper.Map<PaymentResponse>(result);

				return createdPayment;
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task<PaymentResponse> UpdateStatusAsync(Guid id, UpdatePaymentRequest request, CancellationToken ct)
		{
			try
			{
				var existingPayment = await _paymentRepository.GetAsync(id, ct);

				if (existingPayment == null)
				{
					throw new DataErrorException(HttpStatusCode.NotFound, "Payment not found");
				}

				switch (request.Status)
				{
					case PaymentStatus.Pending:
					case PaymentStatus.Authorized:
					case PaymentStatus.Failed:
					case PaymentStatus.Cancelled:
						existingPayment.Status = request.Status;
						break;
					case PaymentStatus.Completed:
						existingPayment.Status = request.Status;
						existingPayment.PaidAt = DateTime.Now;
						break;
					case PaymentStatus.Refunded:
						existingPayment.Status = request.Status;
						break;
				}

				var result = await _paymentRepository.UpdateStatusAsync(id, existingPayment, ct);

				var updatedPayment = _mapper.Map<PaymentResponse>(result);

				return updatedPayment;
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var payment = _paymentRepository.GetAsync(id, ct);

				if (payment == null)
				{
					throw new DataErrorException(HttpStatusCode.NotFound, "Payment not found");
				}

				await _paymentRepository.DeleteAsync(id, ct);
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}
	}
}
