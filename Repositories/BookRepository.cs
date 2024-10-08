using library_sesterm.Models;
using Microsoft.Data.Sqlite; // Use SQLite
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace library_sesterm.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;

        public BookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            var books = new List<Book>();
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqliteCommand("SELECT * FROM Books", conn);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            books.Add(new Book
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Author = reader.GetString(2),
                                Category = reader.GetString(3),
                                Count = reader.GetInt32(4),
                                Url = reader.IsDBNull(5) ? null : reader.GetString(5)
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
            return books;
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            Book book = null;
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqliteCommand("SELECT * FROM Books WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            book = new Book
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Author = reader.GetString(2),
                                Category = reader.GetString(3),
                                Count = reader.GetInt32(4),
                                Url = reader.IsDBNull(5) ? null : reader.GetString(5)
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
            return book;
        }

        public async Task AddBookAsync(Book book)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    var cmd = new SqliteCommand("INSERT INTO Books (Title, Author, Category, Count, Url) VALUES (@Title, @Author, @Category, @Count, @Url)", conn);
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Author", book.Author);
                    cmd.Parameters.AddWithValue("@Category", book.Category);
                    cmd.Parameters.AddWithValue("@Count", book.Count);
                    cmd.Parameters.AddWithValue("@Url", (object)book.Url ?? DBNull.Value);
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

        public async Task UpdateBookAsync(Book book)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    var cmd = new SqliteCommand("UPDATE Books SET Title = @Title, Author = @Author, Category = @Category, Count = @Count, Url = @Url WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", book.Id);
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Author", book.Author);
                    cmd.Parameters.AddWithValue("@Category", book.Category);
                    cmd.Parameters.AddWithValue("@Count", book.Count);
                    cmd.Parameters.AddWithValue("@Url", (object)book.Url ?? DBNull.Value);
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

        public async Task DeleteBookAsync(int id)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    var cmd = new SqliteCommand("DELETE FROM Books WHERE Id = @Id", conn);
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

        public async Task IncreaseBookCountAsync(int bookId)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    var cmd = new SqliteCommand("UPDATE Books SET Count = Count + 1 WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", bookId);
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
