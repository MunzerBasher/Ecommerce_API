using EcommerceDataLayer.DTOS;


namespace EcommerceLogicalLayer.Services
{
    public class ReviewLogic
    {

        public static List<Review> GetAllReviews()
        {
            return ReviewDataAccess.GetAllReviews();
        }

        public static void CreateReview(ReviewDTOForCreate review)
        {
            ReviewDataAccess.Add(review);
        }


        public static void DeleteReview(int reviewID)
        {
            ReviewDataAccess.Delete(reviewID);
        }


        public static void UpdateReview(ReviewDTOForUpdate review)
        {
            ReviewDataAccess.Update(review);
        }

    }


}