namespace ComputerPartsShop.Domain.Response
{
	public record JwtResult(string jwtToken, DateTimeOffset expiresAtUtc);
}
