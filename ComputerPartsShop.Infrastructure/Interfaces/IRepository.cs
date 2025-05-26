namespace ComputerPartsShop.Infrastructure
{
	public interface IRepository<T, TKey> where T : class
	{
		public Task<List<T>> GetListAsync();
		public Task<T> GetAsync(TKey id);
		public Task<TKey> CreateAsync(T entity);
		public Task<T> UpdateAsync(TKey id, T entity);
		public Task DeleteAsync(TKey id);
	}
}
