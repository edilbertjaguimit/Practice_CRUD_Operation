using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Practice_CRUD_Operation.Models
{
    public class UserManagement : IUserManagement
    {
        private string _connString;
        public UserManagement(string connString)
        {
            _connString = connString;
        }

        public async Task<List<User>> ReadAsync()
        {
            var user = new List<User>();
            using (var db = new SqlConnection(_connString))
            {
                await db.OpenAsync();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM [USER]";
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            user.Add(new User
                            {
                                Id = Convert.ToInt32(reader["USER_ID"]),
                                Firstname = reader["USER_FIRSTNAME"].ToString(),
                                Lastname = reader["USER_LASTNAME"].ToString(),
                                Email = reader["USER_EMAIL"].ToString(),
                                Password = reader["USER_PASSWORD"].ToString()
                            });
                        }
                    }
                }
            }
            return user;
        }
        public async Task<bool> InsertAsync(User user)
        {
            var hashPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, 13);

            using (var db = new SqlConnection(_connString))
            {
                await db.OpenAsync();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO [USER] VALUES (@firstname, @lastname, @email, @password)";
                    cmd.Parameters.AddWithValue("firstname", user.Firstname);
                    cmd.Parameters.AddWithValue("lastname", user.Lastname);
                    cmd.Parameters.AddWithValue("email", user.Email);
                    cmd.Parameters.AddWithValue("password", hashPassword);
                    int ctr = await cmd.ExecuteNonQueryAsync();
                    return ctr > 0;
                }
            }
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            var users = await ReadAsync();
            var userEmail = users.FirstOrDefault(u => u.Email == email);
            return userEmail != null;
        }

        public async Task<bool> CheckPasswordAsync(string email, string password)
        {
            var users = await ReadAsync();
            var user = users.FirstOrDefault(u => u.Email == email);
            return BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password);
        }
    }
}