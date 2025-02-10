
namespace EcommerceDataLayer.DTOS
{
    public class CategoryResponse
    {
        public int CategoryID { get; set; }
        public required string CategoryName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }


    public class CategoryRequest
    {
        public required string CategoryName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
