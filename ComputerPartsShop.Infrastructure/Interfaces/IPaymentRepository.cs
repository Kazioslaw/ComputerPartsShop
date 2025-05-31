using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IPaymentRepository
	{
		public Task<List<Payment>> GetListAsync(CancellationToken ct);
		public Task<Payment> GetAsync(int id, CancellationToken ct);
		public Task<Payment> CreateAsync(Payment payment, CancellationToken ct);
		public Task<Payment> UpdateAsync(int id, Payment payment, CancellationToken ct);
		public Task<bool> DeleteAsync(int id, CancellationToken ct);
	}
}
