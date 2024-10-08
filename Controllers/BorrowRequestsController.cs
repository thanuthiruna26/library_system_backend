using library_sesterm.DTOs;
using library_sesterm.Models;
using library_sesterm.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace library_sesterm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowRequestsController : ControllerBase
    {
        private readonly IBorrowRequestRepository _borrowRequestRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IReturnedBookRepository _returnedBookRepository;

        public BorrowRequestsController(IBorrowRequestRepository borrowRequestRepository, IBookRepository bookRepository, IReturnedBookRepository returnedBookRepository)
        {
            _borrowRequestRepository = borrowRequestRepository;
            _bookRepository = bookRepository;
            _returnedBookRepository = returnedBookRepository;
        }

        // POST: api/borrowrequests
        [HttpPost]
        public async Task<IActionResult> SendBorrowRequest([FromBody] BorrowRequestDto borrowRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var borrowRequest = new BorrowRequest
                {
                    BookId = borrowRequestDto.BookId,
                    Nic = borrowRequestDto.Nic,
                    BorrowDate = DateTime.UtcNow,
                    Status = "Pending"
                };

                // Check borrowing constraints
                int borrowedBookCount = await _borrowRequestRepository.GetBorrowedBookCountByNicAsync(borrowRequest.Nic);
                if (borrowedBookCount >= 2)
                {
                    return BadRequest("You have already borrowed the maximum number of books.");
                }

                bool isBookAlreadyBorrowed = await _borrowRequestRepository.IsBookAlreadyBorrowedAsync(borrowRequest.Nic, borrowRequest.BookId);
                if (isBookAlreadyBorrowed)
                {
                    return BadRequest("You have already borrowed this book.");
                }

                var book = await _bookRepository.GetBookByIdAsync(borrowRequest.BookId);
                if (book == null || book.Count <= 0)
                {
                    return BadRequest("This book is not available.");
                }

                await _borrowRequestRepository.AddBorrowRequestAsync(borrowRequest);
                book.Count -= 1;
                await _bookRepository.UpdateBookAsync(book);

                return Ok("Borrow request submitted successfully.");
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/borrowrequests/admin/return
        [HttpPut("admin/return")]
        public async Task<IActionResult> AdminReturnBook([FromBody] ReturnBookRequestDto returnRequest)
        {
            if (returnRequest == null)
            {
                return BadRequest("Invalid return request.");
            }

            try
            {
                var borrowRequest = await _borrowRequestRepository.GetBorrowRequestByBookIdAndNicAsync(returnRequest.BookId, returnRequest.Nic);
                if (borrowRequest == null)
                {
                    return NotFound("Borrow request not found for this book and NIC.");
                }

                if (borrowRequest.Status == "Returned")
                {
                    return BadRequest("This book has already been returned.");
                }

                await _borrowRequestRepository.ReturnBookAsync(borrowRequest.Id);
                await _bookRepository.IncreaseBookCountAsync(borrowRequest.BookId);

                var returnedBook = new ReturnedBook
                {
                    BookId = borrowRequest.BookId,
                    Nic = returnRequest.Nic,
                    ReturnDate = DateTime.UtcNow,
                    Status = "Returned"
                };

                await _returnedBookRepository.AddReturnedBookAsync(returnedBook);

                return NoContent(); // Return 204 No Content
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
