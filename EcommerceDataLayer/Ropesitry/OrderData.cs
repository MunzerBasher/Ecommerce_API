using System.Data;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;

public class OrderDataAccess
{
    private readonly string _connectionString;
    public OrderDataAccess(ConnectionString connectionString)
    {
        _connectionString = connectionString.connectionString;
    }

    public int CreateOrder(OrderDTO order)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
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

                // Execute the stored procedure
                cmd.ExecuteNonQuery();

                // Get the value of the output parameter
                int result = (int)resultParameter.Value;
                return result;
            }
        }
    }

    public int UpdateOrder(OrderDTO order)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
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
                cmd.ExecuteNonQuery();

                // Get the value of the output parameter
                int result = (int)resultParameter.Value;
                return result;
            }
        }
    }

    public int DeleteOrder(int orderId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            try
            {
                connection.Open();
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
                    cmd.ExecuteNonQuery();

                    // Get the value of the output parameter
                    int result = (int)resultParameter.Value;
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return 0;
            }
        }
    }

    public List<OrderDTOWithUserName> GetAllOrders()
    {
        List<OrderDTOWithUserName> orders = new List<OrderDTOWithUserName>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            try
            {
                connection.Open();

                // Create a SqlCommand to call the stored procedure
                using (SqlCommand cmd = new SqlCommand("GetAllOrders", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Execute the stored procedure and retrieve the data
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Check if there are any rows
                    if (reader.HasRows)
                    {
                        while (reader.Read())
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
            catch (Exception ex)
            {
                // Handle exceptions (for example, log it or rethrow it);
                return orders; // Return null or an empty list depending on your needs
            }
        }

        return orders; // Return the list of orders
    }

    public  List<OrderDTOWithUserName> RecentlyOrders()
    {
        List<OrderDTOWithUserName> orders = new List<OrderDTOWithUserName>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            try
            {
                connection.Open();

                // Create a SqlCommand to call the stored procedure
                using (SqlCommand cmd = new SqlCommand("RecentlyOrders", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Execute the stored procedure and retrieve the data
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Check if there are any rows
                    if (reader.HasRows)
                    {
                        while (reader.Read())
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
            catch (Exception ex)
            {
                // Handle exceptions (for example, log it or rethrow it);
                return orders; // Return null or an empty list depending on your needs
            }
        }

        return orders; // Return the list of orders
    }

    public  int GetOrderTotalPrice(int OrderID)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("GetOrderTotalPrice", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", OrderID);
               

                // Execute the command and get the result
                object result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
    }

    public  int CountOrdersByStatus(int ordersStatus)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("CountOrdersByStatus", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrdersStatus", ordersStatus);

                // Execute the command and get the result
                object result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
    }

    public  int CountOrders()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("CountOrders", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Execute the command and get the result
                object result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
    }


}
