using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IUserPaymentSystemRepository
	{
		public Task<List<UserPaymentSystem>> GetListAsync(CancellationToken ct);
		public Task<UserPaymentSystem> GetAsync(Guid id, string username, CancellationToken ct);
		public Task<UserPaymentSystem> CreateAsync(UserPaymentSystem request, CancellationToken ct);
		public Task<int> UpdateAsync(Guid id, string username, UserPaymentSystem request, CancellationToken ct);
		public Task<bool> DeleteAsync(Guid id, string username, CancellationToken ct);
	}
}