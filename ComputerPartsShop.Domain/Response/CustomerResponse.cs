namespace ComputerPartsShop.Domain.DTO
{
	public record CustomerResponse(Guid ID, string FirstName, string Lastname, string? Username, string? Email, string? PhoneNumber);
	public record DetailedCustomerResponse(Guid Id, string FirstName, string LastName, string? Username, string? Email, string? Phone,
		List<AddressResponse> AddressList, List<PaymentInfoInCustomerResponse> PaymentInfoList, List<ReviewInCustomerResponse> ReviewList);
	public record PaymentInfoInCustomerResponse(Guid id, string ProviderName, string PaymentReference);
	public record ReviewInCustomerResponse(int ID, string ProductName, byte Rating, string? Description);
}