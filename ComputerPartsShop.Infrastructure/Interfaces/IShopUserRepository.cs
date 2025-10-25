using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IShopUserRepository
	{
		public Task<List<ShopUser>> GetListAsync(CancellationToken ct);
		public Task<ShopUser?> GetAsync(Guid id, CancellationToken ct);
		public Task<ShopUser?> GetByUsernameOrEmailAsync(string input, CancellationToken ct);
		public Task<bool> CheckIfUserExists(string username, string email, CancellationToken ct);
		public Task<ShopUser> GetByAddressIDAsync(Guid addressID, CancellationToken ct);
		public Task<ShopUser?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct);
		public Task<ShopUser> CreateAsync(ShopUser request, CancellationToken ct);
		public Task<int> UpdateAsync(Guid id, ShopUser request, CancellationToken ct);
		public Task DeleteAsync(Guid id, CancellationToken ct);

	}
}
