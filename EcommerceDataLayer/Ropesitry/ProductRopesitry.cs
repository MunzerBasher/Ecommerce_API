using EcommerceDataLayer.DTOS;
using EcommerceDataLayer.IRopesitry;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;



namespace EcommerceDataLayer.Ropesitry
{
    public class ProductRopesitry : IProductRopesitry
    {
        private readonly string _connectionString;

        public ProductRopesitry(ConnectionString connectionString)
        {
            _connectionString = connectionString.connectionString;
        }

        public async Task<List<ProductResponse>> GetAllAsync()
        {
            List<ProductResponse> products = new List<ProductResponse>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("GetAllProducts", connection);
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ProductResponse product = new ProductResponse
                        {
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            QuantityInStock = reader.GetInt32(reader.GetOrdinal("QuantityInStock")),
                            CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),

                        };
                        products.Add(product);
                    }
                }
            }

            return products;
        }

        public async Task<List<ProductResponse>> GetProductsByFirstCharAsync(string firstChar)
        {
            List<ProductResponse> products = new List<ProductResponse>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("SearchForProductByFirstChar", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FirstChar", firstChar);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ProductResponse product = new ProductResponse
                        {
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            QuantityInStock = reader.GetInt32(reader.GetOrdinal("QuantityInStock")),
                            CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                        };

                        products.Add(product);
                    }
                }
            }

            return products;
        }

        public async Task<bool> AddAsync(ProductResponse product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("CreateProduct", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateAsync(ProductResponse product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("UpdateProduct", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", product.ProductID);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAsync(int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("DeleteProduct", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", productId);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
        }

        public Task<ProductResponse> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsExistAsync(int ProductID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("ProductIsEXISTS", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", ProductID);

                int rowsAffected = Convert.ToInt32(await command.ExecuteScalarAsync());

                return rowsAffected > 0;
            }
        }
    }

}