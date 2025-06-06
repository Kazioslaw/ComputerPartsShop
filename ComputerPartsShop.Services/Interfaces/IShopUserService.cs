using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IShopUserService
	{
		public Task<List<ShopUserResponse>> GetListAsync(CancellationToken ct);
		public Task<DetailedShopUserResponse> GetAsync(Guid id, CancellationToken ct);
		public Task<ShopUserWithAddressResponse> GetByUsernameOrEmailAsync(string input, CancellationToken ct);
		public Task<ShopUserResponse> CreateAsync(ShopUserRequest request, CancellationToken ct);
		public Task<string> LoginAsync(LoginRequest request, CancellationToken ct);
		public Task<string> RefreshTokenAsync(string input, CancellationToken ct);
		public Task<ShopUserResponse> UpdateAsync(Guid id, ShopUserRequest request, CancellationToken ct);
		public Task DeleteAsync(Guid id, CancellationToken ct);
	}
}
