using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IPaymentRepository
	{
		public Task<List<Payment>> GetListAsync(CancellationToken ct);
		public Task<Payment> GetAsync(Guid id, CancellationToken ct);
		public Task<Payment> CreateAsync(Payment request, CancellationToken ct);
		public Task<Payment> UpdateStatusAsync(Guid id, Payment request, CancellationToken ct);
		public Task DeleteAsync(Guid id, CancellationToken ct);
	}
}
