using ComputerPartsShop.Domain.DTO;
using FluentValidation;

namespace ComputerPartsShop.API.Validators
{
	public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
	{
		public PaymentRequestValidator()
		{
			RuleFor(x => x.CustomerPaymentSystemId).NotEmpty().WithMessage("CustomerPaymentSystemId must be provided.");
			RuleFor(x => x.OrderId).GreaterThan(0).WithMessage("OrderId must be greater than zero.");
			RuleFor(x => x.Total).GreaterThan(0).WithMessage("Total must be greater than zero.");
			RuleFor(x => x.Method).IsInEnum().WithMessage("Payment method must be a valid value.");
			RuleFor(x => x.Status).IsInEnum().WithMessage("Payment status must be a valid value.");
			RuleFor(x => x.PaidAt).GreaterThanOrEqualTo(x => x.PaymentStartAt).When(x => x.PaidAt.HasValue && x.PaymentStartAt.HasValue)
				.WithMessage("PaidAt must be greater than or equal to PaymentStartAt.");
			RuleFor(x => x.PaymentStartAt).LessThanOrEqualTo(DateTime.Now).When(x => x.PaymentStartAt.HasValue)
				.WithMessage("PaymentStartAt cannot be in the future.");
			RuleFor(x => x.PaidAt).LessThanOrEqualTo(DateTime.Now).When(x => x.PaidAt.HasValue)
				.WithMessage("PaidAt cannot be in the future.");
		}
	}

	public class UpdatePaymentRequestValidator : AbstractValidator<UpdatePaymentRequest>
	{
		public UpdatePaymentRequestValidator()
		{
			RuleFor(x => x.Status).IsInEnum().WithMessage("Payment status must be a valid value.");
		}
	}
}
