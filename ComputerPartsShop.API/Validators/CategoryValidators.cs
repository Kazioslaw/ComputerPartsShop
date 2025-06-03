using ComputerPartsShop.Domain.DTO;
using FluentValidation;

namespace ComputerPartsShop.API.Validators
{
	public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
	{
		public CategoryRequestValidator()
		{
			RuleFor(x => x.Name).NotNull().NotEmpty().Length(2, 50).WithMessage("Name can't be null or empty, and must be between 2 and 50 characters.");
			RuleFor(x => x.Description).NotNull().NotEmpty().Length(10, 4000).WithMessage("Description can't be null or empty, and must be between 10 and 4000 characters.");
		}
	}
}
