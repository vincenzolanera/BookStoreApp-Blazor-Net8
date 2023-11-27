using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Books;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(BookStoreDbContext _context, IMapper _mapper, ILogger<BooksController> _logger) : ControllerBase
    {
        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
            try
            {
                _logger.LogInformation($"Request to {nameof(GetBooks)}");
                var bookDtos = await _context.Books
                    .Include(x => x.Author)
                    .ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return Ok(bookDtos);
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"Error Performing GET in {nameof(GetBooks)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailDto>> GetBook(int id)
        {
            try
            {
                _logger.LogInformation($"Request to {nameof(GetBook)}");
                var book = await _context.Books
                    .Include(x => x.Author)
                    .ProjectTo<BookDetailDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (book == null)
                {
                    _logger.LogWarning($"{nameof(GetBook)}: Book with id {id} not found");
                    return NotFound();
                }

                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing GET in {nameof(GetBook)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            try
            {
                _logger.LogInformation($"Request to {nameof(PutBook)}");
                if (id != bookDto.Id)
                {
                    _logger.LogWarning($"{PutBook}: id and dto id mismatch");
                    return BadRequest();
                }

                var book = await _context.Books.FindAsync(id);
                if (book is null)
                {
                    _logger.LogWarning($"{PutBook}: no books found with id {id}");
                    return NotFound();
                }

                _mapper.Map(bookDto, book);
                _context.Entry(book).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(id))
                    {
                        _logger.LogWarning($"{PutBook}: Book with {id} not found.");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing PUT in {nameof(PutBook)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(BookCreateDto bookDto)
        {
            try
            {
                _logger.LogInformation($"Request to {nameof(PutBook)}");
                var book = _mapper.Map<Book>(bookDto);
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing POST in {nameof(PostBook)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                _logger.LogInformation($"Request to {nameof(PutBook)}");
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    _logger.LogWarning($"{nameof(DeleteBook)}: No book found for id {id}");
                    return NotFound();
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing DELETE in {nameof(DeleteBook)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
