
namespace EcommerceDataLayer.Entities.Reviews
{
   

    public class ReviewResponse
    {
        public required string UserId { get; set; }
        public int ReviewId { get; set; }
        public required string ReviewText { get; set; }
        public required int ProductId { get; set; }
        public required int Rating { get; set; }
        public required DateTime ReviewDate { get; set; }
    }


}
