using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class ProductRepository : IProductRepository
	{
		private readonly TempData _dbContext;

		public ProductRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Product>> GetListAsync(CancellationToken ct)
		{
			await Task.Delay(500, ct);

			return _dbContext.ProductList;
		}

		public async Task<Product> GetAsync(int id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var product = _dbContext.ProductList.FirstOrDefault(x => x.Id == id);

			return product!;
		}

		public async Task<int> CreateAsync(Product request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var last = _dbContext.ProductList.OrderBy(x => x.Id).LastOrDefault();

			if (last == null)
			{
				request.Id = 1;
			}
			else
			{
				request.Id = last.Id + 1;
			}

			_dbContext.ProductList.Add(request);

			return request.Id;
		}

		public async Task<Product> UpdateAsync(int id, Product request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var product = _dbContext.ProductList.FirstOrDefault(x => x.Id == id);

			if (product != null)
			{
				product.Name = request.Name;
				product.Description = request.Description;
				product.UnitPrice = request.UnitPrice;
				product.Stock = request.Stock;
				product.CategoryId = request.CategoryId;
				product.InternalCode = request.InternalCode;
			}

			return product!;
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var product = _dbContext.ProductList.FirstOrDefault(x => x.Id == id);

			if (product != null)
			{
				_dbContext.ProductList.Remove(product);
			}
		}
	}
}
