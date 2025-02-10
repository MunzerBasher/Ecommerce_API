using System.Data;
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
                cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int)).Value = order.UserID;
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
                cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int)).Value = order.UserID;
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

    public async Task<List<OrderDTOWithUserName>> GetAllAsync()
    {
        List<OrderDTOWithUserName> orders = new List<OrderDTOWithUserName>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Create a SqlCommand to call the stored procedure
            using (SqlCommand cmd = new SqlCommand("GetAllOrders", connection))
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
                            // Map the results to OrderDTO
                            OrderDTOWithUserName order = new OrderDTOWithUserName
                            {
                                OrderID = Convert.ToInt32(reader["OrderID"]),
                                OrderDate = (DateTime)reader["OrderDate"],
                                TotalAmount = (decimal)reader["TotalAmount"],
                                Status = Convert.ToInt32(reader["Status"]),
                                UserName = reader["user_name"].ToString()
                            };

                            // Add the order to the list
                            orders.Add(order);
                        }
                    }
                }
            }
        }

        return orders; // Return the list of orders
    }

    public async Task<List<OrderDTOWithUserName>> RecentlyOrdersAsync()
    {
        List<OrderDTOWithUserName> orders = new List<OrderDTOWithUserName>();

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
                            // Map the results to OrderDTO
                            OrderDTOWithUserName order = new OrderDTOWithUserName
                            {
                                OrderID = Convert.ToInt32(reader["OrderID"]),
                                OrderDate = (DateTime)reader["OrderDate"],
                                TotalAmount = (decimal)reader["TotalAmount"],
                                Status = Convert.ToInt32(reader["Status"]),
                                UserName = reader["user_name"].ToString()
                            };

                            // Add the order to the list
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
                object result = await command.ExecuteScalarAsync();
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
                object result = await command.ExecuteScalarAsync()!;
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
                object result = await command.ExecuteScalarAsync()!;
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
    }



    public async Task<bool> IsExistAsync(int ProductID)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand("OrderIsEXISTS", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@OrderID", ProductID);

            int rowsAffected = Convert.ToInt32(await command.ExecuteScalarAsync());

            return rowsAffected > 0;
        }
    }







}
