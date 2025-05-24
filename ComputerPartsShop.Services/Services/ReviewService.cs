using ComputerPartsShop.Domain.DTOs;

namespace ComputerPartsShop.Services
{
	public class ReviewService : ICRUDService<ReviewRequest, ReviewResponse, ReviewResponse, int>
	{
		public List<ReviewResponse> GetList()
		{
			throw new NotImplementedException();
		}

		public ReviewResponse Get(int id)
		{
			throw new NotImplementedException();
		}

		public ReviewResponse Create(ReviewRequest request)
		{
			throw new NotImplementedException();
		}

		public ReviewResponse Update(int id, ReviewRequest request)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{
			return;
		}
	}
}
