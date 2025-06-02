using ComputerPartsShop.Domain.DTO;
using FluentValidation;

namespace ComputerPartsShop.API.Validators
{
	public class ReviewRequestValidator : AbstractValidator<ReviewRequest>
	{
		public ReviewRequestValidator()
		{
			RuleFor(x => x.Username).Must(x => string.IsNullOrEmpty(x) || x.Length > 0).WithMessage("Username cannot be empty if provided.");

			RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("ProductId must be greater than 0.");

			RuleFor(x => x.Rating).InclusiveBetween((byte)1, (byte)6).WithMessage("Rating must be between 1 and 6.");

			RuleFor(x => x.Description).Must(x => string.IsNullOrEmpty(x) || x.Length > 0).WithMessage("Description cannot be empty if provided.");
		}
	}
}
