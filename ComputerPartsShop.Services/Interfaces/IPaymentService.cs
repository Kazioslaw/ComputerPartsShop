using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IPaymentService
	{
		public Task<List<PaymentResponse>> GetListAsync(string username, CancellationToken ct);
		public Task<PaymentResponse> GetAsync(Guid id, string username, CancellationToken ct);
		public Task<PaymentResponse> CreateAsync(string username, PaymentRequest request, CancellationToken ct);
		public Task<PaymentResponse> UpdateStatusAsync(Guid id, string username, UpdatePaymentRequest request, CancellationToken ct);
		public Task DeleteAsync(Guid id, string username, CancellationToken ct);
	}
}
