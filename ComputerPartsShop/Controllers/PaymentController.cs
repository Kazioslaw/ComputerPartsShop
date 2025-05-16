using ComputerPartsShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		/// <summary>
		/// Get method to get list of payments
		/// </summary>
		/// <returns>List of payments</returns>
		[HttpGet]
		public ActionResult<List<Payment>> GetPaymentList()
		{
			return Ok(Payments);
		}

		/// <summary>
		/// Get method to get payment by its id
		/// </summary>
		/// <param name="id">Payment id to get</param>
		/// <returns>Payment by its id</returns>
		[HttpGet("{id}")]
		public ActionResult<Payment> GetPayment(int id)
		{
			var payment = Payments.FirstOrDefault(x => x.ID == id);
			if (payment == null)
			{
				return NotFound();
			}

			return Ok(payment);
		}


		/// <summary>
		/// Post method to create payment
		/// </summary>
		/// <param name="payment">Payment model to create</param>
		/// <returns>
		/// Created payment with id
		/// </returns>
		[HttpPost]
		public ActionResult<Payment> CreatePayment(Payment payment)
		{
			payment.ID = Payments.Count + 1;
			Payments.Add(payment);
			return Ok(payment);
		}


		/// <summary>
		/// Put method to update payment by its id
		/// </summary>
		/// <param name="id">Payment id to update</param>
		/// <param name="updatedPayment">Payment model to update</param>
		/// <returns>Updated payment</returns>
		[HttpPut("{id}")]
		public ActionResult<Payment> UpdatePayment(int id, Payment updatedPayment)
		{
			var payment = Payments.FirstOrDefault(x => x.ID == id);
			if (payment == null)
			{
				return NotFound();
			}
			payment.Customer = updatedPayment.Customer;
			payment.Order = updatedPayment.Order;
			payment.PaymentStatus = updatedPayment.PaymentStatus;
			payment.PaymentType = updatedPayment.PaymentType;
			return Ok(payment);
		}

		/// <summary>
		/// Delete method to delete payment by id
		/// </summary>
		/// <param name="id">Payment id to delete</param>
		/// <returns>Information about successful deletion</returns>
		[HttpDelete("{id}")]
		public ActionResult DeletePayment(int id)
		{
			var payment = Payments.FirstOrDefault(x => x.ID == id);
			if (payment == null)
			{
				return NotFound();
			}
			Payments.Remove(payment);
			return Ok();
		}

		static List<Payment> Payments = new List<Payment>
		{
			new Payment { ID = 1,
				Customer = new Customer { FirstName = "Bernard", LastName = "Mortensen", PhoneNumber = "201-714-0523", Email = "bernard.Mortensen@mail.com"},
				Order = new Order{},
			PaymentStatus = "Paid", PaymentType = "Credit Card" },
			new Payment { ID = 2, Customer = new Customer { FirstName = "Kade", LastName = "Kim", PhoneNumber = "201-976-1755", Email = "KadeKim@mail.com"},
				Order = new Order{}, PaymentStatus = "Paid", PaymentType = "Cash" },
			new Payment { ID = 3, Customer = new Customer {FirstName = "Alonso", LastName = "Chapman", PhoneNumber = "201-766-4997", Email = "AlonsoChapman@mail.com"},
				Order = new Order{}, PaymentStatus = "Paid", PaymentType = "Cash" },
			new Payment { ID = 4, Customer = new Customer {FirstName = "Felipe", LastName = "Reeves", PhoneNumber = "201-745-0187", Email = "FelipeReeves@mail.com"},
				Order = new Order{}, PaymentStatus = "Paid", PaymentType = "Cash" },
			new Payment { ID = 5, Customer = new Customer {FirstName = "Sawyer", LastName = "Blackburn", PhoneNumber = "201-498-9148", Email = "SawyerBlackburn@mail.com"},
				Order = new Order{}, PaymentStatus = "Not Yet Paid", PaymentType = "Credit Card" },
			new Payment { ID = 6, Customer = new Customer {FirstName = "Owen", LastName = "Kidd", PhoneNumber = "201-287-4477", Email = "OwenKidd@mail.com"},
				Order = new Order{}, PaymentStatus = "Not Yet Paid", PaymentType = "Credit Card" },
			new Payment { ID = 7, Customer = new Customer {FirstName = "Tia", LastName = "Lowe", PhoneNumber = "201-353-1630", Email = "TiaLowe@mail.com"},
				Order = new Order{}, PaymentStatus = "Not Yet Paid", PaymentType = "Credit Card" },
			new Payment { ID = 8, Customer = new Customer {FirstName = "Mireya", LastName = "Haas", PhoneNumber = "201-217-8365", Email = "MireyaHaas@mail.com"},
				Order = new Order{}, PaymentStatus = "Not Yet Paid", PaymentType = "Paypal" },
			new Payment { ID = 9, Customer = new Customer {FirstName = "Misael", LastName = "Chavez", PhoneNumber = "201-495-5360", Email = "MisaelChavez@mail.com"},
				Order = new Order{}, PaymentStatus = "Not Yet Paid", PaymentType = "Paypal" },
			new Payment { ID = 10, Customer = new Customer {FirstName = "Macy", LastName = "House", PhoneNumber = "201-760-9160", Email = "MacyHouse@mail.com"},
				Order = new Order{}, PaymentStatus = "Not Yet Paid", PaymentType = "Paypal" }
		};

	}
}
