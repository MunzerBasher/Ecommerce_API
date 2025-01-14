using EcommerceDataLayer.DTOS;
using EcommerceDataLayer.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

public class ReviewDataAccess
{
    private readonly string _connectionString;
    public ReviewDataAccess(ConnectionString connectionString)
    {
        _connectionString = connectionString.connectionString;
    }

    public  List<Review> GetAll()
    {
        List<Review> reviews = new List<Review>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            try
            {
                // Open connection
                connection.Open();

                // Create the command for the stored procedure
                using (SqlCommand command = new SqlCommand("GetAllReviews", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Execute the command and retrieve the results
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Read each row from the result set
                        while (reader.Read())
                        {
                            // Map each row to a Review object
                            Review review = new Review
                            {
                                ReviewId = Convert.ToInt32(reader["ReviewID"]),
                                ReviewText = reader["ReviewText"].ToString(),
                                ProductName = reader["ProductName"].ToString(),
                                UserName = reader["user_name"].ToString(),
                                Rating = Convert.ToInt32(reader["Rating"]),
                                ReviewDate = Convert.ToDateTime(reader["ReviewDate"])
                            };

                            // Add the review to the list
                            reviews.Add(review);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it, rethrow it, etc.)
                throw new Exception("Error: " + ex.Message);
            }
        }

        // Return the list of reviews
        return reviews;
    }

    public  void Add(ReviewDTOForCreate review)
    {

        // Create a new SqlConnection
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            // Create a SqlCommand for the stored procedure
            using (SqlCommand command = new SqlCommand("dbo.CreateReview", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add the parameters from the ReviewDTO object
                command.Parameters.AddWithValue("@ProductID", review.ProductID);
                command.Parameters.AddWithValue("@UserID", review.UserID);
                command.Parameters.AddWithValue("@ReviewText", review.ReviewText);
                command.Parameters.AddWithValue("@Rating", review.Rating);

                try
                {
                    // Open the connection
                    connection.Open();

                    // Execute the command (no return value, since it's a procedure)
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // Handle any SQL errors here
                    throw new Exception(ex.Message);
                }
            }
        }
    }


    public void Delete(int reviewID)
    {
        // Using "using" to ensure resources are cleaned up automatically
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            try
            {
                // Open the connection
                connection.Open();

                // Create the command to execute the stored procedure
                using (SqlCommand command = new SqlCommand("DeleteReview", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add the ReviewID parameter to the command
                    command.Parameters.AddWithValue("@ReviewID", reviewID);

                    // Execute the command
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check how many rows were affected (should be 1 if successful)

                }
            }
            catch (SqlException ex)
            {
                // Log the error (or handle it as needed)
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log general errors
                throw new Exception(ex.Message);
            }
        }
    }

    public void Update(ReviewDTOForUpdate review)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            try
            {
                // Open the connection
                connection.Open();

                // Create a command to execute the stored procedure
                using (SqlCommand command = new SqlCommand("UpdateReview", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the command
                    command.Parameters.AddWithValue("@ReviewID", review.ReviewID);
                    command.Parameters.AddWithValue("@ReviewText", review.ReviewText);
                    command.Parameters.AddWithValue("@Rating", review.Rating);

                    // Execute the stored procedure
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log the error (you can implement better error handling)
                throw new Exception("An error occurred while updating the review.", ex);
            }
        }
    }


}