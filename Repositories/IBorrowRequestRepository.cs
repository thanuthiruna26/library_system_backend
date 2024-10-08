using library_sesterm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace library_sesterm.Repositories
{
    public interface IBorrowRequestRepository
    {
        Task<IEnumerable<BorrowRequest>> GetBorrowRequestsByNicAsync(int nic);
        Task AddBorrowRequestAsync(BorrowRequest borrowRequest);
        Task<BorrowRequest> GetBorrowRequestByIdAsync(int id);
        Task UpdateBorrowRequestStatusAsync(int id, string status);
        Task<int> GetBorrowedBookCountByNicAsync(int nic);
        Task<bool> IsBookAlreadyBorrowedAsync(int nic, int bookId);
        Task<BorrowRequest> GetBorrowRequestByBookIdAndNicAsync(int bookId, int nic);
        Task ReturnBookAsync(int id);
    }
}
