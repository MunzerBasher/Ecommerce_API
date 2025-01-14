using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;


public class AddressDataAccess
{
    private  readonly string _connectionString;
    public AddressDataAccess(ConnectionString connectionString)
    {
        _connectionString = connectionString.connectionString;
    }

    public bool AddAddress(AddressDTO address)
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
            conn.Open();
            var AddressId = Convert.ToInt32(cmd.ExecuteScalar()); 
            var result = 0;
            if (AddressId > 0)
            {
                SqlCommand cmd2 = new SqlCommand("sp_InsertUserAddress", conn);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@UserId", address.UserID);
                cmd2.Parameters.AddWithValue("@AddressId", AddressId);
                result = Convert.ToInt32(cmd2.ExecuteScalar());
            }
            return Convert.ToInt32(result) > 0;
        }


    }

    public  AddressDTO? GetAddressByID(int addressID)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_GetAddress", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AddressID", addressID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                return null;
            }
            DataRow row = dt.Rows[0];
            AddressDTO address = new AddressDTO
            {
                AddressID = Convert.ToInt32(row["AddressID"]),
                AddressLine = row["AddressLine"].ToString(),
                City = row["City"].ToString(),
                Country = row["Country"].ToString(),
                Longitude = Convert.ToDecimal(row["Longitude"]),
                Latitude = Convert.ToDecimal(row["Latitude"]),
            };

            return address;
        }
    }

    public  void UpdateAddress(AddressDTO address)
    {
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
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }


    public  void DeleteAddress(int addressID)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_DeleteAddress", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AddressID", addressID);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public  List<AddressDTO> GetAllAddressesByUserID(int userID)
    {
        List<AddressDTO> addresses = new List<AddressDTO>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("GetUserAddress", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", userID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                AddressDTO address = new AddressDTO
                {
                    AddressID = Convert.ToInt32(row["AddressID"]),
                    AddressLine = row["AddressLine"].ToString(),
                    City = row["City"].ToString(),
                    Country = row["Country"].ToString(),
                    Longitude = Convert.ToDecimal(row["Longitude"]),
                    Latitude = Convert.ToDecimal(row["Latitude"]),
                };

                addresses.Add(address);
            }
        }

        return addresses;
    }



}