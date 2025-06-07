using ComputerPartsShop.Domain;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ShopUserController : ControllerBase
	{
		public readonly IShopUserService _userService;
		public readonly IAddressRepository _addressRepository;
		public readonly IValidator<ShopUserRequest> _userValidator;
		public readonly IValidator<LoginRequest> _loginValidator;

		public ShopUserController(IShopUserService userService, IAddressRepository addressRepository, IValidator<ShopUserRequest> userValidator, IValidator<LoginRequest> loginValidator)
		{
			_userService = userService;
			_addressRepository = addressRepository;
			_userValidator = userValidator;
			_loginValidator = loginValidator;
		}

		/// <summary>
		/// Asynchronously retrieves all users.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of users</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>List of users</returns>
		[HttpGet]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> GetUserListAsync(CancellationToken ct)
		{
			try
			{
				var userList = await _userService.GetListAsync(ct);

				return Ok(userList);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an user by its ID.
		/// </summary>
		/// <param name="id">User ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the user</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the user was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>User</returns>
		[HttpGet("{id:guid}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> GetUserAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var user = await _userService.GetAsync(id, ct);

				return Ok(user);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an user by its Username or Email.
		/// </summary>
		/// <param name="usernameOrEmail">User Username or Email</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the user</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the user was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>User</returns>
		[HttpGet("{usernameOrEmail}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> GetUserByUsernameOrEmail(string usernameOrEmail, CancellationToken ct)
		{
			try
			{
				var user = await _userService.GetByUsernameOrEmailAsync(usernameOrEmail, ct);

				return Ok(user);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously creates a new user.
		/// </summary>
		/// <param name="request">User model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the user</response>
		/// <response code="400">Returns if the user was not created</response>
		/// <response code="404">Returns if the user was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>User</returns>
		[HttpPost("register")]
		public async Task<IActionResult> CreateUserAsync(ShopUserRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _userValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var user = await _userService.CreateAsync(request, ct);

				return Ok(user);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously login user
		/// </summary>
		/// <param name="request">Login model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the authentication token if login is successful</response>
		/// <response code="400">Returned when the request is invalid (e.g., missing fields)</response>
		/// <response code="401">Returned when the credentials are incorrect</response>
		/// <response code="404">Returned if the user does not exist</response>
		/// <response code="499">Returned if the client canceled the request</response>
		/// <response code="500">Returned if an internal server or database error occurs</response>
		/// <returns>Authentication token</returns>
		[HttpPost("login")]
		public async Task<IActionResult> LoginUserAsync(LoginRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _loginValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());

					return BadRequest(errors);
				}

				var token = await _userService.LoginAsync(request, ct);

				return Ok(token);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously refreshing token
		/// </summary>
		/// <param name="refreshToken">Token for refresh jwt token</param>
		/// <param name="ct">Cancelation token</param>
		/// <returns>New Jwt and Refresh tokens</returns>
		[HttpPost("refresh")]
		public async Task<IActionResult> RefreshUserTokenAsync(string refreshToken, CancellationToken ct)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(refreshToken))
				{
					throw new DataErrorException(HttpStatusCode.Forbidden, "Refresh token is empty, not valid or expired");
				}

				var token = await _userService.RefreshTokenAsync(refreshToken, ct);

				return Ok(token);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously updates an user by its ID.
		/// </summary>
		/// <param name="id">User ID</param>
		/// <param name="request">Updated user model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated user</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the user was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Updated user</returns>
		[HttpPut("{id:guid}")]
		[Authorize]
		public async Task<IActionResult> UpdateUserAsync(Guid id, ShopUserRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _userValidator.ValidateAsync(request);
				var usernameFromToken = HttpContext.User.Identity?.Name;
				var emailFromToken = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

				if (string.IsNullOrWhiteSpace(usernameFromToken) || string.IsNullOrWhiteSpace(emailFromToken))
				{
					throw new DataErrorException(HttpStatusCode.Forbidden, "Username and Email is missing");
				}

				request = request with { Username = usernameFromToken, Email = emailFromToken };

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var updatedUser = await _userService.UpdateAsync(id, request, ct);

				return Ok(updatedUser);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously deletes an user by its ID.
		/// </summary>
		/// <param name="id">User ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the user was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:guid}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken ct)
		{
			try
			{
				await _userService.DeleteAsync(id, ct);

				return NoContent();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}
	}
}
