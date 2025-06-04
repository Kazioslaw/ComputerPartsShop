using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
using Microsoft.Data.SqlClient;

namespace ComputerPartsShop.Services
{
	public class ProductService : IProductService
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;

		public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
			_mapper = mapper;
		}

		public async Task<List<ProductResponse>> GetListAsync(CancellationToken ct)
		{
			try
			{
				var result = await _productRepository.GetListAsync(ct);

				var productList = _mapper.Map<IEnumerable<ProductResponse>>(result);

				return productList.ToList();
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<ProductResponse> GetAsync(int id, CancellationToken ct)
		{
			try
			{
				var result = await _productRepository.GetAsync(id, ct);

				if (result == null)
				{
					throw new DataErrorException(404, "Product not found");
				}

				var product = _mapper.Map<ProductResponse>(result);

				return product;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<ProductResponse> CreateAsync(ProductRequest entity, CancellationToken ct)
		{
			try
			{
				var category = await _categoryRepository.GetByNameAsync(entity.CategoryName, ct);

				if (category == null)
				{
					throw new DataErrorException(400, "Invalid category name");
				}

				var newProduct = _mapper.Map<Product>(entity);
				newProduct.CategoryId = category.Id;
				newProduct.Category = category;

				var result = await _productRepository.CreateAsync(newProduct, ct);

				var createdProduct = _mapper.Map<ProductResponse>(result);

				return createdProduct;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<ProductResponse> UpdateAsync(int id, ProductRequest entity, CancellationToken ct)
		{
			try
			{
				var product = await _productRepository.GetAsync(id, ct);

				if (product == null)
				{
					throw new DataErrorException(404, "Product not found");
				}

				var category = await _categoryRepository.GetByNameAsync(entity.CategoryName, ct);

				if (category == null)
				{
					throw new DataErrorException(400, "Invalid category name");
				}

				var productToUpdate = _mapper.Map<Product>(entity);
				productToUpdate.CategoryId = category.Id;
				productToUpdate.Category = category;

				var result = await _productRepository.UpdateAsync(id, productToUpdate, ct);

				var updatedProduct = _mapper.Map<ProductResponse>(result);

				return updatedProduct;
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
				var product = await _productRepository.GetAsync(id, ct);

				if (product == null)
				{
					throw new DataErrorException(404, "Product not found");
				}

				await _productRepository.DeleteAsync(id, ct);
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}
	}
}
