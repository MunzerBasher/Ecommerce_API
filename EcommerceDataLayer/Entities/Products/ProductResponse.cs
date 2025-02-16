namespace EcommerceDataLayer.Entities.Products
{
    public class ProductResponse
    {
        public int ProductID { get; set; }
        public required string ProductName { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ImagesUrl { get; set; } = string.Empty; 
        public int ImageId { get; set; }
    }


    public class ImagesUrlResponse
    {
        public int ImageID { get; set; }
        public string ImageURL { get; set; } = string.Empty;
    }





    public class ProductRequest
    {
        public required string ProductName { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int CategoryId { get; set; }
    }








}