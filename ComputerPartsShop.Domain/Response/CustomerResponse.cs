namespace ComputerPartsShop.Domain.DTO
{
	public record CustomerResponse(Guid Id, string FirstName, string lastName, string? Username, string? Email, string? PhoneNumber);
	public record DetailedCustomerResponse(Guid Id, string FirstName, string LastName, string? Username, string? Email, string? PhoneNumber,
		List<AddressResponse> AddressList, List<PaymentInfoInCustomerResponse> PaymentInfoList, List<ReviewInCustomerResponse> ReviewList);
	public record PaymentInfoInCustomerResponse(Guid Id, string ProviderName, string PaymentReference);
	public record ReviewInCustomerResponse(int Id, string ProductName, byte Rating, string? Description);
}