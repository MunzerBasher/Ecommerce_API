using EcommerceDataLayer.Entities.Address;
using EcommerceDataLayer.IRopesitry;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;


public class AddressDataAccess: IAddressRopesitry
{
    private  readonly string _connectionString;
    public AddressDataAccess(ConnectionString connectionString)
    {
        _connectionString = connectionString.connectionString;
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
            await conn.OpenAsync();
            var AddressId = Convert.ToInt32(await cmd.ExecuteScalarAsync()); 
            var result = 0;
            if (AddressId > 0)
            {
                SqlCommand cmd2 = new SqlCommand("sp_InsertUserAddress", conn);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@UserId", address.UserID);
                cmd2.Parameters.AddWithValue("@AddressId", AddressId);
                result = Convert.ToInt32(await cmd2.ExecuteScalarAsync()); ;
            }
            return Convert.ToInt32(result) > 0;
        }


    }

    public async Task<AddressDTO?> GetByIdAsync(int addressID)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            SqlCommand cmd = new SqlCommand("sp_GetAddress", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AddressID", addressID);

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (!await reader.ReadAsync())
                {
                    return null;
                }

                AddressDTO address = new AddressDTO
                {
                    UserID = Convert.ToInt32(reader["UserID"]),
                    AddressID = Convert.ToInt32(reader["AddressID"]),
                    AddressLine = reader["AddressLine"].ToString(),
                    City = reader["City"].ToString(),
                    Country = reader["Country"].ToString(),
                    Longitude = Convert.ToDecimal(reader["Longitude"]),
                    Latitude = Convert.ToDecimal(reader["Latitude"]),
                };

                return address;
            }
        }
    }


    public async Task<bool> UpdateAsync(AddressDTO address)
    {
        var rows = 0;
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_UpdateAddress", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@AddressID", address.AddressID);
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


    public async Task<bool> DeleteAsync(int addressID)
    {
        var row = 0;
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_DeleteAddress", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AddressID", addressID);
            await conn.OpenAsync();
            row  = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }
        return row > 0;
    }

    public async Task<List<AddressDTO>> GetAllAddressesByUserIDAsync(int userID)
    {
        List<AddressDTO> addresses = new List<AddressDTO>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            SqlCommand cmd = new SqlCommand("GetUserAddress", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", userID);

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    AddressDTO address = new AddressDTO
                    {
                        //UserID = Convert.ToInt32(reader["UserID"]),
                        AddressID = Convert.ToInt32(reader["AddressID"]),
                        AddressLine = reader["AddressLine"].ToString(),
                        City = reader["City"].ToString(),
                        Country = reader["Country"].ToString(),
                        Longitude = Convert.ToDecimal(reader["Longitude"]),
                        Latitude = Convert.ToDecimal(reader["Latitude"]),
                    };

                    addresses.Add(address);
                }
            }
        }

        return addresses;
    }



}