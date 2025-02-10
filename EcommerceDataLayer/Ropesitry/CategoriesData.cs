using EcommerceDataLayer.Entities.Categories;
using EcommerceDataLayer.IRopesitry;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;



namespace EcommerceDataLayer.Ropesitry
{
    public class CategoriesData : ICategoriesRopesitry
    {
        private readonly string _connectionString;
        private readonly ILogger<CategoriesData> _logger ;

        public CategoriesData(ConnectionString connectionString, ILogger<CategoriesData> logger)
        {
            _connectionString = connectionString.connectionString;
            _logger = logger;
        }

        public async Task<bool> AddAsync(CategoryRequest  categoryRequest)
        {
            var row = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("CreateProductCategory", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryName", categoryRequest.CategoryName);
                cmd.Parameters.AddWithValue("@ImageUrl", categoryRequest.ImageUrl);
                await conn.OpenAsync();
                row =  await cmd.ExecuteNonQueryAsync();  
            }

            return row > 0;
        }

        public async Task<CategoryResponse> GetByIdAsync(int categoryId)
        {
            CategoryResponse category = null!;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("GetProductCategory", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryID", categoryId);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (!await reader.ReadAsync())
                    {
                        return null!;
                    }

                    category = new CategoryResponse
                    {
                        ImageUrl = reader["ImageUrl"].ToString()!,
                        CategoryID = Convert.ToInt32(reader["CategoryId"]),
                        CategoryName = reader["CategoryName"].ToString()!
                    };

                }
            }



            return category;
        }

        public async Task<CategoryResponse> GetByNameAsync(string categoryName)
        {
            CategoryResponse category = null!;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("GetProductCategoryByName", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryName", categoryName);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (!await reader.ReadAsync())
                    {
                        return null!;
                    }

                    category = new CategoryResponse
                    {
                        ImageUrl = reader["ImageUrl"].ToString()!,
                        CategoryID = Convert.ToInt32(reader["CategoryId"]),
                        CategoryName = reader["CategoryName"].ToString()!
                    };

                }
            }



            return category;
        }

        public async Task<bool> UpdateAsync(int categoryId, CategoryRequest categoryRequest)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateProductCategory", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                cmd.Parameters.AddWithValue("@CategoryName", categoryRequest.CategoryName);
                cmd.Parameters.AddWithValue("@ImageUrl", categoryRequest.ImageUrl);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();  // Execute the stored procedure asynchronously
            }

            return true; // Return true if the update was successful
        }

        public async Task<bool> ToggleStatusAsync(int categoryId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteProductCategory", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryID", categoryId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();  // Execute the stored procedure asynchronously
            }

            return true; // Return true if the deletion was successful
        }

        public async Task<List<CategoryResponse>> GetAllAsync()
        {
            List<CategoryResponse> categories = new List<CategoryResponse>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("GetAllProductCategory", connection);
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        CategoryResponse category = new CategoryResponse
                        {
                            ImageUrl = reader["ImageUrl"].ToString()!,
                            CategoryID = Convert.ToInt32(reader["CategoryId"]),
                            CategoryName = reader["CategoryName"].ToString()!,
                            // Map other columns as needed
                        };
                        categories.Add(category);
                    }
                }
            }

            return categories;
        }

        public async Task<List<CategoryResponse>> SearchAsync(string firstChar)
        {
            List<CategoryResponse> categories = new List<CategoryResponse>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SearchProductCategoriesByFirstChar", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FirstChar", firstChar);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        CategoryResponse category = new CategoryResponse
                        {
                            ImageUrl = reader["ImageUrl"].ToString()!,
                            CategoryID = Convert.ToInt32(reader["CategoryId"]),
                            CategoryName = reader["CategoryName"].ToString()!
                        };
                        categories.Add(category);
                    }
                }
            }

            return categories;
        }

  
        public async Task<bool> IsExistAsync(int CategoryId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("CategoryIsEXISTS", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryID", CategoryId);
                int rowsAffected = Convert.ToInt32(await command.ExecuteScalarAsync());
                return rowsAffected > 0;
            }
        }

        public async Task<bool> IsExistNameAsync(string Name)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("CategoryNameIsEXISTS", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", Name);
                int rowsAffected = Convert.ToInt32(await command.ExecuteScalarAsync());
                return rowsAffected > 0;
            }
        }

    }
}
