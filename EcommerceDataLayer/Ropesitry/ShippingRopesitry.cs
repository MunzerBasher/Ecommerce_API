using EcommerceDataLayer.IRopesitry;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;




public class ShippingRopesitry : IShippingRopesitry
{
    private readonly string _connectionString;

    public ShippingRopesitry(ConnectionString connectionString)
    {
        _connectionString = connectionString.connectionString;
    }

    public async Task<int> AddAsync(ShippingRequest shippingDTO)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_CreateShipping", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@OrderID", shippingDTO.OrderID);
            cmd.Parameters.AddWithValue("@CarrierName", shippingDTO.CarrierName);
            cmd.Parameters.AddWithValue("@TrackingNumber", shippingDTO.TrackingNumber);
            cmd.Parameters.AddWithValue("@ShippingStatus", shippingDTO.ShippingStatus);
            cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", shippingDTO.EstimatedDeliveryDate);
            cmd.Parameters.AddWithValue("@ActualDeliveryDate", (object)shippingDTO.ActualDeliveryDate! ?? DBNull.Value);

            SqlParameter outputParam = new SqlParameter("@OutputParam", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)outputParam.Value;
        }
    }

    public async Task<ShippingResponse> GetByIdAsync(int shippingID)
    {
        ShippingResponse shippingDTO = null!;

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_GetShippingByID", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@ShippingID", shippingID);

            SqlParameter outputParam = new SqlParameter("@OutputParam", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            int affectedRows = (int)outputParam.Value;
            if (affectedRows > 0)
            {
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    shippingDTO = new ShippingResponse
                    {
                        ShippingID = shippingID,
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        CarrierName = reader["CarrierName"].ToString()!,
                        TrackingNumber = reader["TrackingNumber"].ToString()!,
                        ShippingStatus = reader["ShippingStatus"].ToString()!,
                        EstimatedDeliveryDate = Convert.ToDateTime(reader["EstimatedDeliveryDate"]),
                        ActualDeliveryDate = reader["ActualDeliveryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ActualDeliveryDate"])
                    };
                }
                reader.Close();
            }
        }

        return shippingDTO;
    }

    public async Task<int> UpdateAsync(int ShippingID, ShippingRequest  shippingDTO)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_UpdateShipping", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@ShippingID",ShippingID);
            cmd.Parameters.AddWithValue("@OrderID", shippingDTO.OrderID);
            cmd.Parameters.AddWithValue("@CarrierName", shippingDTO.CarrierName);
            cmd.Parameters.AddWithValue("@TrackingNumber", shippingDTO.TrackingNumber);
            cmd.Parameters.AddWithValue("@ShippingStatus", shippingDTO.ShippingStatus);
            cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", shippingDTO.EstimatedDeliveryDate);
            cmd.Parameters.AddWithValue("@ActualDeliveryDate", (object)shippingDTO.ActualDeliveryDate! ?? DBNull.Value);

            SqlParameter outputParam = new SqlParameter("@OutputParam", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)outputParam.Value;
        }
    }

    public async Task<int> DeleteAsync(int shippingID)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_DeleteShipping", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@ShippingID", shippingID);

            SqlParameter outputParam = new SqlParameter("@OutputParam", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (int)outputParam.Value;
        }
    }

    public async Task<List<ShippingResponse>> GetAllAsync()
    {
        List<ShippingResponse> shippings = new List<ShippingResponse>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            SqlCommand cmd = new SqlCommand("GetAllShippings", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    ShippingResponse shipping = new ShippingResponse
                    {
                        ShippingID = reader.GetInt32(reader.GetOrdinal("ShippingID")),
                        OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                        CarrierName = reader.GetString(reader.GetOrdinal("CarrierName")),
                        TrackingNumber = reader.GetString(reader.GetOrdinal("TrackingNumber")),
                        ShippingStatus = reader.GetString(reader.GetOrdinal("Shipping_Status")),
                        EstimatedDeliveryDate = reader.GetDateTime(reader.GetOrdinal("EstimatedDeliveryDate")),
                        ActualDeliveryDate = reader.IsDBNull(reader.GetOrdinal("ActualDeliveryDate"))
                            ? (DateTime?)null
                            : reader.GetDateTime(reader.GetOrdinal("ActualDeliveryDate"))
                    };
                    shippings.Add(shipping);
                }
            }
        }

        return shippings;
    }

    public async Task<bool> IsExistAsync(int shippingID)
    {
        int shippingExists =   1;

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            SqlCommand cmd = new SqlCommand("CheckShippingIDExists", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@ShippingID", shippingID);
            shippingExists = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        return shippingExists > 0;
    }

    public async Task<ShippingOwnerResponse?> GetShippingOwnerDetailsAsync(int ownerId)
    {
        ShippingOwnerResponse? shippingOwnerDetails = null;

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            SqlCommand cmd = new SqlCommand("ShippingOwner", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new SqlParameter("@OwnerID", SqlDbType.Int)).Value = ownerId;

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    shippingOwnerDetails = new ShippingOwnerResponse
                    {
                        UserName = reader["user_name"].ToString() !,
                        UserEmail = reader["user_email"].ToString()!,
                        UserPhone = reader["user_phone"].ToString()!
                    };
                }
            }
        }

        return shippingOwnerDetails;
    }

    public async Task<bool> UpdateStatus(int Status, int ShippingId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand command = new SqlCommand("UpdateShippingStatus", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Status", Status);
                command.Parameters.AddWithValue("@ShippingId", ShippingId);
                var result = Convert.ToInt32(await command.ExecuteScalarAsync());
                return result > 0 ? true : false;
            }
        }
    }

   

    public async Task<bool> UpdateActualDeliveryDate(DateTime ActualDeliveryDate, int ShippingId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand command = new SqlCommand("UpdateActualDeliveryDate", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DeliveryDate", ActualDeliveryDate);
                command.Parameters.AddWithValue("@ShippingId", ShippingId);
                var result = Convert.ToInt32( await command.ExecuteScalarAsync());
                return result > 0 ? true : false;   
            }
        }
    }

}