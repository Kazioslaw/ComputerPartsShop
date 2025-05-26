namespace ComputerPartsShop.Services
{
	public interface IService<TRequest, TSimpleResponse, TDetailResponse, TKey> where TRequest : class where TSimpleResponse : class where TDetailResponse : class
	{
		public Task<List<TSimpleResponse>> GetList();
		public Task<TDetailResponse> Get(TKey id);
		public Task<TSimpleResponse> Create(TRequest entity);
		public Task<TSimpleResponse> Update(TKey id, TRequest entity);
		public Task Delete(TKey id);
	}
}
