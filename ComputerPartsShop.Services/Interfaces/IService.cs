namespace ComputerPartsShop.Services
{
	public interface IService<TRequest, TSimpleResponse, TDetailResponse, TKey> where TRequest : class where TSimpleResponse : class where TDetailResponse : class
	{
		public Task<List<TSimpleResponse>> GetListAsync();
		public Task<TDetailResponse> GetAsync(TKey id);
		public Task<TSimpleResponse> CreateAsync(TRequest entity);
		public Task<TSimpleResponse> UpdateAsync(TKey id, TRequest entity);
		public Task DeleteAsync(TKey id);
	}
}
