using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private readonly ICustomerRepository _customerRepository;
		private readonly IAddressService _addressService;


		public OrderController(IOrderService orderService,
			ICustomerRepository customerRepository,
			IAddressService addressService)
		{
			_orderService = orderService;
			_customerRepository = customerRepository;
			_addressService = addressService;
		}

		[HttpGet]
		public async Task<ActionResult<List<OrderResponse>>> GetOrderListAsync(CancellationToken ct)
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

		[HttpGet("{id:int}")]
		public async Task<ActionResult<DetailedOrderResponse>> GetOrderAsync(int id, CancellationToken ct)
		{
			try
			{
				var order = await _orderService.GetAsync(id, ct);

				if (order == null)
				{
					return NotFound();
				}

				return Ok(order);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		[HttpPost]
		public async Task<ActionResult<OrderResponse>> CreateOrderAsync(OrderRequest request, CancellationToken ct)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Email))
				{
					return BadRequest();
				}

				var customer = await _customerRepository.GetByUsernameOrEmailAsync(request.Username! ?? request.Email!, ct);

				if (customer == null)
				{
					return BadRequest();
				}

				var address = await _addressService.GetAsync(request.AddressId, ct);

				if (address == null)
				{
					return BadRequest();
				}

				var order = await _orderService.CreateAsync(request, ct);


				return CreatedAtAction(nameof(CreateOrderAsync), order);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<OrderResponse>> UpdateOrderAsync(int id, OrderRequest request, CancellationToken ct)
		{
			try
			{
				var order = await _orderService.GetAsync(id, ct);

				if (order == null)
				{
					return NotFound();
				}

				var updatedOrder = _orderService.UpdateAsync(id, request, ct);

				return Ok(updatedOrder);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteOrderAsync(int id, CancellationToken ct)
		{
			try
			{
				var order = await _orderService.GetAsync(id, ct);

				if (order == null)
				{
					return NotFound();
				}

				await _orderService.DeleteAsync(id, ct);

				return Ok();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}
	}
}
