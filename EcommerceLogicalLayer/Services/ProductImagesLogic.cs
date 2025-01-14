using EcommerceDataLayer.DTOS;
using EcommerceDataLayer.Ropesitry;


namespace EcommerceLogicalLayer.Services
{
    public class ProductImagesLogic
    {

        public ProductImagesLogic(Mode mode, ProductImageDTO ProductImage)
        {
            this.mode = mode;
            this.ProductImage = ProductImage;
        }



        public enum Mode { New = 1, Update = 2 };
        public Mode mode { get; set; }
        public ProductImageDTO ProductImage;
        private int CreateProductImage(ProductImageDTO productImage)
        {
            return ProductImagesDataAccess.CreateProductImage(productImage);
        }

        public static List<ProductImageDTO> ReadProductImages(int productId)
        {
            return ProductImagesDataAccess.ReadProductImages(productId);
        }

        private int UpdateProductImage(ProductImageDTO productImage)
        {
            return ProductImagesDataAccess.UpdateProductImage(productImage);
        }

        public static int DeleteProductImage(int imageId)
        {
            return ProductImagesDataAccess.DeleteProductImage(imageId);
        }


        public int Save()
        {
            switch (mode)
            {
                case Mode.New:
                    return CreateProductImage(ProductImage);
                case Mode.Update:
                    return UpdateProductImage(ProductImage);
            }
            return 0;
        }



    }
}