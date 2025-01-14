using EcommerceDataLayer.DTOS;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceDataLayer.Ropesitry
{
    public class ProductCategoryData
    {
        private readonly string _connectionString;
        public ProductCategoryData(ConnectionString connectionString)
        {
            _connectionString = connectionString.connectionString;
        }

        public void Add(string categoryName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("CreateProductCategory", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CategoryName", categoryName);

                    conn.Open();
                    cmd.ExecuteNonQuery();  // Execute the stored procedure
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in CreateProductCategory: " + ex.Message);
            }
        }


        public DataTable GetById(int categoryId)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetProductCategory", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CategoryID", categoryId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);  // Fill the DataTable with data from the database
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetProductCategory: " + ex.Message);
            }

            return dt;
        }


        public void Update(int categoryId, string categoryName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("UpdateProductCategory", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                    cmd.Parameters.AddWithValue("@CategoryName", categoryName);

                    conn.Open();
                    cmd.ExecuteNonQuery();  // Execute the stored procedure
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateProductCategory: " + ex.Message);
            }
        }


        public void Delete(int categoryId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("DeleteProductCategory", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CategoryID", categoryId);

                    conn.Open();
                    cmd.ExecuteNonQuery();  // Execute the stored procedure
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in DeleteProductCategory: " + ex.Message);
            }
        }

        public List<ProductCategoryDTO> GetAll()
        {
            List<ProductCategoryDTO> categories = new List<ProductCategoryDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("GetAllProductCategory", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductCategoryDTO category = new ProductCategoryDTO
                            {
                                CategoryID = Convert.ToInt32(reader["CategoryId"]),
                                CategoryName = reader["CategoryName"].ToString(),
                                // Map other columns as needed
                            };
                            categories.Add(category);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle exception as needed
                    Console.WriteLine(ex.Message);
                }
            }

            return categories;
        }


        public List<ProductCategoryDTO> Search(string firstChar)
        {
            List<ProductCategoryDTO> categories = new List<ProductCategoryDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SearchProductCategoriesByFirstChar", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstChar", firstChar);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductCategoryDTO category = new ProductCategoryDTO
                            {
                                CategoryID = Convert.ToInt32(reader["CategoryId"]),
                                CategoryName = reader["CategoryName"].ToString()
                            };
                            categories.Add(category);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle exception as needed
                    Console.WriteLine(ex.Message);
                }
            }

            return categories;
        }



    }
}