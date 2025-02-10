namespace EcommerceDataLayer.Entities.Reviews
{


    public class ReviewRequest
    {
        public required string UserId { get; set; }
        public required int ProductId { get; set; }
        public required string ReviewText { get; set; }
        public int Rating { get; set; }
    
    }

    public class UpdateReviewRequest
    {
        public required string ReviewText { get; set; }
        public int Rating { get; set; }

    }




}
