using EcommerceDataLayer.Entities.Reviews;
using EcommerceDataLayer.IRopesitry;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;


public class ReviewsRopesitry : IReviewsRopesitry
{
    private readonly string _connectionString;

    public ReviewsRopesitry(ConnectionString connectionString)
    {
        _connectionString = connectionString.connectionString;
    }

    public async Task<List<ReviewResponse>> GetAllAsync()
    {
        List<ReviewResponse> reviews = new List<ReviewResponse>();

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
                        ReviewResponse review = new ReviewResponse
                        {
                            ReviewId = Convert.ToInt32(reader["ReviewID"]),
                            ReviewText = reader["ReviewText"].ToString()!,
                            ProductId = Convert.ToInt32(reader["ProductID"]),
                            UserId = reader["UserId"].ToString()!,
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

    public async Task<bool> AddAsync(ReviewRequest review)
    {
        var result = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand("dbo.CreateReview", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", review.ProductId);
                command.Parameters.AddWithValue("@UserID", review.UserId);
                command.Parameters.AddWithValue("@ReviewText", review.ReviewText);
                command.Parameters.AddWithValue("@Rating", review.Rating);

                await connection.OpenAsync();
                result = await command.ExecuteNonQueryAsync();
            }
        }
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int reviewID)
    {
        var result = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand("DeleteReview", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ReviewID", reviewID);

                await connection.OpenAsync();
                result = await command.ExecuteNonQueryAsync();
            }
        }
        return result > 0;
    }

    public async Task<bool> UpdateAsync(UpdateReviewRequest review, int ReviewId)
    {
        var result = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand("UpdateReview", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ReviewID", ReviewId);
                command.Parameters.AddWithValue("@ReviewText", review.ReviewText);
                command.Parameters.AddWithValue("@Rating", review.Rating);

                await connection.OpenAsync();
                result = await command.ExecuteNonQueryAsync();
            }
        }
        return result > 0;
    }

    public async Task<List<ReviewResponse>> GetAllProductReviewsAsync(int ProductId)
    {
        List<ReviewResponse> reviews = new List<ReviewResponse>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand("GetAllReviewsInProduct", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductId", ProductId);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ReviewResponse review = new ReviewResponse
                        {
                            ReviewId = Convert.ToInt32(reader["ReviewID"]),
                            ReviewText = reader["ReviewText"].ToString()!,
                            ProductId = Convert.ToInt32(reader["ProductID"]),
                            UserId = reader["UserId"].ToString()!,
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

    public async Task<List<ReviewResponse>> GetAllUserReviewsAsync(string UserId)
    {
        List<ReviewResponse> reviews = new List<ReviewResponse>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand("GetAllUserReviews", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", UserId);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ReviewResponse review = new ReviewResponse
                        {
                            ReviewId = Convert.ToInt32(reader["ReviewID"]),
                            ReviewText = reader["ReviewText"].ToString()!,
                            ProductId = Convert.ToInt32(reader["ProductID"]),
                            UserId = reader["UserId"].ToString()!,
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

    public async Task<List<ReviewResponse>> GetAllUserInProductReviewsAsync(string UserId, int ProductId)
    {
        List<ReviewResponse> reviews = new List<ReviewResponse>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand("GetAllUserReviewsInProduct", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@ProductId", ProductId);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ReviewResponse review = new ReviewResponse
                        {
                            ReviewId = Convert.ToInt32(reader["ReviewID"]),
                            ReviewText = reader["ReviewText"].ToString()!,
                            ProductId = Convert.ToInt32(reader["ProductID"]),
                            UserId = reader["UserId"].ToString()!,
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

    public async Task<bool> IsExistAsync(int ReviewId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand("ReviewIsEXISTS", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ReviewId", ReviewId);

            int rowsAffected = Convert.ToInt32(await command.ExecuteScalarAsync());

            return rowsAffected > 0;
        }
    }
}