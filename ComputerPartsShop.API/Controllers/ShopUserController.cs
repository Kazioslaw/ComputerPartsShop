using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ShopUserController : ControllerBase
	{
		public readonly IShopUserService _shopUserService;
		public readonly IAddressRepository _addressRepository;
		public readonly IValidator<ShopUserRequest> _userValidator;
		public readonly IValidator<LoginRequest> _loginValidator;

		public ShopUserController(IShopUserService shopUserService, IAddressRepository addressRepository, IValidator<ShopUserRequest> userValidator, IValidator<LoginRequest> loginValidator)
		{
			_shopUserService = shopUserService;
			_addressRepository = addressRepository;
			_userValidator = userValidator;
			_loginValidator = loginValidator;
		}

		/// <summary>
		/// Asynchronously retrieves all users.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of users</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>List of users</returns>
		[HttpGet]
		public async Task<IActionResult> GetUserListAsync(CancellationToken ct)
		{
			try
			{
				var userList = await _shopUserService.GetListAsync(ct);

				return Ok(userList);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode(ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an user by its ID.
		/// </summary>
		/// <param name="id">User ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the user</response>
		/// <response code="404">Returns if the user was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>User</returns>
		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetUserAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var user = await _shopUserService.GetAsync(id, ct);

				return Ok(user);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode(ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an user by its Username or Email.
		/// </summary>
		/// <param name="usernameOrEmail">User Username or Email</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the user</response>
		/// <response code="404">Returns if the user was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>User</returns>
		[HttpGet("{usernameOrEmail}")]
		public async Task<IActionResult> GetUserByUsernameOrEmail(string usernameOrEmail, CancellationToken ct)
		{
			try
			{
				var user = await _shopUserService.GetByUsernameOrEmailAsync(usernameOrEmail, ct);

				return Ok(user);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode(ex.StatusCode, ex.Message);
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
		[HttpPost]
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

				var user = await _shopUserService.CreateAsync(request, ct);

				return Ok(user);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode(ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously updates an user by its ID.
		/// </summary>
		/// <param name="id">User ID</param>
		/// <param name="request">Updated user model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated user</response>
		/// <response code="404">Returns if the user was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Updated user</returns>
		[HttpPut("{id:guid}")]
		public async Task<IActionResult> UpdateUserAsync(Guid id, ShopUserRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _userValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var updatedUser = await _shopUserService.UpdateAsync(id, request, ct);

				return Ok(updatedUser);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode(ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously deletes an user by its ID.
		/// </summary>
		/// <param name="id">User ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the user was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken ct)
		{
			try
			{
				await _shopUserService.DeleteAsync(id, ct);

				return NoContent();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode(ex.StatusCode, ex.Message);
			}
		}
	}
}
