using EcommerceDataLayer.DTOS;
using EcommerceDataLayer.IRopesitry;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

public class ReviewDataAccess : IReviewsRopesitry
{
    private readonly string _connectionString;

    public ReviewDataAccess(ConnectionString connectionString)
    {
        _connectionString = connectionString.connectionString;
    }

    public async Task<List<Review>> GetAllAsync()
    {
        List<Review> reviews = new List<Review>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand("GetAllReviews", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Review review = new Review
                        {
                            ReviewId = Convert.ToInt32(reader["ReviewID"]),
                            ReviewText = reader["ReviewText"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            UserName = reader["user_name"].ToString(),
                            Rating = Convert.ToInt32(reader["Rating"]),
                            ReviewDate = Convert.ToDateTime(reader["ReviewDate"])
                        };

                        reviews.Add(review);
                    }
                }
            }
        }

        return reviews;
    }

    public async Task AddAsync(ReviewDTOForCreate review)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand("dbo.CreateReview", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", review.ProductID);
                command.Parameters.AddWithValue("@UserID", review.UserID);
                command.Parameters.AddWithValue("@ReviewText", review.ReviewText);
                command.Parameters.AddWithValue("@Rating", review.Rating);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task DeleteAsync(int reviewID)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand("DeleteReview", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ReviewID", reviewID);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task UpdateAsync(ReviewDTOForUpdate review)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand("UpdateReview", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ReviewID", review.ReviewID);
                command.Parameters.AddWithValue("@ReviewText", review.ReviewText);
                command.Parameters.AddWithValue("@Rating", review.Rating);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
