using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IPaymentRepository
	{
		public Task<List<Payment>> GetListAsync(CancellationToken ct);
		public Task<Payment> GetAsync(Guid id, CancellationToken ct);
		public Task<Payment> CreateAsync(Payment payment, CancellationToken ct);
		public Task<Payment> UpdateStatusAsync(Guid id, Payment payment, CancellationToken ct);
		public Task<bool> DeleteAsync(Guid id, CancellationToken ct);
	}
}
