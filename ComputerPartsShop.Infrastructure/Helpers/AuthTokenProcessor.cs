using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Domain.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ComputerPartsShop.Infrastructure.Helpers
{
	public class AuthTokenProcessor : IAuthTokenProcessor
	{
		private readonly JwtOptions _jwtOptions;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public AuthTokenProcessor(IOptions<JwtOptions> jwtOptions, IHttpContextAccessor httpContextAccessor)
		{
			_jwtOptions = jwtOptions.Value;
			_httpContextAccessor = httpContextAccessor;
		}

		public JwtResult GenerateJwtToken(ShopUser user)
		{
			var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

			var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
				new Claim(ClaimTypes.Role, user.Role.ToString()),
				new Claim(ClaimTypes.NameIdentifier, $"{user.FirstName} {user.LastName}"),
			};

			var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationTimeInMinutes);

			var token = new JwtSecurityToken(issuer: _jwtOptions.Issuer, audience: _jwtOptions.Audience, claims: claims, expires: expires, signingCredentials: credentials);

			var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

			return new JwtResult(jwtToken, expires);
		}

		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[64];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}

		public void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTimeOffset expiration)
		{
			_httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, token, new CookieOptions
			{
				HttpOnly = true,
				Expires = expiration,
				IsEssential = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
			});
		}
	}
}
