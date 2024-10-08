using library_sesterm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace library_sesterm.Repositories
{
    public interface IReturnedBookRepository
    {
        Task AddReturnedBookAsync(ReturnedBook returnedBook);
        Task<IEnumerable<ReturnedBook>> GetAllReturnedBooksAsync();
    }
}
