using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class ProductRepository : ICRUDRepository<Product, int>
	{
		readonly TempData _dbContext;

		public ProductRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Product>> GetList()
		{
			return _dbContext.ProductList;
		}

		public async Task<Product> Get(int id)
		{
			var product = _dbContext.ProductList.FirstOrDefault(x => x.ID == id);

			return product;
		}

		public async Task<int> Create(Product request)
		{
			var last = _dbContext.ProductList.OrderBy(x => x.ID).LastOrDefault();

			if (last == null)
			{
				request.ID = 1;
			}
			else
			{
				request.ID = last.ID + 1;
			}

			_dbContext.ProductList.Add(request);

			return request.ID;
		}

		public async Task<Product> Update(int id, Product request)
		{
			var product = _dbContext.ProductList.FirstOrDefault(x => x.ID == id);

			if (product != null)
			{
				product.Name = request.Name;
				product.Description = request.Description;
				product.UnitPrice = request.UnitPrice;
				product.Stock = request.Stock;
				product.CategoryID = request.CategoryID;
				product.InternalCode = request.InternalCode;
			}

			return product;
		}

		public async Task Delete(int id)
		{
			var product = _dbContext.ProductList.FirstOrDefault(x => x.ID == id);

			if (product != null)
			{
				_dbContext.ProductList.Remove(product);
			}
		}
	}
}
