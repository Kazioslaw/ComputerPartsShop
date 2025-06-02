using ComputerPartsShop.Domain.DTO;
using FluentValidation;

namespace ComputerPartsShop.API.Validators
{
	public class AddressRequestValidator : AbstractValidator<AddressRequest>
	{
		public AddressRequestValidator()
		{
			RuleFor(x => x.Street).NotNull().NotEmpty().Length(5, 100).WithMessage("Street name can't be null or empty, and must be between 2 and 100 characters.");
			RuleFor(x => x.City).NotNull().NotEmpty().Length(3, 50).WithMessage("City name can't be null or empty, and must be between 3 and 50 characters.");
			RuleFor(x => x.Region).NotNull().NotEmpty().Length(2, 50).WithMessage("Region name can't be null or empty, and must be between 2 and 50 characters.");
			RuleFor(x => x.ZipCode).NotNull().NotEmpty().Length(2, 10).WithMessage("Zipcode can't be null or empty, and must be between 2 and 10 characters.");
			RuleFor(x => x.Country3Code).NotNull().NotEmpty().Length(3).WithMessage("Country3Code must be a valid ISO 3166-1 alpha-3 country code.");
			RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.Username) || !string.IsNullOrWhiteSpace(x.Email))
				.WithMessage("Email or Username is required (at least one must be provided)");
		}
	}

	public class UpdateAddressRequestValidator : AbstractValidator<UpdateAddressRequest>
	{
		public UpdateAddressRequestValidator()
		{
			RuleFor(x => x.newStreet).NotNull().NotEmpty().Length(5, 100).WithMessage("Street name can't be null or empty, and must be between 5 and 100 characters.");
			RuleFor(x => x.newCity).NotNull().NotEmpty().Length(3, 50).WithMessage("City name can't be null or empty, and must be between 3 and 50 characters.");
			RuleFor(x => x.newRegion).NotNull().NotEmpty().Length(2, 50).WithMessage("Region name can't be null or empty, and must be between 2 and 50 characters.");
			RuleFor(x => x.newZipCode).NotNull().NotEmpty().Length(2, 10).WithMessage("Zipcode can't be null or empty, and must be between 2 and 10 characters.");
			RuleFor(x => x.newCountry3Code).NotNull().NotEmpty().Length(3).WithMessage("Country3Code must be a valid ISO 3166-1 alpha-3 country code.");
			RuleFor(x => x).Must(x =>
			(!string.IsNullOrWhiteSpace(x.oldUsername) || !string.IsNullOrWhiteSpace(x.oldEmail)) &&
			(!string.IsNullOrWhiteSpace(x.newUsername) || !string.IsNullOrWhiteSpace(x.newEmail)))
				.WithMessage("oldEmail/oldUsername and newEmail/newUsername are required (at least one of each).");
		}
	}
}
