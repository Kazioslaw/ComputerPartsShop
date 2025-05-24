namespace ComputerPartsShop.Services
{
	public interface ICRUDService<TRequest, TSimpleResponse, TDetailResponse, TKey> where TRequest : class where TSimpleResponse : class where TDetailResponse : class
	{
		public List<TSimpleResponse> GetList();
		public TDetailResponse Get(TKey id);
		public TSimpleResponse Create(TRequest entity);
		public TSimpleResponse Update(TKey id, TRequest entity);
		public void Delete(TKey id);
	}
}
