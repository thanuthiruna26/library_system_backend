using library_sesterm.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace library_sesterm.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = new List<User>();
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqliteCommand("SELECT * FROM Users", conn);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(new User
                            {
                                Id = reader.GetInt32(0),
                                Nic = reader.GetInt32(1),
                                Name = reader.GetString(2),
                                Email = reader.GetString(3),
                                PhoneNumber = reader.GetString(4),
                                Address = reader.GetString(5),
                                Role = reader.GetString(6)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception here
                throw; // Rethrow the exception after logging
            }
            return users;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            User user = null;
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqliteCommand("SELECT * FROM Users WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                Id = reader.GetInt32(0),
                                Nic = reader.GetInt32(1),
                                Name = reader.GetString(2),
                                Email = reader.GetString(3),
                                PhoneNumber = reader.GetString(4),
                                Address = reader.GetString(5),
                                Role = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception here
                throw; // Rethrow the exception after logging
            }
            return user;
        }

        public async Task<User> GetUserByNicAsync(int nic)
        {
            User user = null;
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqliteCommand("SELECT * FROM Users WHERE Nic = @Nic", conn);
                    cmd.Parameters.AddWithValue("@Nic", nic);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                Id = reader.GetInt32(0),
                                Nic = reader.GetInt32(1),
                                Name = reader.GetString(2),
                                Email = reader.GetString(3),
                                PhoneNumber = reader.GetString(4),
                                Address = reader.GetString(5),
                                Role = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception here
                throw; // Rethrow the exception after logging
            }
            return user;
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    var cmd = new SqliteCommand("INSERT INTO Users (Nic, Name, Email, PhoneNumber, Address, Role) VALUES (@Nic, @Name, @Email, @PhoneNumber, @Address, @Role)", conn);
                    cmd.Parameters.AddWithValue("@Nic", user.Nic);
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Address", user.Address);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Log exception here
                throw; // Rethrow the exception after logging
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    var cmd = new SqliteCommand("UPDATE Users SET Nic = @Nic, Name = @Name, Email = @Email, PhoneNumber = @PhoneNumber, Address = @Address, Role = @Role WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@Nic", user.Nic);
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Address", user.Address);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Log exception here
                throw; // Rethrow the exception after logging
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    var cmd = new SqliteCommand("DELETE FROM Users WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Log exception here
                throw; // Rethrow the exception after logging
            }
        }
    }
}
