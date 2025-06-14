﻿using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private readonly ICustomerRepository _customerRepository;
		private readonly IAddressService _addressService;
		private readonly IValidator<OrderRequest> _orderValidator;
		private readonly IValidator<UpdateOrderRequest> _updateOrderValidator;

		public OrderController(IOrderService orderService,
			ICustomerRepository customerRepository,
			IAddressService addressService,
			IValidator<OrderRequest> orderValidator,
			IValidator<UpdateOrderRequest> updateOrderValidator)
		{
			_orderService = orderService;
			_customerRepository = customerRepository;
			_addressService = addressService;
			_orderValidator = orderValidator;
			_updateOrderValidator = updateOrderValidator;
		}

		/// <summary>
		/// Asynchronously retrieves all orders.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of orders</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>List of orders</returns>
		[HttpGet]
		public async Task<IActionResult> GetOrderListAsync(CancellationToken ct)
		{
			try
			{
				var orderList = await _orderService.GetListAsync(ct);

				return Ok(orderList);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an order by its ID.
		/// </summary>
		/// <param name="id">Order ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the order</response>
		/// <response code="404">Returns if the order was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Order</returns>
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetOrderAsync(int id, CancellationToken ct)
		{
			try
			{
				var order = await _orderService.GetAsync(id, ct);

				if (order == null)
				{
					return NotFound("Order not found");
				}

				return Ok(order);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously creates a new order.
		/// </summary>
		/// <param name="request">Order model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created order</response>
		/// <response code="400">Returns if the username, email or address was empty or invalid</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Created order</returns>
		[HttpPost]
		public async Task<IActionResult> CreateOrderAsync(OrderRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _orderValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var customer = await _customerRepository.GetByUsernameOrEmailAsync(request.Username! ?? request.Email!, ct);

				if (customer == null)
				{
					return BadRequest("Invalid or missing username or email");
				}

				var address = await _addressService.GetAsync(request.AddressId, ct);

				if (address == null)
				{
					return BadRequest("That address does not exist or does not belong to the customer.");
				}

				var order = await _orderService.CreateAsync(request, ct);

				if (order == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Create failed");
				}

				return Ok(order);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously updates an order by its ID.
		/// </summary>
		/// <param name="id">Order ID</param>
		/// <param name="request">Updated order model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated order</response>
		/// <response code="400">Returns if the username, email or address was empty or invalid</response>
		/// <response code="404">Returns if the order was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Updated order</returns>
		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateOrderStatusAsync(int id, UpdateOrderRequest request, CancellationToken ct)
		{
			try
			{
				var validation = _updateOrderValidator.Validate(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var order = await _orderService.GetAsync(id, ct);

				if (order == null)
				{
					return NotFound("Order not found");
				}

				var updatedOrder = await _orderService.UpdateStatusAsync(id, request, ct);

				if (updatedOrder == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Update failed");
				}

				return Ok(updatedOrder);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously deletes an order by its ID.
		/// </summary>
		/// <param name="id">Order ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the order was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteOrderAsync(int id, CancellationToken ct)
		{
			try
			{
				var order = await _orderService.GetAsync(id, ct);

				if (order == null)
				{
					return NotFound("Order not found");
				}

				var isDeleted = await _orderService.DeleteAsync(id, ct);

				if (!isDeleted)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Delete failed");
				}

				return NoContent();

			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}
	}
}
