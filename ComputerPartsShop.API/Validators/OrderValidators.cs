using ComputerPartsShop.Domain.DTO;
using FluentValidation;

namespace ComputerPartsShop.API.Validators
{
	public class OrderRequestValidator : AbstractValidator<OrderRequest>
	{
		public OrderRequestValidator()
		{
			RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.Username) || !string.IsNullOrWhiteSpace(x.Email))
				.WithMessage("Email or Username is required (at least one must be provided)");
			RuleFor(x => x.Products).NotEmpty().WithMessage("Products list cannot be empty.");
			RuleForEach(x => x.Products).ChildRules(x =>
			{
				x.RuleFor(y => y.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
			});
			RuleFor(x => x.Total).GreaterThan(0).WithMessage("Total must be greater than zero.");
			RuleFor(x => x.AddressId).NotNull().NotEmpty().WithMessage("AddressID must be provided.");
			RuleFor(x => x.CustomerPaymentSystemId).NotNull().NotEmpty().WithMessage("CustomerPaymentSystemID must be provided.");
			RuleFor(x => x.Status).IsInEnum().When(x => x.Status.HasValue).WithMessage("Status must be valid DeliveryStatus value");
			RuleFor(x => x.OrderedAt).LessThanOrEqualTo(DateTime.Now).When(x => x.OrderedAt.HasValue).WithMessage("OrderedAt cannot be in the future.");
			RuleFor(x => x.SendAt).GreaterThanOrEqualTo(x => x.OrderedAt).When(x => x.SendAt.HasValue && x.OrderedAt.HasValue).WithMessage("SendAt must be later than or equal to OrderedAt.");
			RuleFor(x => x.SendAt).LessThanOrEqualTo(DateTime.Now).When(x => x.SendAt.HasValue).WithMessage("SendAt cannot be in the future.");
		}
	}

	public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
	{
		public UpdateOrderRequestValidator()
		{
			RuleFor(x => x.Status).IsInEnum().WithMessage("Status must be valid DeliveryStatus value");
		}
	}
}
