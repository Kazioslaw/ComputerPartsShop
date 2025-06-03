using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IPaymentService
	{
		public Task<List<PaymentResponse>> GetListAsync(CancellationToken ct);
		public Task<PaymentResponse> GetAsync(Guid id, CancellationToken ct);
		public Task<PaymentResponse> CreateAsync(PaymentRequest request, CancellationToken ct);
		public Task<PaymentResponse> UpdateStatusAsync(Guid id, UpdatePaymentRequest request, CancellationToken ct);
		public Task<bool> DeleteAsync(Guid id, CancellationToken ct);
	}
}
