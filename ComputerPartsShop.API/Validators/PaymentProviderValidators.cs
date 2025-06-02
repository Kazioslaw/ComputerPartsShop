using ComputerPartsShop.Domain.DTO;
using FluentValidation;

namespace ComputerPartsShop.API.Validators
{
	public class PaymentProviderRequestValidator : AbstractValidator<PaymentProviderRequest>
	{
		public PaymentProviderRequestValidator()
		{
			RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(100).WithMessage("Provider name can't be numm or empty and can't have more than 100 characters.");
		}
	}
}
