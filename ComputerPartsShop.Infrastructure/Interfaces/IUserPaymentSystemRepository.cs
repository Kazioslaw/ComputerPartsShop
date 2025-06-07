using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IUserPaymentSystemRepository
	{
		public Task<List<UserPaymentSystem>> GetListAsync(CancellationToken ct);
		public Task<UserPaymentSystem> GetAsync(Guid id, string username, CancellationToken ct);
		public Task<UserPaymentSystem> CreateAsync(UserPaymentSystem request, CancellationToken ct);
		public Task<UserPaymentSystem> UpdateAsync(Guid id, UserPaymentSystem request, CancellationToken ct);
		public Task DeleteAsync(Guid id, CancellationToken ct);
	}
}