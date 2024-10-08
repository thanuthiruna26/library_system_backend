using library_sesterm.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace library_sesterm.Repositories
{
    public class ReturnedBookRepository : IReturnedBookRepository
    {
        private readonly string _connectionString;

        public ReturnedBookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task AddReturnedBookAsync(ReturnedBook returnedBook)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    var cmd = new SqliteCommand("INSERT INTO ReturnedBooks (BookId, Nic, ReturnDate, Status) VALUES (@BookId, @Nic, @ReturnDate, @Status)", conn);
                    cmd.Parameters.AddWithValue("@BookId", returnedBook.BookId);
                    cmd.Parameters.AddWithValue("@Nic", returnedBook.Nic);
                    cmd.Parameters.AddWithValue("@ReturnDate", returnedBook.ReturnDate);
                    cmd.Parameters.AddWithValue("@Status", returnedBook.Status);
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

        public async Task<IEnumerable<ReturnedBook>> GetAllReturnedBooksAsync()
        {
            var returnedBooks = new List<ReturnedBook>();
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqliteCommand("SELECT * FROM ReturnedBooks", conn);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            returnedBooks.Add(new ReturnedBook
                            {
                                Id = reader.GetInt32(0),
                                BookId = reader.GetInt32(1),
                                Nic = reader.GetInt32(2),
                                ReturnDate = reader.GetDateTime(3),
                                Status = reader.GetString(4)
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
            return returnedBooks;
        }
    }
}
