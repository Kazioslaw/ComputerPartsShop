using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IPaymentService
	{
		public Task<List<PaymentResponse>> GetListAsync(CancellationToken ct);
		public Task<DetailedPaymentResponse> GetAsync(int id, CancellationToken ct);
		public Task<PaymentResponse> CreateAsync(PaymentRequest entity, CancellationToken ct);
		public Task<PaymentResponse> UpdateAsync(int id, PaymentRequest entity, CancellationToken ct);
		public Task DeleteAsync(int id, CancellationToken ct);
	}
}
