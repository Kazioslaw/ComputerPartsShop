using ComputerPartsShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CustomerController : ControllerBase
	{
		static List<Customer> Customers = new List<Customer>
		{
			new Customer {
				ID = 1, FirstName = "Bernard", LastName = "Mortensen", PhoneNumber = "714-201-0523", Email = "vergie.schimm@yahoo.com",
				ContactAddress = new Address {ID = 1, Street = "1935 Ashford Drive", City = "Ashburn", Zipcode = "22011", Region = "VA"},
				DeliveryAddress = new Address {ID = 1, Street = "1935 Ashford Drive", City = "Ashburn", Zipcode = "22011", Region = "VA"},
			},

			new Customer {
				ID = 2, FirstName = "Kade", LastName = "Kim", PhoneNumber = "", Email = "",
				ContactAddress = new Address {ID = 10, Street = "2790 Leo Street", City = "Washington", Zipcode = "15301", Region = "PA"},
				DeliveryAddress = new Address {ID = 10, Street = "2790 Leo Street", City = "Washington", Zipcode = "15301", Region = "PA"}
			},
			new Customer
			{
				ID = 3, FirstName = "Macy", LastName = "House", PhoneNumber = "", Email = "",
				ContactAddress = new Address {ID = 9, Street = "3845 Angie Drive", City = "Santa Ana", Zipcode = "92705", Region = "CA"},
				DeliveryAddress = new Address {ID = 9, Street = "3845 Angie Drive", City = "Santa Ana", Zipcode = "92705", Region = "CA"}
			},
			new Customer
			{
				ID = 4, FirstName = "Alonso", LastName = "Chapman", PhoneNumber = "", Email = "",
				ContactAddress = new Address {ID = 5, Street = "4153 Private Lane", City = "Miami", Zipcode = "65344", Region = "MO"},
				DeliveryAddress = new Address {ID = 5, Street = "4153 Private Lane", City = "Miami", Zipcode = "65344", Region = "MO"}
			},
			new Customer
			{
				ID = 5, FirstName = "Felipe", LastName = "Reeves", PhoneNumber = "", Email = "",
				ContactAddress = new Address {ID = 4, Street = "425 Hart Country Lane", City = "Anaheim", Zipcode = "92801", Region = "CA"},
				DeliveryAddress = new Address {ID = 4, Street = "425 Hart Country Lane", City = "Anaheim", Zipcode = "92801", Region = "CA"}
			},
			new Customer
			{
				ID = 6, FirstName = "Misael", LastName = "Chavez", PhoneNumber = "", Email = "",
				ContactAddress = new Address {ID = 3, Street = "3785 Blackwell Street", City = "Cordova", Zipcode = "99574", Region = "AK"},
				DeliveryAddress = new Address {ID = 3, Street = "3785 Blackwell Street", City = "Cordova", Zipcode = "99574", Region = "AK"}
			},
			new Customer
			{
				ID = 7, FirstName = "Mireya", LastName = "Haas", PhoneNumber = "", Email = "",
				ContactAddress =  new Address {ID = 7, Street = "3456 Water Street", City = "Oakland", Zipcode = "94612", Region = "CA"},
				DeliveryAddress = new Address {ID = 7, Street = "3456 Water Street", City = "Oakland", Zipcode = "94612", Region = "CA"}
			},
			new Customer
			{
				ID = 8, FirstName = "Sawyer", LastName = "Blackburn", PhoneNumber = "", Email = "",
				ContactAddress = new Address {ID = 6, Street = "1417 Pin Oak Drive", City = "Long Beach", Zipcode = "90807", Region = "CA"},
				DeliveryAddress = new Address {ID = 6, Street = "1417 Pin Oak Drive", City = "Long Beach", Zipcode = "90807", Region = "CA"}
			},
			new Customer
			{
				ID = 9, FirstName = "Tia", LastName = "Lowe", PhoneNumber = "", Email = "",
				ContactAddress = new Address {ID = 2, Street = "41 Center Avenue", City = "Fresno", Zipcode = "93710", Region = "CA"},
				DeliveryAddress = new Address {ID = 2, Street = "41 Center Avenue", City = "Fresno", Zipcode = "93710", Region = "CA"}
			},
			new Customer
			{
				ID = 10, FirstName = "Owen", LastName = "Kidd", PhoneNumber = "", Email = "",
				ContactAddress = new Address {ID = 8, Street = "672 State Street", City = "Detroit", Zipcode = "48219", Region = "MI"},
				DeliveryAddress = new Address {ID = 8, Street = "672 State Street", City = "Detroit", Zipcode = "48219", Region = "MI"}
			}

		};


		/// <summary>
		/// Get method to get customer list
		/// </summary>
		/// <returns>
		/// List of customers
		/// </returns>
		[HttpGet]
		public ActionResult<List<Customer>> GetCustomerList()
		{
			return Ok(Customers);
		}

		/// <summary>
		/// Get method to get customer by its id
		/// </summary>
		/// <param name="id">Customer id to get</param>
		/// <returns>Customer by its id</returns>
		[HttpGet("{id}")]
		public ActionResult<Customer> GetCustomer(int id)
		{
			var customer = Customers.FirstOrDefault(c => c.ID == id);
			if (customer == null)
			{
				return NotFound();
			}
			return Ok(customer);
		}

		/// <summary>
		/// Post method to create customer
		/// </summary>
		/// <param name="customer">Customer model to create</param>
		/// <returns>Created customer with id</returns>
		[HttpPost]
		public ActionResult<Customer> CreateCustomer(Customer customer)
		{
			customer.ID = Customers.Count + 1;
			Customers.Add(customer);
			return CreatedAtAction(nameof(CreateCustomer), customer);
		}

		/// <summary>
		/// Put method to update customer by its id
		/// </summary>
		/// <param name="id">Customer id to update</param>
		/// <param name="updatedCustomer">Customer model to update</param>
		/// <returns>Updated customer</returns>
		[HttpPut("{id}")]
		public ActionResult<Customer> UpdateCustomer(int id, Customer updatedCustomer)
		{
			var customer = Customers.FirstOrDefault(c => c.ID == id);
			if (customer == null)
			{
				return NotFound();
			}
			customer.FirstName = updatedCustomer.FirstName;
			customer.LastName = updatedCustomer.LastName;
			customer.PhoneNumber = updatedCustomer.PhoneNumber;
			customer.Email = updatedCustomer.Email;
			customer.ContactAddress = updatedCustomer.ContactAddress;
			customer.DeliveryAddress = updatedCustomer.DeliveryAddress;

			return Ok(customer);
		}

		/// <summary>
		/// Delete method to delete customer by its id
		/// </summary>
		/// <param name="id">Customer id to delete</param>
		/// <returns>Information about successful deletion</returns>
		[HttpDelete("{id}")]
		public ActionResult DeleteCustomer(int id)
		{
			var customer = Customers.FirstOrDefault(c => c.ID == id);
			if (customer == null)
			{
				return NotFound();
			}
			Customers.Remove(customer);
			return Ok();
		}
	}
}
