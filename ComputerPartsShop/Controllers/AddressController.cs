using ComputerPartsShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AddressController : ControllerBase
	{
		static List<Address> Addresses = new List<Address>
		{
			new Address {ID = 1, Street = "1935 Ashford Drive", City = "Ashburn", Zipcode = "22011", Region = "VA"},
			new Address {ID = 2, Street = "41 Center Avenue", City = "Fresno", Zipcode = "93710", Region = "CA"},
			new Address {ID = 3, Street = "3785 Blackwell Street", City = "Cordova", Zipcode = "99574", Region = "AK"},
			new Address {ID = 4, Street = "425 Hart Country Lane", City = "Anaheim", Zipcode = "92801", Region = "CA"},
			new Address {ID = 5, Street = "4153 Private Lane", City = "Miami", Zipcode = "65344", Region = "MO"},
			new Address {ID = 6, Street = "1417 Pin Oak Drive", City = "Long Beach", Zipcode = "90807", Region = "CA"},
			new Address {ID = 7, Street = "3456 Water Street", City = "Oakland", Zipcode = "94612", Region = "CA"},
			new Address {ID = 8, Street = "672 State Street", City = "Detroit", Zipcode = "48219", Region = "MI"},
			new Address {ID = 9, Street = "3845 Angie Drive", City = "Santa Ana", Zipcode = "92705", Region = "CA"},
			new Address {ID = 10, Street = "2790 Leo Street", City = "Washington", Zipcode = "15301", Region = "PA"}
		};

		/// <summary>
		///	Get method to get list of addresses
		/// </summary>
		/// <returns>
		/// List of addresses
		/// </returns>
		[HttpGet]
		public ActionResult<List<Address>> GetAddressList()
		{
			return Ok(Addresses);
		}

		/// <summary>
		/// Get method to get address by its id
		/// </summary>
		/// <param name="id">Address id to get</param>
		/// <returns>Address by its id</returns>

		[HttpGet("{id}")]
		public ActionResult<Address> GetAddress(int id)
		{
			var address = Addresses.FirstOrDefault(a => a.ID == id);
			if (address == null)
			{
				return NotFound();
			}
			return Ok(address);
		}

		/// <summary>
		/// Post method to Create new address
		/// </summary>
		/// <param name="address">Addres model to create</param>
		/// <returns>Newly created address with id</returns>
		[HttpPost]
		public ActionResult<Address> CreateAddress(Address address)
		{
			address.ID = Addresses.Count + 1;
			Addresses.Add(address);
			return CreatedAtAction(nameof(CreateAddress), address);
		}

		/// <summary>
		/// Put method to Update address by its id
		/// </summary>
		/// <param name="id">Address id to update</param>
		/// <param name="updatedAddress">Address model to update</param>
		/// <returns>Updated address</returns>

		[HttpPut("{id}")]
		public ActionResult<Address> UpdateAddress(int id, Address updatedAddress)
		{
			var address = Addresses.FirstOrDefault(a => a.ID == id);
			if (address == null)
			{
				return NotFound();
			}
			address.Street = updatedAddress.Street;
			address.City = updatedAddress.City;
			address.Zipcode = updatedAddress.Zipcode;
			address.Region = updatedAddress.Region;
			return Ok(address);
		}

		/// <summary>
		/// Delete method to delete address by its id
		/// </summary>
		/// <param name="id">Address id to delete</param>
		/// <returns>Information about successful deletion</returns>
		[HttpDelete("{id}")]
		public ActionResult DeleteAddress(int id)
		{
			var address = Addresses.FirstOrDefault(a => a.ID == id);
			if (address == null)
			{
				return NotFound();
			}

			return Ok();
		}
	}
}
