using EcommerceDataLayer.DTOS;
using EcommerceDataLayer.Ropesitry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLogicalLayer.Services
{
    public class ProductCategoryLogic
    {

        // Create Product Category
        public static void CreateProductCategory(string categoryName)
        {
            // Pass to server layer to insert the product category
            ProductCategoryData.CreateProductCategory(categoryName);
        }

        // Get Product Category by CategoryID
        public static DataTable GetProductCategory(int categoryId)
        {
            // Retrieve data from server layer
            return ProductCategoryData.GetProductCategory(categoryId);
        }

        // Update Product Category
        public static void UpdateProductCategory(int categoryId, string categoryName)
        {

            // Pass to server layer to update the product category
            ProductCategoryData.UpdateProductCategory(categoryId, categoryName);
        }

        // Delete Product Category
        public static void DeleteProductCategory(int categoryId)
        {
            // Pass to server layer to delete the product category
            ProductCategoryData.DeleteProductCategory(categoryId);
        }

        public static List<ProductCategoryDTO> GetAllProductCategories()
        {
            return ProductCategoryData.GetAllProductCategories();
        }

        public static List<ProductCategoryDTO> SearchProductCategoriesByFirstChar(string firstChar)
        {
            return ProductCategoryData.SearchProductCategoriesByFirstChar(firstChar);
        }




    }
}