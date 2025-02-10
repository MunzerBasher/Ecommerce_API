using EcommerceDataLayer.Entities.Items;
using EcommerceDataLayer.IRopesitry;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;


namespace EcommerceDataLayer.Ropesitry
{
    public class ShareItemsOrdersRopesitry(ConnectionString connectionString) : IShareItemsOrdersRopesitry
    {
        private readonly string _connectionString = connectionString.connectionString;


        public async Task<int> AddItemAsync(ItemRequest orderItem, int orderId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_CreateOrderItem", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@ProductID", orderItem.ProductId);
                cmd.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
                var result = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
        }


        public async Task<bool> IsOrderExistAsync(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("OrderIsEXISTS", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderID", orderId);

                int rowsAffected = Convert.ToInt32(await command.ExecuteScalarAsync());

                return rowsAffected > 0;
            }
        }
    }
}
