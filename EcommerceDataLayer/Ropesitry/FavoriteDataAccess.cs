using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;




public class FavoriteDataAccess
{
    private  readonly string _connectionString;
    public FavoriteDataAccess(ConnectionString connectionString)
    {
        _connectionString = connectionString.connectionString;
    }


    public  void Delete(int productID, int userID)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_DeleteFromFavorite", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productID", productID);
            cmd.Parameters.AddWithValue("@userID", userID);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public  void Add(int productID, int userID, bool isFavorite = true)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_AddToFavorite", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productID", productID);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@isFavorite", isFavorite);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public List<int> GetByUserId(int userId)
    {
        List<int> favoriteProductIds = new List<int>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand("GetFavoritesByUserId", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", userId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        favoriteProductIds.Add(reader.GetInt32(0)); // Assuming productID is the first column
                    }
                }
            }
        }
        return favoriteProductIds;
    }


}