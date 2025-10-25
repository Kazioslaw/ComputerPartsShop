namespace ComputerPartsShop.Domain.DTO
{
	public record UserPaymentSystemRequest(string? Username, string? Email, string ProviderName, string PaymentReference);
}