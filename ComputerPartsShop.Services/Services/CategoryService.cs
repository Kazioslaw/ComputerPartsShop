using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
using Microsoft.Data.SqlClient;

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
			try
			{
				var result = await _categoryRepository.GetListAsync(ct);

				var categoryList = _mapper.Map<IEnumerable<CategoryResponse>>(result);

				return categoryList.ToList();
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<CategoryResponse> GetAsync(int id, CancellationToken ct)
		{
			try
			{
				var result = await _categoryRepository.GetAsync(id, ct);

				if (result == null)
				{
					throw new DataErrorException(404, "Category not found");
				}

				var category = _mapper.Map<CategoryResponse>(result);

				return category;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}

		}

		public async Task<CategoryResponse> GetByNameAsync(string name, CancellationToken ct)
		{
			try
			{
				var result = await _categoryRepository.GetByNameAsync(name, ct);

				if (result == null)
				{
					throw new DataErrorException(404, "Category not found");
				}

				var category = _mapper.Map<CategoryResponse>(result);

				return category;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<CategoryResponse> CreateAsync(CategoryRequest entity, CancellationToken ct)
		{
			try
			{
				var newCategory = _mapper.Map<Category>(entity);

				var result = await _categoryRepository.CreateAsync(newCategory, ct);

				var createdCategory = _mapper.Map<CategoryResponse>(result);

				return createdCategory;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<CategoryResponse> UpdateAsync(int id, CategoryRequest entity, CancellationToken ct)
		{
			try
			{
				var existingCategory = await _categoryRepository.GetAsync(id, ct);

				if (existingCategory == null)
				{
					throw new DataErrorException(404, "Category not found");
				}

				var categoryToUpdate = _mapper.Map<Category>(entity);


				var result = await _categoryRepository.UpdateAsync(id, categoryToUpdate, ct);

				if (result == null)
				{
					throw new DataErrorException(500, "Database operation failed");
				}

				var updatedCategory = _mapper.Map<CategoryResponse>(result);

				return updatedCategory;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			try
			{
				var category = await _categoryRepository.GetAsync(id, ct);

				if (category == null)
				{
					throw new DataErrorException(404, "Category not found");
				}

				await _categoryRepository.DeleteAsync(id, ct);
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}
	}
}
