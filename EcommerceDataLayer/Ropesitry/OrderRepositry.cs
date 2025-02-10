using System.Data;
using EcommerceDataLayer.Entities.Orders;
using EcommerceDataLayer.IRopesitry;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;

public class OrdersRepositry : IOrdersRepositry
{
    private readonly string _connectionString;

    public OrdersRepositry(ConnectionString connectionString)
    {
        _connectionString = connectionString.connectionString;
    }

    public async Task<int> AddAsync(OrderDTO order)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("CreateOrder", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters
                cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = order.UserID;
                cmd.Parameters.Add(new SqlParameter("@OrderDate", SqlDbType.DateTime)).Value = order.OrderDate;
                cmd.Parameters.Add(new SqlParameter("@TotalAmount", SqlDbType.SmallMoney)).Value = order.TotalAmount;
                cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.SmallInt)).Value = order.Status;
                SqlParameter resultParameter = new SqlParameter("@Result", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(resultParameter);

               
                await cmd.ExecuteNonQueryAsync();

              
                int result = (int)resultParameter.Value;
                return result;
            }
        }
    }

    public async Task<bool> UpdateAsync(OrderDTO order)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("UpdateOrder", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters
                cmd.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.Int)).Value = order.OrderID;
                cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = order.UserID;
                cmd.Parameters.Add(new SqlParameter("@OrderDate", SqlDbType.DateTime)).Value = order.OrderDate;
                cmd.Parameters.Add(new SqlParameter("@TotalAmount", SqlDbType.SmallMoney)).Value = order.TotalAmount;
                cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.SmallInt)).Value = order.Status;

                // Execute the command and return the result (1 for success, 0 for failure)
                SqlParameter resultParameter = new SqlParameter("@Result", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(resultParameter);

                // Execute the stored procedure
                await cmd.ExecuteNonQueryAsync();

                // Get the value of the output parameter
                int result = (int)resultParameter.Value;
                return result > 0;
            }
        }
    }

    public async Task<bool> DeleteAsync(int orderId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("DeleteOrder", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters
                cmd.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.Int)).Value = orderId;

                // Execute the command and return the result (1 for success, 0 for failure)
                SqlParameter resultParameter = new SqlParameter("@Result", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(resultParameter);

                // Execute the stored procedure
                await cmd.ExecuteNonQueryAsync();

                // Get the value of the output parameter
                int result = (int)resultParameter.Value;
                return result > 0;
            }
        }
    }

    public async Task<List<OrderResponse>> GetAllAsync()
    {
        List<OrderResponse> orders = new List<OrderResponse>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("GetAllOrders", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                           
                            OrderResponse order = new OrderResponse
                            {
                                OrderID = Convert.ToInt32(reader["OrderID"]),
                                OrderDate = (DateTime)reader["OrderDate"],
                                TotalAmount = (decimal)reader["TotalAmount"],
                                Status = Convert.ToInt32(reader["Status"]),
                                UserName = reader["UserName"].ToString()!
                            };

                          
                            orders.Add(order);
                        }
                    }
                }
            }
        }

        return orders; 
    }

    public async Task<List<OrderResponse>> RecentlyOrdersAsync()
    {
        List<OrderResponse> orders = new List<OrderResponse>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Create a SqlCommand to call the stored procedure
            using (SqlCommand cmd = new SqlCommand("RecentlyOrders", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Execute the stored procedure and retrieve the data
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    // Check if there are any rows
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            OrderResponse order = new OrderResponse
                            {
                                OrderID = Convert.ToInt32(reader["OrderID"]),
                                OrderDate = (DateTime)reader["OrderDate"],
                                TotalAmount = (decimal)reader["TotalAmount"],
                                Status = Convert.ToInt32(reader["Status"]),
                                UserName = reader["UserName"].ToString()!
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
        }

        return orders; 
    }

    public async Task<int> GetOrderTotalPriceAsync(int OrderID)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand command = new SqlCommand("GetOrderTotalPrice", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", OrderID);
                var result = await command.ExecuteScalarAsync();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
    }

    public async Task<int> CountOrdersByStatusAsync(int ordersStatus)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand command = new SqlCommand("CountOrdersByStatus", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrdersStatus", ordersStatus);
                var result = await command.ExecuteScalarAsync();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
    }

    public async Task<int> CountOrdersAsync()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand command = new SqlCommand("CountOrders", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                var result = await command.ExecuteScalarAsync()!;
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
    }

    public async Task<List<OrderResponse>> GetUserOrdersAsync(string UserId)
    {
        List<OrderResponse> orders = new List<OrderResponse>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("GetAllUserOrders", connection))
            {
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            OrderResponse order = new OrderResponse
                            {
                                OrderID = Convert.ToInt32(reader["OrderID"]),
                                OrderDate = (DateTime)reader["OrderDate"],
                                TotalAmount = (decimal)reader["TotalAmount"],
                                Status = Convert.ToInt32(reader["Status"]),
                                UserName = reader["UserName"].ToString()!
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
        }

        return orders; 
    }

}