using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly TempData _dbContext;
		public CategoryRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Category>> GetListAsync(CancellationToken ct)
		{
			await Task.Delay(500, ct);

			return _dbContext.CategoryList;
		}

		public async Task<Category> GetAsync(int id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var category = _dbContext.CategoryList.FirstOrDefault(c => c.Id == id);

			return category!;
		}

		public async Task<Category> GetByNameAsync(string name, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var category = _dbContext.CategoryList.FirstOrDefault(c => c.Name == name);

			return category!;
		}

		public async Task<int> CreateAsync(Category request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var last = _dbContext.CategoryList.OrderBy(x => x.Id).LastOrDefault();

			if (last == null)
			{
				request.Id = 1;
			}
			else
			{
				request.Id = last.Id + 1;
			}

			_dbContext.CategoryList.Add(request);

			return request.Id;
		}

		public async Task<Category> UpdateAsync(int id, Category request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var category = _dbContext.CategoryList.FirstOrDefault(c => c.Id == id);

			if (category != null)
			{
				category.Name = request.Name;
				category.Description = request.Description;
			}

			return category!;
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var category = _dbContext.CategoryList.FirstOrDefault(c => c.Id == id);

			if (category != null)
			{
				_dbContext.CategoryList.Remove(category);
			}
		}
	}
}
