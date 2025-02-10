using EcommerceDataLayer.IRopesitry;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;


namespace EcommerceDataLayer.Ropesitry
{
  

    public class OrderItemRopesitry : IOrderItemsRopesitry
    {
        private readonly string _connectionString;
        public OrderItemRopesitry(ConnectionString connectionString)
        {
            _connectionString = connectionString.connectionString;
        }
        

        public async Task<bool> DeleteAsync(int orderItemID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteOrderItem", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Adding parameter
                cmd.Parameters.AddWithValue("@OrderItemID", orderItemID);

                await conn.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<List<OrderItemDTO>> GetAllAsync(int orderID)
        {
            List<OrderItemDTO> orderItems = new List<OrderItemDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("GetAllOrderItemsByOrderId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", orderID);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            OrderItemDTO orderItem = new OrderItemDTO
                            {
                                OrderItemID = reader.GetInt32(0),
                                OrderID = reader.GetInt32(1),
                                ProductID = reader.GetInt32(2),
                                Quantity = reader.GetInt32(3),
                                Price = reader.GetDecimal(4),
                                TotalItemsPrice = reader.GetDecimal(5)
                            };
                            orderItems.Add(orderItem);
                        }
                    }
                }
            }

            return orderItems;
        }



        public async Task DeleteAllAsync(int orderID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("DeletedAllOrderItemsByOrderId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", orderID);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"{rowsAffected} order items deleted for OrderID: {orderID}");
                }
            }
        }



        public async Task<int> UpdateOrderItemQuantityAsync(int orderItemID, int quantity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("sp_UpdateOrderItemQuantity", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@OrderItemID", SqlDbType.Int)).Value = orderItemID;
                    command.Parameters.Add(new SqlParameter("@Quantity", SqlDbType.Int)).Value = quantity;

                    int rowsAffected = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return rowsAffected;
                }
            }
        }



        public async Task<bool> IsExistAsync(int ProductID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("ItemIsEXISTS", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ItemID", ProductID);

                int rowsAffected = Convert.ToInt32(await command.ExecuteScalarAsync());

                return rowsAffected > 0;
            }
        }
    }



}