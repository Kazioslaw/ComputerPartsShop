using ComputerPartsShop.Domain.DTOs;
using ComputerPartsShop.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AddressController : ControllerBase
	{
		/// <summary>
		///	Get method to get list of addresses
		/// </summary>
		/// <returns>
		/// List of addresses.
		/// </returns>
		[HttpGet]
		public ActionResult<List<AddressResponse>> GetAddressList()
		{
			return Ok(Addresses);
		}

		/// <summary>
		/// Get method to get address by its id
		/// </summary>
		/// <param name="id">Address id to get</param>
		/// <returns>Address by its id</returns>
		[HttpGet("{id:guid}")]
		public ActionResult<AddressResponse> GetAddress(Guid id)
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
		/// <param name="address">Address model to create</param>
		/// <returns>Newly created address with id</returns>
		[HttpPost]
		public ActionResult<Address> CreateAddress(Address address)
		{
			address.ID = Guid.NewGuid();

			Addresses.Add(address);

			return CreatedAtAction(nameof(CreateAddress), address);
		}

		/// <summary>
		/// Put method to Update address by its id
		/// </summary>
		/// <param name="id">Address id to update</param>
		/// <param name="updatedAddress">Address model to update</param>
		/// <returns>Updated address</returns>
		[HttpPut("{id:guid}")]
		public ActionResult<Address> UpdateAddress(Guid id, Address updatedAddress)
		{
			var address = Addresses.FirstOrDefault(a => a.ID == id);

			if (address == null)
			{
				return NotFound();
			}

			address.Street = updatedAddress.Street;
			address.City = updatedAddress.City;
			address.ZipCode = updatedAddress.ZipCode;
			address.Region = updatedAddress.Region;

			return Ok(address);
		}

		/// <summary>
		/// Delete method to delete address by its id
		/// </summary>
		/// <param name="id">Address id to delete</param>
		/// <returns>Information about successful deletion</returns>
		[HttpDelete("{id:guid}")]
		public ActionResult DeleteAddress(Guid id)
		{
			var address = Addresses.FirstOrDefault(a => a.ID == id);

			if (address == null)
			{
				return NotFound();
			}

			return Ok();
		}

		private static List<Address> Addresses = new List<Address>
		{
			new Address {ID = Guid.NewGuid(), Street = "Sosnowa 39A", City = "Ostrów Mazowiecka", ZipCode = "28-903", Region = "Wielkopolskie", CountryID = 1,},
			new Address {ID = Guid.NewGuid(), Street = "41 Center Avenue", City = "Fresno", ZipCode = "93710", Region = "CA", CountryID = 1},
			new Address {ID = Guid.NewGuid(), Street = "3785 Blackwell Street", City = "Cordova", ZipCode = "99574", Region = "AK"},
			new Address {ID = Guid.NewGuid(), Street = "425 Hart Country Lane", City = "Anaheim", ZipCode = "92801", Region = "CA"},
			new Address {ID = Guid.NewGuid(), Street = "4153 Private Lane", City = "Miami", ZipCode = "65344", Region = "MO"},
			new Address {ID = Guid.NewGuid(), Street = "1417 Pin Oak Drive", City = "Long Beach", ZipCode = "90807", Region = "CA"},
			new Address {ID = Guid.NewGuid(), Street = "3456 Water Street", City = "Oakland", ZipCode = "94612", Region = "CA"},
			new Address {ID = Guid.NewGuid(), Street = "672 State Street", City = "Detroit", ZipCode = "48219", Region = "MI"},
			new Address {ID = Guid.NewGuid(), Street = "3845 Angie Drive", City = "Santa Ana", ZipCode = "92705", Region = "CA"},
			new Address {ID = Guid.NewGuid(), Street = "2790 Leo Street", City = "Washington", ZipCode = "15301", Region = "PA"}
		};
	}
}
