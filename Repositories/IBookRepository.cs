using library_sesterm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace library_sesterm.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task IncreaseBookCountAsync(int bookId);
    }
}
