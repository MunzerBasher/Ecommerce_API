namespace EcommerceDataLayer.Entities.Products
{
    public class ProductImageResponse
    {
        public int ImageID { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public int ProductID { get; set; }
    }

     public class ProductImageRequest
    {
        public string ImageURL { get; set; } = string.Empty;
        public int ProductID { get; set; }
    }

}