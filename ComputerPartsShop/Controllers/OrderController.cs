using ComputerPartsShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		/// <summary>
		/// Get method to get list of orders
		/// </summary>
		/// <returns>List of orders</returns>
		[HttpGet]
		public ActionResult<List<Order>> GetOrderList()
		{
			return Ok(Orders);
		}

		/// <summary>
		///	Get method to get order by its id
		/// </summary>
		/// <param name="id">Order id to get</param>
		/// <returns>Order by its id</returns>
		[HttpGet("{id}")]
		public ActionResult<Order> GetOrder(int id)
		{
			var order = Orders.FirstOrDefault(x => x.ID == id);

			if (order == null)
			{
				return NotFound();
			}

			return Ok(order);
		}

		/// <summary>
		/// Post method to create new order
		/// </summary>
		/// <param name="order">Order model to create</param>
		/// <returns>Created order with id</returns>
		[HttpPost]
		public ActionResult<Order> CreateOrder(Order order)
		{
			order.ID = (Orders.OrderByDescending(a => a.ID).FirstOrDefault()?.ID ?? 0) + 1;

			Orders.Add(order);

			return Ok(order);
		}

		/// <summary>
		/// Put method to update order by its id
		/// </summary>
		/// <param name="id">Order id to update</param>
		/// <param name="updatedOrder">Order model to update</param>
		/// <returns>Updated order</returns>
		[HttpPut("{id}")]
		public ActionResult<Order> UpdateOrder(int id, Order updatedOrder)
		{
			var order = Orders.FirstOrDefault(x => x.ID == id);

			if (order == null)
			{
				return NotFound();
			}

			order.Customer = updatedOrder.Customer;
			order.Product = updatedOrder.Product;
			order.TotalOrder = updatedOrder.TotalOrder;
			order.DeliveryStatus = updatedOrder.DeliveryStatus;
			order.DeliveryType = updatedOrder.DeliveryType;

			return Ok(order);
		}


		/// <summary>
		/// Delete method to delete order by its id
		/// </summary>
		/// <param name="id">Order id to delete</param>
		/// <returns>Information about successful deletion</returns>
		[HttpDelete("{id}")]
		public ActionResult<Order> DeleteOrder(int id)
		{
			var order = Orders.FirstOrDefault(x => x.ID == id);

			if (order == null)
			{
				return NotFound();
			}

			Orders.Remove(order);

			return Ok();
		}

		private static List<Order> Orders = new List<Order>()
		{
			new Order {ID = 1, Customer = new Customer{ FirstName = "Alonso", LastName = "Chapman" },
			Product = new List<Product>
			{
				new Product { Name = "Nvidia RTX 5090", Price = 1999M },
				new Product { Name = "Ryzen 7 9800x3D", Price =  479M },
				new Product { Name = "Frame 4000D Modular Mid-Tower PC Case", Price = 94.99M},
			},
			DeliveryStatus = "Order Recieved",
			DeliveryType = "Courier",
			TotalOrder = 1999M + 479M + 94.99M
			},
			new Order {ID = 2, Customer = new Customer{ FirstName = "Sawyer", LastName = "Blackburn" },
			Product = new List<Product>
			{
				new Product { Name = "Nvidia RTX 5090", Price = 1999M },
				new Product { Name = "AMD Ryzen 9 9950x3D", Price = 699M }
			},
			DeliveryStatus = "Processing at warehouse",
			DeliveryType = "Parcel Locker",
			TotalOrder = 1999M + 699M},
			new Order {ID = 3, Customer = new Customer{ FirstName = "Mireya", LastName = "Haas" },
			Product = new List<Product>
			{    new Product { Name = "AMD Ryzen 5 9600x", Price = 279M },
				new Product { Name = "RMx Series RM850x Fully Modular Power Supply", Price = 149.99M },
				new Product { Name = "FRAME 4000D Modular Mid-Tower PC Case", Price = 94.99M } },
			DeliveryStatus = "Processing at warehouse",
			DeliveryType = "Pick-up Point",
			TotalOrder = 279M + 149.99M + 94.99M },
			new Order {ID = 4, Customer = new Customer{ FirstName = "Misael", LastName = "Chavez", },
			Product = new List<Product>
			{
				new Product { Name = "Nvidia RTX 5080", Price = 999M },
				new Product { Name = "AMD Ryzen 7 9800x3D", Price = 479M }
			},
			DeliveryStatus = "Shipped",
			DeliveryType = "Shop",
			TotalOrder = 999M + 479M},
			new Order {ID = 5, Customer = new Customer{ FirstName = "Kade", LastName = "Kim" },
			Product = new List<Product>
			{
				new Product { Name = "Nvidia RTX 5060Ti", Price = 379M }
			},
			DeliveryStatus = "Shipped",
			DeliveryType = "Courier", TotalOrder = 379M},
			new Order {ID = 6, Customer = new Customer{ FirstName = "Felipe", LastName = "Reeves" },
			Product = new List<Product>
			{
				new Product { Name = "HX1500i Fully Modular Ultra-Low Noise Platinum ATX 1500 Watt PC Power Supply", Price = 349.99M },
				new Product { Name = "AMD Ryzen 5 9600x", Price = 279M },
				new Product { Name = "FRAME 4000D Modular Mid-Tower PC Case", Price = 94.99M }},
			DeliveryStatus = "Delayed",
			DeliveryType = "Home", TotalOrder = 349.99M + 279M + 94.99M},
			new Order {ID = 7, Customer = new Customer{ FirstName = "Owen", LastName = "Kidd" },
			Product = new List<Product>
			{
				new Product { Name = "Nvidia RTX 5070Ti", Price = 749M },
				new Product { Name = "AMD Ryzen 9 9950x3D", Price = 699M },
				new Product { Name = "RMx Series RM850x Fully Modular Power Supply", Price = 149.99M }
			},
			DeliveryStatus = "Out for delivery",
			DeliveryType = "Shop",
			TotalOrder = 749M + 699M + 149.99M
			},
			new Order {ID = 8, Customer = new Customer{ FirstName = "Macy", LastName = "House" },
			Product = new List<Product>
			{
				new Product { Name = "Nvidia RTX 5090", Price = 1999M }
			},
			DeliveryStatus = "Out for delivery",
			DeliveryType = "Parcel Locker",
			TotalOrder = 1999M},
			new Order {ID = 9, Customer = new Customer{ FirstName = "Tia", LastName = "Lowe" },
			Product = new List<Product>
			{
				new Product { Name = "AMD Ryzen 7 9800x3D", Price = 479M },
				new Product { Name = "FRAME 4000D Modular Mid-Tower PC Case", Price = 94.99M }
			},
			DeliveryStatus = "In transit",
			DeliveryType = "Courier",
			TotalOrder = 479M + 94.99M
			},
			new Order {ID = 10, Customer = new Customer{ FirstName = "Bernard", LastName = "Mortensen" },
			Product = new List<Product>
			{
				new Product { Name = "Nvidia RTX 5060Ti", Price = 379M },
				new Product { Name = "AMD Ryzen 5 9600x", Price = 279M },
				new Product { Name = "HX1500i Fully Modular Ultra-Low Noise Platinum ATX 1500 Watt PC Power Supply", Price = 349.99M }
			},
			DeliveryStatus = "Delivered",
			DeliveryType = "Shop",
			TotalOrder = 379M + 279M + 349.99M}
		};
	}
}
