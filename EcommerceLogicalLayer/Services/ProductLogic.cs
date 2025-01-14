using EcommerceDataLayer.DTOS;
using EcommerceDataLayer.Ropesitry;


namespace EcommerceLogicalLayer.Services
{
    public class ProductLogic
    {
        public static List<ProductDTO> GetAllProducts()
        {
            return ProductDataAccess.GetAllProducts();
        }

        public static List<ProductDTO> GetProductsByFirstChar(string firstChar)
        {
            return ProductDataAccess.GetProductsByFirstChar(firstChar);
        }
        public static bool CreateProduct(ProductDTO product)
        {
            return ProductDataAccess.CreateProduct(product);
        }

        public static bool UpdateProduct(ProductDTO product)
        {
            return ProductDataAccess.UpdateProduct(product);
        }

        public static bool DeleteProduct(int productId)
        {
            return ProductDataAccess.DeleteProduct(productId);
        }

    }
}
