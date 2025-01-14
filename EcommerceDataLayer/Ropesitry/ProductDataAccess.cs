using EcommerceDataLayer.DTOS;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;



namespace EcommerceDataLayer.Ropesitry
{
    public class ProductDataAccess
    {
        private readonly string _connectionString;
        public ProductDataAccess(ConnectionString connectionString)
        {
            _connectionString = connectionString.connectionString;
        }

        public List<ProductDTO> GetAll()
        {
            List<ProductDTO> categories = new List<ProductDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("GetAllProducts", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductDTO product = new ProductDTO
                            {
                                ProductID = Convert.ToInt32(reader["ProductID"]),  // Assuming this is in the result
                                ProductName = reader["ProductName"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                QuantityInStock = Convert.ToInt32(reader["QuantityInStock"]),
                                CategoryName = reader["CategoryName"].ToString(),
                            };
                            categories.Add(product);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle exception as needed
                    throw new Exception(ex.Message);
                }
            }

            return categories;
        }



        public List<ProductDTO> GetProductsByFirstChar(string firstChar)
        {
            List<ProductDTO> products = new List<ProductDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SearchForProductByFirstChar", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstChar", firstChar);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductDTO product = new ProductDTO
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
            }

            return products;
        }


        public bool Add(ProductDTO product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                connection.Open();
                SqlCommand command = new SqlCommand("CreateProduct", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Adding parameters to the stored procedure
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
                command.Parameters.AddWithValue("@CategoryID", product.CategoryId);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0; // Return true if the insert was successful


            }
        }


        public bool Update(ProductDTO product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("UpdateProduct", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the stored procedure
                    command.Parameters.AddWithValue("@ProductID", product.ProductID);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@Description", product.Description);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
                    command.Parameters.AddWithValue("@CategoryID", product.CategoryId);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0; // Return true if the update was successful
                }
                catch (Exception ex)
                {
                    // Handle exception (log it, show error message, etc.)
                    throw new Exception(ex.Message);
                }
            }
        }


        public bool Delete(int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DeleteProduct", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding the ProductID parameter to the stored procedure
                    command.Parameters.AddWithValue("@ProductID", productId);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0; // Return true if the delete was successful
                }
                catch (Exception ex)
                {
                    // Handle exception (log it, show error message, etc.)
                    throw new Exception(ex.Message);
                }
            }
        }





    }
}
