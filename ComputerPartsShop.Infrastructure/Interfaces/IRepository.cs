namespace ComputerPartsShop.Infrastructure
{
	public interface IRepository<T, TKey> where T : class
	{
		public Task<List<T>> GetList();
		public Task<T> Get(TKey id);
		public Task<TKey> Create(T entity);
		public Task<T> Update(TKey id, T entity);
		public Task Delete(TKey id);
	}
}
