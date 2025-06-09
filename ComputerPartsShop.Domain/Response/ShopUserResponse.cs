namespace ComputerPartsShop.Domain.DTO
{
	public record ShopUserResponse(Guid Id, string FirstName, string LastName, string? Username, string? Email, string? PhoneNumber);
	public record DetailedShopUserResponse(Guid Id, string FirstName, string LastName, string? Username, string? Email, string? PhoneNumber,
		List<AddressResponse> AddressList, List<PaymentInfoInShopUserResponse> PaymentInfoList, List<ReviewInShopUserResponse> ReviewList);

	public record ShopUserWithAddressResponse(Guid Id, string FirstName, string LastName, string? Username, string? Email, string? PhoneNumber, List<AddressResponse> AddressList);
	public record PaymentInfoInShopUserResponse(Guid Id, string ProviderName, string PaymentReference);
	public record ReviewInShopUserResponse(int Id, string ProductName, byte Rating, string? Description);
	public record LoginUserResponse(Guid Id, string Username, string Email, string Token, DateTime ExpiresAt, UserRole Role, string RefreshToken);

	public record TokensResponse(string JwtToken, string RefreshToken);
}