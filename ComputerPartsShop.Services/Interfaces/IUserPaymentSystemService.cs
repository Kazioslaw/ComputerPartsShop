using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IUserPaymentSystemService
	{
		public Task<List<UserPaymentSystemResponse>> GetListAsync(CancellationToken ct);
		public Task<DetailedUserPaymentSystemResponse> GetAsync(Guid id, string username, CancellationToken ct);
		public Task<UserPaymentSystemResponse> CreateAsync(UserPaymentSystemRequest request, CancellationToken ct);
		public Task<UserPaymentSystemResponse> UpdateAsync(Guid id, string username, UserPaymentSystemRequest request, CancellationToken ct);
		public Task DeleteAsync(Guid id, string username, CancellationToken ct);
	}
}
