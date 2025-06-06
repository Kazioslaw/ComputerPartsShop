using ComputerPartsShop.Domain.DTO;
using FluentValidation;

namespace ComputerPartsShop.API.Validators
{
	public class ShopUserRequestValidator : AbstractValidator<ShopUserRequest>
	{
		public ShopUserRequestValidator()
		{
			RuleFor(x => x.FirstName).NotNull().NotEmpty().Length(2, 100).WithMessage("First Name can't be null or empty, and must be between 2 and 100 characters.");
			RuleFor(x => x.LastName).NotNull().NotEmpty().Length(2, 100).WithMessage("Last Name can't be null or empty, and must be between 2 and 100 characters.");
			RuleFor(x => x.Username).NotNull().NotEmpty().Matches(@"^[a-zA-Z0-9_.-]+$").Length(3, 50).WithMessage("Username can't be null or empty, must be between 3 and 50 characters, and match the required pattern.");
			RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress().Length(5, 50).WithMessage("Email can't be null or empty, must be between 5 and 50 characters, and must match the required pattern.");
			RuleFor(x => x.PhoneNumber).Matches(@"^\+[0-9]+$").Length(6, 20).WithMessage("Phone number cannot be empty if provided.");
			RuleFor(x => x.Password).NotNull().NotEmpty()
				.MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
				.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
				.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
				.Matches("[0-9]").WithMessage("Password must contain at least one number.")
				.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
		}
	}

	public class LoginRequestValidator : AbstractValidator<LoginRequest>
	{
		public LoginRequestValidator()
		{
			RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.Username) || !string.IsNullOrWhiteSpace(x.Email))
				.WithMessage("Email or Username is required (at least one must be provided)");
		}
	}
}
