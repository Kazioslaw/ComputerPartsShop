using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IReviewService : IService<ReviewRequest, ReviewResponse, ReviewResponse, int>
	{
	}
}
