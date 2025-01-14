

public class ShippingDataAccess
{
    private readonly string _connectionString;
    public ShippingDataAccess(ConnectionString connectionString)
    {
        _connectionString = connectionString.connectionString;
    }
    
    public int Add(ShippingDTO shippingDTO)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_CreateShipping", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Define input parameters from ShippingDTO
            cmd.Parameters.AddWithValue("@OrderID", shippingDTO.OrderID);
            cmd.Parameters.AddWithValue("@CarrierName", shippingDTO.CarrierName);
            cmd.Parameters.AddWithValue("@TrackingNumber", shippingDTO.TrackingNumber);
            cmd.Parameters.AddWithValue("@ShippingStatus", shippingDTO.ShippingStatus);
            cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", shippingDTO.EstimatedDeliveryDate);
            cmd.Parameters.AddWithValue("@ActualDeliveryDate", (object)shippingDTO.ActualDeliveryDate ?? DBNull.Value);

            // Output parameter to receive ShippingID
            int shippingID;
            SqlParameter outputParam = new SqlParameter("@OutputParam", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();

                // Get the output parameter value
                shippingID = (int)outputParam.Value;
               return shippingID;
            }
            catch (Exception ex)
            {
                throw new Exception();  // Set to -1 in case of an error

            }
        }
    }

    public  ShippingDTO GetById(int shippingID)
    {
        ShippingDTO shippingDTO = null;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_GetShippingByID", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Input parameter for ShippingID
            cmd.Parameters.AddWithValue("@ShippingID", shippingID);

            // Output parameter to return number of rows affected (affected rows = 1 if found)
            SqlParameter outputParam = new SqlParameter("@OutputParam", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();

                // Get the output parameter value (number of rows affected)
                int affectedRows = (int)outputParam.Value;
                if (affectedRows > 0)
                {
                    // Populate shippingDTO with the retrieved data
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        shippingDTO = new ShippingDTO
                        {
                            ShippingID = shippingID,
                            OrderID = Convert.ToInt32(reader["OrderID"]),
                            CarrierName = reader["CarrierName"].ToString(),
                            TrackingNumber = reader["TrackingNumber"].ToString(),
                            ShippingStatus = Convert.ToInt16(reader["ShippingStatus"]),
                            EstimatedDeliveryDate = Convert.ToDateTime(reader["EstimatedDeliveryDate"]),
                            ActualDeliveryDate = reader["ActualDeliveryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ActualDeliveryDate"])
                        };
                    }
                    reader.Close();
                }
                 return shippingDTO;
                
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }

    public int Update(ShippingDTO shippingDTO)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_UpdateShipping", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Define input parameters from ShippingDTO
            cmd.Parameters.AddWithValue("@ShippingID", shippingDTO.ShippingID);
            cmd.Parameters.AddWithValue("@OrderID", shippingDTO.OrderID);
            cmd.Parameters.AddWithValue("@CarrierName", shippingDTO.CarrierName);
            cmd.Parameters.AddWithValue("@TrackingNumber", shippingDTO.TrackingNumber);
            cmd.Parameters.AddWithValue("@ShippingStatus", shippingDTO.ShippingStatus);
            cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", shippingDTO.EstimatedDeliveryDate);
            cmd.Parameters.AddWithValue("@ActualDeliveryDate", (object)shippingDTO.ActualDeliveryDate ?? DBNull.Value);

            int affectedRows;
            // Output parameter to return the number of affected rows
            SqlParameter outputParam = new SqlParameter("@OutputParam", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();

                // Get the output parameter value (number of rows affected)
                affectedRows = (int)outputParam.Value;
                return affectedRows;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }


    public  int Delete(int shippingID )
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_DeleteShipping", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Input parameter for ShippingID
            cmd.Parameters.AddWithValue("@ShippingID", shippingID);

            // Output parameter to return the number of affected rows
            SqlParameter outputParam = new SqlParameter("@OutputParam", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);
            int affectedRows;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();

                // Get the output parameter value (number of rows affected)
                affectedRows = (int)outputParam.Value;
               return affectedRows;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }

    public  List<GetShippingDTO> GetAll()
    {
        List<GetShippingDTO> shippings = new List<GetShippingDTO>();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("GetAllShippings", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GetShippingDTO shipping = new GetShippingDTO
                        {
                            ShippingID = reader.GetInt32(reader.GetOrdinal("ShippingID")),
                            OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                            CarrierName = reader.GetString(reader.GetOrdinal("CarrierName")),
                            TrackingNumber = reader.GetString(reader.GetOrdinal("TrackingNumber")),
                            ShippingStatus = reader.GetString(reader.GetOrdinal("Shipping_Status")),
                            EstimatedDeliveryDate = reader.IsDBNull(reader.GetOrdinal("EstimatedDeliveryDate"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("EstimatedDeliveryDate")),
                            ActualDeliveryDate = reader.IsDBNull(reader.GetOrdinal("ActualDeliveryDate"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("ActualDeliveryDate"))
                        };
                        shippings.Add(shipping);
                    }
                }
            }
        }

        return shippings;
    }

    public  bool FindShipping(int shippingID)
    {
        bool shippingExists = false;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("CheckShippingIDExists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                shippingExists = Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }
        return shippingExists;
    }

    public ShippingOwnerDTO? GetShippingOwnerDetails(int ownerId)
    {
        ShippingOwnerDTO? shippingOwnerDetails = null;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("ShippingOwner", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@OwnerID", SqlDbType.Int)).Value = ownerId;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) 
                        {
                            shippingOwnerDetails = new ShippingOwnerDTO
                            {
                                UserName = reader["user_name"].ToString(),
                                UserEmail = reader["user_email"].ToString(),
                                UserPhone = reader["user_phone"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        return shippingOwnerDetails;
    }








}