
using EcommerceDataLayer.DTOS;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IReviewsRopesitry
    {
        // Asynchronously retrieves all reviews.
        Task<List<Review>> GetAllAsync();

        // Asynchronously adds a new review.
        Task AddAsync(ReviewDTOForCreate review);

        // Asynchronously deletes a review by its ID.
        Task DeleteAsync(int reviewID);

        // Asynchronously updates an existing review.
        Task UpdateAsync(ReviewDTOForUpdate review);
    }

}
