using library_sesterm.DTOs;
using library_sesterm.Models;
using library_sesterm.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace library_sesterm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponseDto>>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooksAsync();
            var bookResponseDtos = new List<BookResponseDto>();
            foreach (var book in books)
            {
                bookResponseDtos.Add(new BookResponseDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Category = book.Category,
                    Count = book.Count,
                    Url = book.Url
                });
            }
            return Ok(bookResponseDtos);
        }

        // POST: api/books
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookRequestDto bookRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = new Book
            {
                Title = bookRequest.Title,
                Author = bookRequest.Author,
                Category = bookRequest.Category,
                Count = bookRequest.Count,
                Url = bookRequest.Url
            };

            await _bookRepository.AddBookAsync(book);
            return CreatedAtAction(nameof(GetAllBooks), new { id = book.Id }, book);
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookRequestDto bookRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBook = await _bookRepository.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Title = bookRequest.Title;
            existingBook.Author = bookRequest.Author;
            existingBook.Category = bookRequest.Category;
            existingBook.Count = bookRequest.Count;
            existingBook.Url = bookRequest.Url;

            await _bookRepository.UpdateBookAsync(existingBook);
            return NoContent();
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookRepository.DeleteBookAsync(id);
            return NoContent();
        }
    }
}
