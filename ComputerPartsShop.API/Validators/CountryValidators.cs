using ComputerPartsShop.Domain.DTO;
using FluentValidation;

namespace ComputerPartsShop.API.Validators
{
	public class CountryRequestValidator : AbstractValidator<CountryRequest>
	{
		public CountryRequestValidator()
		{
			RuleFor(x => x.Alpha2).NotNull().NotEmpty().Length(2).WithMessage("Alpha2 must be a valid ISO 3166-1 alpha-2 country code.");
			RuleFor(x => x.Alpha3).NotNull().NotEmpty().Length(3).WithMessage("Alpha3 must be a valid ISO 3166-1 alpha-3 country code.");
			RuleFor(x => x.Name).NotNull().NotEmpty().Length(3, 100).WithMessage("Name can't be null or empty, and must be between 3 and 100 characters.");
		}
	}
}
