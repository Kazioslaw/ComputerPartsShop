namespace ComputerPartsShop.Infrastructure
{
	public interface IRepository<T, TKey> where T : class
	{
		public Task<List<T>> GetListAsync(CancellationToken ct);
		public Task<T> GetAsync(TKey id, CancellationToken ct);
		public Task<TKey> CreateAsync(T entity, CancellationToken ct);
		public Task<T> UpdateAsync(TKey id, T entity, CancellationToken ct);
		public Task DeleteAsync(TKey id, CancellationToken ct);
	}
}
