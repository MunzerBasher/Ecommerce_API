using EcommerceDataLayer.Entities.Reviews;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IReviewsRopesitry
    {
     

        Task<List<ReviewResponse>> GetAllProductReviewsAsync(int ProductId);


        Task<List<ReviewResponse>> GetAllUserReviewsAsync(string UserId);


        Task<List<ReviewResponse>> GetAllUserInProductReviewsAsync(string UserId, int ProductId);


        Task<bool> AddAsync(ReviewRequest review);

        
        Task<bool> DeleteAsync( int ReviewId);


        Task<bool> IsExistAsync(int ReviewId);


        Task<bool> UpdateAsync(UpdateReviewRequest updateReviewRequest, int ReviewId);



    }

}
