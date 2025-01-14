using EcommerceDataLayer.DTOS;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcommerceDataLayer.Ropesitry
{
    public class ProductImagesDataAccess
    {
        private readonly string _connectionString;
        public ProductImagesDataAccess(ConnectionString connectionString)
        {
            _connectionString = connectionString.connectionString;
        }
        public int CreateProductImage(ProductImageDTO productImage)
        {
            int imageId = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("CreateProductImage", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ImageURL", productImage.ImageURL);
                        cmd.Parameters.AddWithValue("@ProductID", productImage.ProductID);
                        cmd.Parameters.AddWithValue("@ImageOrder", productImage.ImageOrder);

                        // Output parameter to get the inserted ImageID
                        SqlParameter outputImageID = new SqlParameter("@ImageID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputImageID);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        // Get the output value
                        imageId = (int)outputImageID.Value;
                    }
                }

                return imageId;  // Return the ImageID
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public List<ProductImageDTO> ReadProductImages(int productId)
        {
            List<ProductImageDTO> productImages = new List<ProductImageDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetProductImagesByProductId", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductID", productId);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductImageDTO productImage = new ProductImageDTO
                                {
                                    ImageID = (int)reader["ImageID"],
                                    ImageURL = reader["ImageURL"].ToString(),
                                    ProductID = (int)reader["ProductID"],
                                    ImageOrder = (short)reader["ImageOrder"]
                                };
                                productImages.Add(productImage);
                            }
                        }
                    }
                }

                return productImages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public int UpdateProductImage(ProductImageDTO productImage)
        {
            int rowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateProductImage", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ImageID", productImage.ImageID);
                    cmd.Parameters.AddWithValue("@ImageURL", productImage.ImageURL);
                    cmd.Parameters.AddWithValue("@ProductID", productImage.ProductID);
                    cmd.Parameters.AddWithValue("@ImageOrder", productImage.ImageOrder);

                    // Output parameter to get the number of rows affected
                    SqlParameter outputRowsAffected = new SqlParameter("@RowsAffected", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputRowsAffected);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    // Get the number of rows affected
                    rowsAffected = (int)outputRowsAffected.Value;
                }
            }

            return rowsAffected;  // Return the number of rows affected
        }



        public int DeleteProductImage(int imageId)
        {
            int rowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteProductImage", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ImageID", imageId);

                    // Output parameter to get the number of rows affected
                    SqlParameter outputRowsAffected = new SqlParameter("@RowsAffected", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputRowsAffected);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    // Get the number of rows affected
                    rowsAffected = (int)outputRowsAffected.Value;
                }
            }

            return rowsAffected;  // Return the number of rows affected
        }



    }

}
