using ComputerPartsShop.Domain.DTOs;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class PaymentService : ICRUDService<PaymentRequest, PaymentResponse, DetailedPaymentResponse, int>
	{
		private readonly PaymentRepository _paymentRepository;

		public PaymentService(PaymentRepository paymentRepository)
		{
			_paymentRepository = paymentRepository;
		}

		public Task<List<PaymentResponse>> GetList()
		{
			throw new NotImplementedException();
		}

		public Task<DetailedPaymentResponse> Get(int id)
		{
			throw new NotImplementedException();
		}

		public Task<PaymentResponse> Create(PaymentRequest payment)
		{
			throw new NotImplementedException();
		}

		public Task<PaymentResponse> Update(int id, PaymentRequest updatedPayment)
		{
			throw new NotImplementedException();
		}

		public Task Delete(int id)
		{
			throw new NotImplementedException();
		}
	}
}
