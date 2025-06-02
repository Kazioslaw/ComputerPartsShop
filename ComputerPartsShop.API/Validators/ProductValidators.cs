using ComputerPartsShop.Domain.DTO;
using FluentValidation;

namespace ComputerPartsShop.API.Validators
{
	public class ProductRequestValidator : AbstractValidator<ProductRequest>
	{
		public ProductRequestValidator()
		{
			RuleFor(x => x.Name).NotNull().NotEmpty().Length(3, 100).WithMessage("Product name can't be null or empty, and must be between 3 and 100 characters.");
			RuleFor(x => x.Description).NotNull().NotEmpty().Length(10, 4000).WithMessage("Description can't be null or empty, and must be between 10 and 4000 characters.");
			RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("Unit price must be greater than or equal to 0.");
			RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock must be greater than or equal to 0.");
			RuleFor(x => x.CategoryName).NotNull().NotEmpty().WithMessage("Category can't be null or empty");
			RuleFor(x => x.InternalCode).NotNull().NotEmpty().Length(4, 100).WithMessage("Internal code can't be null or empty, and must be between 4 and 100 characters.");
		}
	}
}
