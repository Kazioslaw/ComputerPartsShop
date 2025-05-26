using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class CategoryRepository : ICategoryRepository
	{
		readonly TempData _dbContext;
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
			var category = _dbContext.CategoryList.FirstOrDefault(c => c.ID == id);

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
			var last = _dbContext.CategoryList.OrderBy(x => x.ID).LastOrDefault();

			if (last == null)
			{
				request.ID = 1;
			}
			else
			{
				request.ID = last.ID + 1;
			}

			_dbContext.CategoryList.Add(request);

			return request.ID;
		}

		public async Task<Category> UpdateAsync(int id, Category request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var category = _dbContext.CategoryList.FirstOrDefault(c => c.ID == id);

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
			var category = _dbContext.CategoryList.FirstOrDefault(c => c.ID == id);

			if (category != null)
			{
				_dbContext.CategoryList.Remove(category);
			}
		}
	}
}
