using EcommerceDataLayer.DTOS;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;




namespace EcommerceDataLayer.Ropesitry
{
    public class DiscountDataAccess
    {
        private readonly string _connectionString;
        public DiscountDataAccess(ConnectionString connectionString)
        {
            _connectionString = connectionString.connectionString;
        }

        public bool AddDiscount(DiscountDTO discountDto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("AddDiscount", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProductId", discountDto.ProductId);
                    cmd.Parameters.AddWithValue("@StartDate", discountDto.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", discountDto.EndDate);
                    cmd.Parameters.AddWithValue("@DiscountValue", discountDto.DiscountValue);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public bool DeleteDiscountByProductId(int productId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("DeleteDiscountByProductId", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting discount: {ex.Message}");
                    return false;
                }
            }
        }

        public List<DiscountDTO> GetDiscountsByProductId(int productId)
        {
            List<DiscountDTO> discounts = new List<DiscountDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("GetDiscountsByProductId", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DiscountDTO discount = new DiscountDTO
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            ProductId = Convert.ToInt32(reader["product_id"]),
                            StartDate = Convert.ToDateTime(reader["start_date"]),
                            EndDate = Convert.ToDateTime(reader["end_date"]),
                            DiscountValue = Convert.ToDecimal(reader["discount_value"])
                        };
                        discounts.Add(discount);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }

            return discounts;
        }
    }


}
