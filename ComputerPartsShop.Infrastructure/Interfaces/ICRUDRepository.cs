namespace ComputerPartsShop.Infrastructure
{
	interface ICRUDRepository<T, TKey> where T : class
	{
		public List<T> GetList();
		public T Get(TKey id);
		public T Create(T entity);
		public T Update(TKey id, T entity);
		public void Delete(TKey id);
	}
}
