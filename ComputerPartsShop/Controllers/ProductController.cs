using ComputerPartsShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductController : ControllerBase
	{
		/// <summary>
		/// Get method to get list of products
		/// </summary>
		/// <returns>
		/// List of products.
		/// </returns>
		[HttpGet]
		public ActionResult<List<Product>> GetProductList()
		{
			return Ok(products);
		}

		/// <summary>
		/// Get method to get product by its id
		/// </summary>
		/// <param name="id">Product id to get</param>
		/// <returns>
		/// Product by its id
		/// </returns>
		[HttpGet("{id}")]
		public ActionResult<Product> GetProduct(int id)
		{
			var product = products.FirstOrDefault(x => x.ID == id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}

		/// <summary>
		/// Post method to create new product
		/// </summary>
		/// <param name="product">Product model to create</param>
		/// <returns>Newly created product with id.</returns>
		[HttpPost]
		public ActionResult<Product> CreateProduct(Product product)
		{
			product.ID = products.Count + 1;
			products.Add(product);
			return Ok(product);
		}

		/// <summary>
		/// Put method to update product by its id
		/// </summary>
		/// <param name="id">Product id to update</param>
		/// <param name="updatedProduct">Product model to update</param>
		/// <returns>
		///	Updated product.
		/// </returns>
		[HttpPut("{id}")]
		public ActionResult<Product> UpdateProduct(int id, Product updatedProduct)
		{
			var product = products.FirstOrDefault(x => x.ID == id);
			if (product == null)
			{
				return NotFound();
			}

			product.Name = updatedProduct.Name;
			product.Description = updatedProduct.Description;
			product.Category = updatedProduct.Category;
			product.Price = updatedProduct.Price;
			product.Stock = updatedProduct.Stock;

			return Ok(product);
		}

		/// <summary>
		/// Delete method to delete product by its id
		/// </summary>
		/// <param name="id">Product id to delete</param>
		/// <returns>
		/// nformation about successful deletion.
		/// </returns>
		[HttpDelete("{id}")]
		public ActionResult DeleteProduct(int id)
		{
			var product = products.FirstOrDefault(x => x.ID == id);
			if (product == null)
			{
				return NotFound();
			}

			products.Remove(product);
			return Ok();
		}

		static List<Product> products = new List<Product>
		{
			new Product {
				ID = 1, Name = "Nvidia RTX 5090",
				Description = "The NVIDIA® GeForce RTX™ 5090 is the most powerful GeForce GPU ever made, bringing game-changing capabilities to gamers and creators. " +
				"Tackle the most advanced models and most challenging creative workloads with unprecedented AI horsepower. Game with full ray tracing and the lowest latency. " +
				"The GeForce RTX 5090 is powered by the NVIDIA Blackwell architecture and equipped with 32 GB of super-fast GDDR7 memory, so you can do it all.",
				Category = new Category { ID = 4, Name = "GPU/Graphics Card"},
				Price = 1999M, Stock = 5
			},
			new Product {
				ID = 2, Name = "Nvidia RTX 5080",
				Description = "Gear up for game-changing experiences with the NVIDIA® GeForce RTX™ 5080 and AI-powered DLSS 4. " +
				"Built with NVIDIA Blackwell and equipped with blistering-fast GDDR7 memory, " +
				"it lets you run the most graphically demanding games and creative applications with stunning fidelity and performance. " +
				"With NVIDIA Studio you can bring your creative projects to life faster than ever.",
				Category = new Category{ ID = 4, Name = "GPU/Graphics Card" }, Price = 999M, Stock = 7
			},
			new Product {
				ID = 3, Name = "Nvidia RTX 5070Ti",
				Description = "Get game-changing performance with the GeForce RTX™ 5070 Ti and RTX 5070, powered by NVIDIA Blackwell. " +
				"Game at high frame rates with DLSS 4, supercharge your creativity with NVIDIA Studio, and enable new experiences with the power of AI.",
				Category = new Category{ ID = 4, Name = "GPU/Graphics Card" }, Price = 749M, Stock = 15
			},
			new Product {
				ID = 4, Name = "Nvidia RTX 5060Ti",
				Description = "The NVIDIA® GeForce RTX™ 5060 Ti and RTX 5060, " +
				"powered by the NVIDIA Blackwell architecture, enables game-changing AI capabilities in the latest games and apps. " +
				"Multiply performance with NVIDIA DLSS 4, enjoy realistic graphics with ray tracing, and take your creativity further with NVIDIA Studio.",
				Category = new Category{ ID = 4, Name = "GPU/Graphics Card" }, Price = 379M, Stock = 30
			},
			new Product {
				ID = 5, Name = "AMD Ryzen 9 9950x3D",
				Description = "The ultimate 16-core desktop CPU with 2nd gen AMD 3D V-Cache™ Technology that can do it all with incredible performance for the most demanding gamers and creators.",
				Category = new Category { ID = 1, Name = "CPU/Processor" }, Price = 699M, Stock = 5
			},
			new Product {
				ID = 6, Name = "AMD Ryzen 7 9800x3D",
				Description = "Harness the ultimate gaming edge with AMD Ryzen™ 7 9800X3D Processor." +
				" Enjoy faster gaming with 2nd gen AMD 3D V-Cache™ technology for low latency.",
				Category = new Category { ID = 1, Name = "CPU/Processor" }, Price = 479M, Stock = 10
			},
			new Product {
				ID = 7, Name = "AMD Ryzen 5 9600x",
				Description = "Pure Gaming Performance",
				Category = new Category{ ID = 1, Name = "CPU/Processor" }, Price = 279M, Stock = 15
			},
			new Product {
				ID = 8, Name = "FRAME 4000D Modular Mid-Tower PC Case",
				Description = "The CORSAIR FRAME 4000D is a fully-modular and spacious mid-tower PC case featuring InfiniRail™ Fan Mounting System and compatible with reverse connector motherboards.",
				Category = new Category { ID = 5, Name = "Computer Case"}, Price = 94.99M, Stock = 18
			},
			new Product {
				ID = 9, Name = "HX1500i Fully Modular Ultra-Low Noise Platinum ATX 1500 Watt PC Power Supply",
				Description = "ATX 3.1 CERTIFIED, QUIET OPERATION WITH ZERO RPM MODE, 100% JAPANESE CAPACITORS",
				Category = new Category { ID = 6, Name = "Power Supply"}, Price = 349.99M, Stock = 25
			},
			new Product {
				ID = 10, Name = "RMx Series RM850x Fully Modular Power Supply",
				Description = "Power your next build with a CORSAIR RMx Series fully modular PSU for quiet, Cybenetics Gold-certified efficient performance.",
				Category = new Category { ID = 6, Name = "Power Supply"}, Price = 149.99M, Stock = 25
			},
		};

	}
}
