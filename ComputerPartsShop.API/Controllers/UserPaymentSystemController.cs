using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Authorize]
	[Route("[controller]")]
	public class UserPaymentSystemController : ControllerBase
	{

		private readonly IUserPaymentSystemService _userPaymentSystemService;
		private readonly IValidator<UserPaymentSystemRequest> _userPaymentSystemValidator;
		public UserPaymentSystemController(IUserPaymentSystemService userPaymentSystemService,
			IValidator<UserPaymentSystemRequest> userPaymentSystemValidator)
		{
			_userPaymentSystemService = userPaymentSystemService;
			_userPaymentSystemValidator = userPaymentSystemValidator;
		}

		/// <summary>
		/// Asynchronously retrieves all user payment systems.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of user payment systems</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>List of user payment systems</returns>

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetUserPaymentSystemListAsync(CancellationToken ct)
		{
			try
			{
				var userPaymentSystemList = await _userPaymentSystemService.GetListAsync(ct);

				return Ok(userPaymentSystemList);
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
		/// Asynchronously retrieves an user payment system by its ID.
		/// </summary>
		/// <param name="id">User payment system ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the user payment system</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the user payment system was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>User payment system</returns>
		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetUserPaymentSystemAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var userPaymentSystem = await _userPaymentSystemService.GetAsync(id, ct);

				return Ok(userPaymentSystem);
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
		/// Asynchronously creates a new user payment system.
		/// </summary>
		/// <param name="request">User payment system model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created user payment system</response>
		/// <response code="400">Returns if the username, email or provider name was empty or invalid</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Created user payment system</returns>
		[HttpPost]
		public async Task<IActionResult> CreateUserPaymentSystemAsync(UserPaymentSystemRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _userPaymentSystemValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var userPaymentSystem = await _userPaymentSystemService.CreateAsync(request, ct);

				return Created(nameof(GetUserPaymentSystemAsync), userPaymentSystem);
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
		/// Asynchronously updates an user payment system by its ID.
		/// </summary>
		/// <param name="id">User payment system ID</param>
		/// <param name="request">Updated user payment system model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated user payment system</response>
		/// <response code="400">Returns if the username, email or provider name was empty or invalid</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the user payment system was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Updated user payment system</returns>
		[HttpPut("{id:guid}")]
		public async Task<IActionResult> UpdateUserPaymentSystemAsync(Guid id, UserPaymentSystemRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _userPaymentSystemValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var userPaymentSystemUpdated = await _userPaymentSystemService.UpdateAsync(id, request, ct);

				if (userPaymentSystemUpdated == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Update failed");
				}

				return Ok(userPaymentSystemUpdated);
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
		/// Asynchronously deletes an user payment system by its ID.
		/// </summary>
		/// <param name="id">User payment system ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the user payment system was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeleteUserPaymentSystemAsync(Guid id, CancellationToken ct)
		{
			try
			{
				await _userPaymentSystemService.DeleteAsync(id, ct);

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
