using EcommerceDataLayer.Entities.Reviews;
using EcommerceLogicalLayer.Helpers;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IReviewsServices
    {
        Task<Result<List<ReviewResponse>>> GetAllProductReviewsAsync(int ProductId);


        Task<Result<List<ReviewResponse>>> GetAllUserReviewsAsync(string UserId);


        Task<Result<List<ReviewResponse>>> GetAllUserReviewsInProductAsync(string UserId, int ProductId);


        Task<Result<bool>>AddAsync(ReviewRequest review);


        Task<Result<bool>> DeleteAsync(int ReviewId);


        Task<bool> IsExistAsync(int ReviewId);


        Task<Result<bool>> UpdateAsync(UpdateReviewRequest updateReviewRequest, int ReviewId);


    }
}
