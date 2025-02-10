using System.Data;
using Microsoft.Data.SqlClient;
using EcommerceDataLayer.DTOS;
using EcommerceDataLayer.Shared;
using EcommerceDataLayer.IRopesitry;


namespace EcommerceDataLayer.Ropesitry
{

    public class UserDataAccess : IApplicationUser
    {
        private readonly string _connectionString;
        public UserDataAccess(ConnectionString connectionString)
        {
            _connectionString = connectionString.connectionString;
        }
        public int Add(UserDTO user)
        {
            // Declare the output parameter for new user id
            int newUserId = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Prepare the stored procedure call
                using (SqlCommand cmd = new SqlCommand("dbo.CreateUserAndReturnId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add input parameters
                    cmd.Parameters.Add(new SqlParameter("@user_name", SqlDbType.NVarChar, 255)).Value = user.UserName;
                    cmd.Parameters.Add(new SqlParameter("@user_email", SqlDbType.NVarChar, 255)).Value = user.UserEmail;
                    cmd.Parameters.Add(new SqlParameter("@user_phone", SqlDbType.NVarChar, 15)).Value = user.UserPhone;
                    cmd.Parameters.Add(new SqlParameter("@user_verflycode", SqlDbType.Int)).Value = user.UserVerflyCode;
                    cmd.Parameters.Add(new SqlParameter("@user_approve", SqlDbType.Int)).Value = user.UserApprove;
                    cmd.Parameters.Add(new SqlParameter("@user_date", SqlDbType.NVarChar, 255)).Value = user.UserDate;
                    cmd.Parameters.Add(new SqlParameter("@user_permission", SqlDbType.Int)).Value = user.UserPermission;
                    cmd.Parameters.Add(new SqlParameter("@user_password", SqlDbType.NVarChar, 255)).Value = user.UserPassword;

                    // Add output parameter for new user ID
                    SqlParameter outputIdParam = new SqlParameter("@new_user_id", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputIdParam);

                    try
                    {
                        // Open connection and execute stored procedure
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        // Get the output value for new user ID
                        newUserId = Convert.ToInt32(outputIdParam.Value);
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions (e.g., logging)
                        throw new Exception("Error: " + ex.Message + "in CreateUserAndReturnId ");
                    }
                }
            }

            return newUserId;
        }

        public bool CheckVerifyCodeByEmail(string userEmail, int verifyCode)
        {
            bool isValid = false;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.CheckVerifyCodeByEmail", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters for email and verifyCode
                    cmd.Parameters.Add(new SqlParameter("@user_email", SqlDbType.NVarChar, 255)).Value = userEmail;
                    cmd.Parameters.Add(new SqlParameter("@verify_code", SqlDbType.Int)).Value = verifyCode;

                    try
                    {
                        // Open the connection and execute the function
                        conn.Open();
                        int result = (int)cmd.ExecuteScalar(); // The result will be either 1 or 0

                        // If the result is 1, the code is valid
                        isValid = result == 1;
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions (logging, etc.)
                        throw new Exception("Error: " + ex.Message + "in CheckVerifyCodeByEmail ");
                    }
                }
            }

            return isValid;
        }

        public bool Delete(string userEmail)
        {
            bool isDeleted = false;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.DeleteUserByEmail", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add the email parameter
                    cmd.Parameters.Add(new SqlParameter("@user_email", SqlDbType.NVarChar, 255)).Value = userEmail;

                    try
                    {
                        // Open the connection and execute the stored procedure
                        conn.Open();
                        int rowsAffected = (int)cmd.ExecuteScalar();  // Execute and get the number of rows affected

                        // If rowsAffected is greater than 0, the user was deleted
                        isDeleted = rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions (logging, etc.)
                        throw new Exception("Error: " + ex.Message + "in DeleteUserByEmail ");
                    }
                }
            }

            return isDeleted;
        }


        public async Task<List<GetUserDTO>> GetAll()
        {
            var users = new List<GetUserDTO>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetAllUsers", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(new GetUserDTO
                        {
                            UserName = reader["user_name"]?.ToString(),
                            UserEmail = reader["user_email"].ToString(),
                            UserPhone = reader["user_phone"].ToString(),
                            UserApprove = Convert.ToBoolean(reader["user_approve"]),
                            UserDate = Convert.ToDateTime(reader["user_date"]),
                            UserPermission = reader["user_permission"].ToString()
                        });
                    }
                }
            }
            return users;
        }


        public async Task<bool> Update(string userEmail, string userName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UpdateUserName", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@user_email", userEmail);
                command.Parameters.AddWithValue("@user_name", userName);

                connection.Open();
                var result = await command.ExecuteNonQueryAsync();
                return result > 0; // Returns true if update was successful
            }
        }


        public async Task<bool> UpdateUserPasswordAsync(string userEmail, string userPassword)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UpdateUserPassword", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@user_email", userEmail);
                command.Parameters.AddWithValue("@user_password", userPassword);

                connection.Open();
                var result = await command.ExecuteNonQueryAsync();
                return result > 0; // Returns true if update was successful
            }
        }

    }

}