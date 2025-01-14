using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;


namespace EcommerceDataLayer.Ropesitry
{
    public class OrderItemDataAccess
    {
        private readonly string _connectionString;
        public OrderItemDataAccess(ConnectionString connectionString)
        {
            _connectionString = connectionString.connectionString;
        }

        public int Add(OrderItemDTO orderItem)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_CreateOrderItem", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding parameters
                cmd.Parameters.AddWithValue("@OrderID", orderItem.OrderID);
                cmd.Parameters.AddWithValue("@ProductID", orderItem.ProductID);
                cmd.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
                cmd.Parameters.AddWithValue("@Price", orderItem.Price);


                // Open the connection and execute the command
                var result = cmd.ExecuteScalar(); // Executes and returns the OrderItemID

                return Convert.ToInt32(result); // Return the created OrderItemID
            }

        }


        public bool Update(OrderItemDTO orderItem)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateOrderItem", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                // Adding parameters
                cmd.Parameters.AddWithValue("@OrderItemID", orderItem.OrderItemID);
                cmd.Parameters.AddWithValue("@OrderID", orderItem.OrderID);
                cmd.Parameters.AddWithValue("@ProductID", orderItem.ProductID);
                cmd.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
                cmd.Parameters.AddWithValue("@Price", orderItem.Price);

                int rowsAffected = cmd.ExecuteNonQuery(); // Executes and returns number of rows affected

                return rowsAffected > 0; // Returns true if rows were affected
            }
        }


        public bool DeleteOrderItem(int orderItemID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteOrderItem", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding parameter
                cmd.Parameters.AddWithValue("@OrderItemID", orderItemID);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery(); // Executes and returns number of rows affected

                return rowsAffected > 0; // Returns true if rows were affected
            }
        }


        public List<OrderItemDTO> GetOrderItemsByOrderID(int orderID)
        {
            List<OrderItemDTO> orderItems = new List<OrderItemDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Open the database connection
                connection.Open();

                // Define the SqlCommand to call the stored procedure
                using (SqlCommand command = new SqlCommand("GetAllOrderItemsByOrderId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add the parameter for OrderID
                    command.Parameters.AddWithValue("@OrderID", orderID);

                    // Execute the stored procedure and get the result set
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Loop through the result set and create OrderItemDTO objects
                        while (reader.Read())
                        {
                            OrderItemDTO orderItem = new OrderItemDTO
                            {
                                OrderItemID = reader.GetInt32(0), // OrderItemID
                                OrderID = reader.GetInt32(1), // OrderID
                                ProductID = reader.GetInt32(2), // ProductID
                                Quantity = reader.GetInt32(3), // Quantity
                                Price = reader.GetDecimal(4), // Price
                                TotalItemsPrice = reader.GetDecimal(5) // TotalItemsPrice
                            };
                            // Add the order item to the list
                            orderItems.Add(orderItem);
                        }
                    }
                }
            }

            return orderItems;
        }


        public void DeleteAllOrderItemsByOrderId(int orderID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DeletedAllOrderItemsByOrderId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", orderID);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} order items deleted for OrderID: {orderID}");
                }
            }
        }

        public int UpdateOrderItemQuantity(int orderItemID, int quantity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_UpdateOrderItemQuantity", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters for the stored procedure
                    command.Parameters.Add(new SqlParameter("@OrderItemID", SqlDbType.Int)).Value = orderItemID;
                    command.Parameters.Add(new SqlParameter("@Quantity", SqlDbType.Int)).Value = quantity;

                    int rowsAffectedParam = Convert.ToInt32(command.ExecuteScalar());
                    return rowsAffectedParam;
                }

            }
        }
    }



}