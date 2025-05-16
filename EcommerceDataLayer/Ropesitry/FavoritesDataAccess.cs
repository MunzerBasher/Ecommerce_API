using EcommerceDataLayer.IRopesitry;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;




public class FavoritesDataAccess : IFavoritesRopesitry
{
    private  readonly string _connectionString;
    public FavoritesDataAccess(ConnectionString connectionString)
    {
        _connectionString = connectionString.connectionString;
    }


    public async Task<bool> Delete(int productID, int userID, CancellationToken cancellationToken = default)
    {
        var rows = 0;
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_DeleteFromFavorite", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productID", productID);
            cmd.Parameters.AddWithValue("@userID", userID);
            conn.Open();
            rows = await cmd.ExecuteNonQueryAsync();
        }
        return rows > 0;
    }

    public  async Task<bool> Add(int productID, int userID, bool isFavorite = true, CancellationToken cancellationToken = default)
    {
        var rows = 0;
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_AddToFavorite", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productID", productID);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@isFavorite", isFavorite);

            conn.Open();
            rows =  await cmd.ExecuteNonQueryAsync();
        }
        return rows > 0;
    }

    public async Task<List<int>> GetByUserId(int userId, CancellationToken cancellationToken = default)
    {
        List<int> favoriteProductIds = new List<int>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand("GetFavoritesByUserId", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", userId));

                connection.Open();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
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