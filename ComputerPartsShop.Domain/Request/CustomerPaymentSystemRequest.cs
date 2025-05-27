namespace ComputerPartsShop.Domain.DTO
{
	public record CustomerPaymentSystemRequest(string? Username, string? Email, string ProviderName, string PaymentReference);
}