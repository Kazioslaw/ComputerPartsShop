using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class ReviewRepository : ICRUDRepository<Review, int>
	{
		public Task<List<Review>> GetList()
		{
			throw new NotImplementedException();
		}

		public Task<Review> Get(int id)
		{
			throw new NotImplementedException();
		}

		public Task<int> Create(Review request)
		{
			throw new NotImplementedException();
		}

		public Task<Review> Update(int id, Review request)
		{
			throw new NotImplementedException();
		}

		public Task Delete(int id)
		{
			return Task.CompletedTask;
		}
	}
}
