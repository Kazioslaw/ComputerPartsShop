using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Domain.Response;

namespace ComputerPartsShop.Infrastructure
{
	public interface IAuthTokenProcessor
	{
		public JwtResult GenerateJwtToken(ShopUser user);
		public string GenerateRefreshToken();
		public void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTimeOffset expiration);

	}
}
