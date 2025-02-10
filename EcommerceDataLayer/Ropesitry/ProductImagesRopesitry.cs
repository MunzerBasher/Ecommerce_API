using EcommerceDataLayer.Entities.Products;
using EcommerceDataLayer.IRopesitry;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcommerceDataLayer.Ropesitry
{
    public class ProductImagesRopesitry : IProductImagesRopesitry
    {
        private readonly string _connectionString;

        public ProductImagesRopesitry(ConnectionString connectionString)
        {
            _connectionString = connectionString.connectionString;
        }

        public async Task<int> AddAsync(ProductImageRequest productImage)
        {
            int imageId = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("CreateProductImage", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ImageURL", productImage.ImageURL);
                    cmd.Parameters.AddWithValue("@ProductID", productImage.ProductID);
                    SqlParameter outputImageID = new SqlParameter("@ImageID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputImageID);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    imageId = (int)outputImageID.Value;
                }
            }

            return imageId; 
        }

        public async Task<List<ProductImageResponse>> GetAllAsync(int productId)
        {
            List<ProductImageResponse> productImages = new List<ProductImageResponse>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetProductImagesByProductId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductID", productId);

                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ProductImageResponse productImage = new ProductImageResponse
                            {
                                ImageID = (int)reader["ImageID"],
                                ImageURL = reader["ImageURL"].ToString()!,
                                ProductID = (int)reader["ProductID"],
                            };
                            productImages.Add(productImage);
                        }
                    }
                }
            }

            return productImages;
        }

        

        public async Task<int> DeleteAsync(int imageId)
        {
            int rowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteProductImage", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ImageID", imageId);
                    SqlParameter outputRowsAffected = new SqlParameter("@RowsAffected", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputRowsAffected);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    rowsAffected = (int)outputRowsAffected.Value;
                }
            }

            return rowsAffected; 
        }






        public async Task<string> GetImageURL(int @ImageId)
        {
            var ImageURL = "";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("GetImageURL", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ImageId", ImageId);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (!await reader.ReadAsync())
                    {
                        return null!;
                    }

                    ImageURL = Convert.ToString(await cmd.ExecuteScalarAsync());

                }
            }
            return ImageURL!;
        }





        public async Task<bool> IsExistAsync(int @ImageId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("ImageIsEXISTS", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ImageId", @ImageId);

                int rowsAffected = Convert.ToInt32(await command.ExecuteScalarAsync());

                return rowsAffected > 0;
            }


        }








        }


}