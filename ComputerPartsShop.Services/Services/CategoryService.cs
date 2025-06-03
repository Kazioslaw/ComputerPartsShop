using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;

		public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
		}

		public async Task<List<CategoryResponse>> GetListAsync(CancellationToken ct)
		{
			var result = await _categoryRepository.GetListAsync(ct);

			var categoryList = _mapper.Map<IEnumerable<CategoryResponse>>(result);

			return categoryList.ToList();
		}

		public async Task<CategoryResponse> GetAsync(int id, CancellationToken ct)
		{
			var result = await _categoryRepository.GetAsync(id, ct);

			if (result == null)
			{
				return null;
			}

			var category = _mapper.Map<CategoryResponse>(result);

			return category;
		}

		public async Task<CategoryResponse> GetByNameAsync(string name, CancellationToken ct)
		{
			var result = await _categoryRepository.GetByNameAsync(name, ct);

			if (result == null)
			{
				return null;
			}

			var category = _mapper.Map<CategoryResponse>(result);

			return category;
		}

		public async Task<CategoryResponse> CreateAsync(CategoryRequest entity, CancellationToken ct)
		{
			var newCategory = _mapper.Map<Category>(entity);

			var result = await _categoryRepository.CreateAsync(newCategory, ct);

			if (result == null)
			{
				return null;
			}

			var createdCategory = _mapper.Map<CategoryResponse>(result);

			return createdCategory;
		}

		public async Task<CategoryResponse> UpdateAsync(int id, CategoryRequest entity, CancellationToken ct)
		{
			var categoryToUpdate = _mapper.Map<Category>(entity);


			var result = await _categoryRepository.UpdateAsync(id, categoryToUpdate, ct);

			if (result == null)
			{
				return null;
			}

			var updatedCategory = _mapper.Map<CategoryResponse>(result);

			return updatedCategory;
		}

		public async Task<bool> DeleteAsync(int id, CancellationToken ct)
		{
			return await _categoryRepository.DeleteAsync(id, ct);
		}
	}
}
