using library_sesterm.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace library_sesterm.Repositories
{
    public class BorrowRequestRepository : IBorrowRequestRepository
    {
        private readonly string _connectionString;

        public BorrowRequestRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<BorrowRequest>> GetBorrowRequestsByNicAsync(int nic)
        {
            var borrowRequests = new List<BorrowRequest>();
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqliteCommand("SELECT * FROM BorrowRequests WHERE Nic = @Nic", conn);
                    cmd.Parameters.AddWithValue("@Nic", nic);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            borrowRequests.Add(new BorrowRequest
                            {
                                Id = reader.GetInt32(0),
                                BookId = reader.GetInt32(1),
                                Nic = reader.GetInt32(2),
                                BorrowDate = reader.GetDateTime(3),
                                ReturnDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                Status = reader.GetString(5)
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
            return borrowRequests;
        }

        public async Task AddBorrowRequestAsync(BorrowRequest borrowRequest)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    var cmd = new SqliteCommand("INSERT INTO BorrowRequests (BookId, Nic, BorrowDate, Status) VALUES (@BookId, @Nic, @BorrowDate, @Status)", conn);
                    cmd.Parameters.AddWithValue("@BookId", borrowRequest.BookId);
                    cmd.Parameters.AddWithValue("@Nic", borrowRequest.Nic);
                    cmd.Parameters.AddWithValue("@BorrowDate", borrowRequest.BorrowDate);
                    cmd.Parameters.AddWithValue("@Status", borrowRequest.Status);
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

        public async Task<BorrowRequest> GetBorrowRequestByIdAsync(int id)
        {
            BorrowRequest borrowRequest = null;
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqliteCommand("SELECT * FROM BorrowRequests WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            borrowRequest = new BorrowRequest
                            {
                                Id = reader.GetInt32(0),
                                BookId = reader.GetInt32(1),
                                Nic = reader.GetInt32(2),
                                BorrowDate = reader.GetDateTime(3),
                                ReturnDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                Status = reader.GetString(5)
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
            return borrowRequest;
        }

        public async Task UpdateBorrowRequestStatusAsync(int id, string status)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    var cmd = new SqliteCommand("UPDATE BorrowRequests SET Status = @Status WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Status", status);
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

        public async Task<int> GetBorrowedBookCountByNicAsync(int nic)
        {
            int count = 0;
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqliteCommand("SELECT COUNT(*) FROM BorrowRequests WHERE Nic = @Nic AND Status = 'Borrowed'", conn);
                    cmd.Parameters.AddWithValue("@Nic", nic);
                    count = (int)(await cmd.ExecuteScalarAsync() ?? 0);
                }
            }
            catch (Exception ex)
            {
                // Log exception here
                throw; // Rethrow the exception after logging
            }
            return count;
        }

        public async Task<bool> IsBookAlreadyBorrowedAsync(int nic, int bookId)
        {
            bool isBorrowed = false;
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqliteCommand("SELECT COUNT(*) FROM BorrowRequests WHERE Nic = @Nic AND BookId = @BookId AND Status = 'Borrowed'", conn);
                    cmd.Parameters.AddWithValue("@Nic", nic);
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    isBorrowed = (int)(await cmd.ExecuteScalarAsync() ?? 0) > 0;
                }
            }
            catch (Exception ex)
            {
                // Log exception here
                throw; // Rethrow the exception after logging
            }
            return isBorrowed;
        }

        public async Task<BorrowRequest> GetBorrowRequestByBookIdAndNicAsync(int bookId, int nic)
        {
            BorrowRequest borrowRequest = null;
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqliteCommand("SELECT * FROM BorrowRequests WHERE BookId = @BookId AND Nic = @Nic", conn);
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    cmd.Parameters.AddWithValue("@Nic", nic);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            borrowRequest = new BorrowRequest
                            {
                                Id = reader.GetInt32(0),
                                BookId = reader.GetInt32(1),
                                Nic = reader.GetInt32(2),
                                BorrowDate = reader.GetDateTime(3),
                                ReturnDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                Status = reader.GetString(5)
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
            return borrowRequest;
        }

        public async Task ReturnBookAsync(int id)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    var cmd = new SqliteCommand("UPDATE BorrowRequests SET ReturnDate = @ReturnDate, Status = 'Returned' WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@ReturnDate", DateTime.UtcNow);
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
