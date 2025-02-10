using EcommerceDataLayer.Entities.Address;
using EcommerceDataLayer.IRopesitry;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;


public class AddressRopesitry: IAddressRopesitry
{
    private  readonly string _connectionString;
    public AddressRopesitry(ConnectionString connectionString)
    {
        _connectionString = connectionString.connectionString;
    }


    public async Task<List<AddressResponse>> GetAllAsync()
    {
        List<AddressResponse> addresses = new List<AddressResponse>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            SqlCommand cmd = new SqlCommand("sp_GetAddress", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    AddressResponse address = new AddressResponse
                    {
                        UserId = reader["UserId"].ToString()!,
                        AddressId = Convert.ToInt32(reader["AddressID"]),
                        AddressLine = reader["AddressLine"].ToString()!,
                        City = reader["City"].ToString()!,
                        Country = reader["Country"].ToString()!,
                        Longitude = Convert.ToDecimal(reader["Longitude"]),
                        Latitude = Convert.ToDecimal(reader["Latitude"]),
                    };

                    addresses.Add(address);
                }
            }
        }

        return addresses;

    }

    public async Task<bool> AddAsync(AddressRequest address)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_InsertAddress", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AddressLine", address.AddressLine);
            cmd.Parameters.AddWithValue("@City", address.City);
            cmd.Parameters.AddWithValue("@Country", address.Country);
            cmd.Parameters.AddWithValue("@Longitude", address.Longitude);
            cmd.Parameters.AddWithValue("@Latitude", address.Latitude);
            cmd.Parameters.AddWithValue("@UserId", address.UserId);

            await conn.OpenAsync();
            var AddressId = Convert.ToInt32(await cmd.ExecuteScalarAsync()); 
           
            return AddressId > 0;
        }


    }

    public async Task<AddressResponse?> GetByIdAsync(int addressId)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            SqlCommand cmd = new SqlCommand("sp_GetAddress", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AddressID", addressId);

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (!await reader.ReadAsync())
                {
                    return null;
                }

                AddressResponse address = new AddressResponse
                {
                    UserId = reader["UserID"].ToString()!,
                    AddressId = Convert.ToInt32(reader["AddressID"]),
                    AddressLine = reader["AddressLine"].ToString()!,
                    City = reader["City"].ToString()!,
                    Country = reader["Country"].ToString()!,
                    Longitude = Convert.ToDecimal(reader["Longitude"]),
                    Latitude = Convert.ToDecimal(reader["Latitude"]),
                };

                return address;
            }
        }
    }


    public async Task<bool> UpdateAsync(AddressRequest address, int addressId)
    {
        var rows = 0;
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_UpdateAddress", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@AddressID",addressId);
            cmd.Parameters.AddWithValue("@AddressLine", address.AddressLine);
            cmd.Parameters.AddWithValue("@City", address.City);
            cmd.Parameters.AddWithValue("@Country", address.Country);
            cmd.Parameters.AddWithValue("@Longitude", address.Longitude);
            cmd.Parameters.AddWithValue("@Latitude", address.Latitude);
             await conn.OpenAsync();
            rows = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }
        return rows > 0;
    }


    public async Task<bool> DeleteAsync(int addressId)
    {
        var row = 0;
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_DeleteAddress", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AddressID", addressId);
            await conn.OpenAsync();
            row  = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }
        return row > 0;
    }


    public async Task<List<AddressResponse>> GetAllAddressesByUserIDAsync(string UserId)
    {
        List<AddressResponse> addresses = new List<AddressResponse>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            SqlCommand cmd = new SqlCommand("GetUserAddress", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", UserId);

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    AddressResponse address = new AddressResponse
                    {
                        UserId = reader["UserId"].ToString()!,
                        AddressId = Convert.ToInt32(reader["AddressID"]),
                        AddressLine = reader["AddressLine"].ToString()!,
                        City = reader["City"].ToString()!,
                        Country = reader["Country"].ToString()!,
                        Longitude = Convert.ToDecimal(reader["Longitude"]),
                        Latitude = Convert.ToDecimal(reader["Latitude"]),
                    };

                    addresses.Add(address);
                }
            }
        }

        return addresses;
    }

    public async Task<bool> IsExistAsync(int AddressId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand("AddressIsEXISTS", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@AddressId", AddressId);

            int rowsAffected = Convert.ToInt32(await command.ExecuteScalarAsync());

            return rowsAffected > 0;
        }
    }


}