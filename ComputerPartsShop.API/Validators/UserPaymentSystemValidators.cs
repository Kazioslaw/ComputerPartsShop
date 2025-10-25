using ComputerPartsShop.Domain.DTO;
using FluentValidation;

namespace ComputerPartsShop.API.Validators
{
	public class UserPaymentSystemRequestValidator : AbstractValidator<UserPaymentSystemRequest>
	{
		public UserPaymentSystemRequestValidator()
		{
			RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.Username) || !string.IsNullOrWhiteSpace(x.Email))
				.WithMessage("Email or Username is required (at least one must be provided)");
			RuleFor(x => x.ProviderName).NotNull().NotEmpty().WithMessage("Provider name is required");
			RuleFor(x => x.PaymentReference).NotNull().NotEmpty().MaximumLength(50).WithMessage("Payment reference can't be empty or null and can't have more than 50 characters.");
		}
	}
}
