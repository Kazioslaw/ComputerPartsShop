using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

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
			var result = await _productRepository.GetListAsync(ct);

			var productList = _mapper.Map<IEnumerable<ProductResponse>>(result);

			return productList.ToList();
		}

		public async Task<ProductResponse> GetAsync(int id, CancellationToken ct)
		{
			var result = await _productRepository.GetAsync(id, ct);

			if (result == null)
			{
				return null;
			}

			var product = _mapper.Map<ProductResponse>(result);

			return product;
		}

		public async Task<ProductResponse> CreateAsync(ProductRequest entity, CancellationToken ct)
		{
			var category = await _categoryRepository.GetByNameAsync(entity.CategoryName, ct);

			var newProduct = _mapper.Map<Product>(entity);
			newProduct.CategoryId = category.Id;
			newProduct.Category = category;

			var result = await _productRepository.CreateAsync(newProduct, ct);

			if (result == null)
			{
				return null;
			}

			var createdProduct = _mapper.Map<ProductResponse>(result);

			return createdProduct;
		}

		public async Task<ProductResponse> UpdateAsync(int id, ProductRequest entity, CancellationToken ct)
		{
			var category = await _categoryRepository.GetByNameAsync(entity.CategoryName, ct);

			var productToUpdate = _mapper.Map<Product>(entity);
			productToUpdate.CategoryId = category.Id;
			productToUpdate.Category = category;

			var result = await _productRepository.UpdateAsync(id, productToUpdate, ct);

			if (result == null)
			{
				return null;
			}

			var updatedProduct = _mapper.Map<ProductResponse>(result);

			return updatedProduct;
		}

		public async Task<bool> DeleteAsync(int id, CancellationToken ct)
		{
			return await _productRepository.DeleteAsync(id, ct);
		}
	}
}
