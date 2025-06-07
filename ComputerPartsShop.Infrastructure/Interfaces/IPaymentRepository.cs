using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IPaymentRepository
	{
		public Task<List<Payment>> GetListAsync(string username, CancellationToken ct);
		public Task<Payment> GetAsync(Guid id, string username, CancellationToken ct);
		public Task<Payment> CreateAsync(Payment request, CancellationToken ct);
		public Task<Payment> UpdateStatusAsync(Guid id, Payment request, CancellationToken ct);
		public Task DeleteAsync(Guid id, string username, CancellationToken ct);
	}
}
