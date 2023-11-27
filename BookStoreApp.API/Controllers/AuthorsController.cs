using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Authors;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController(BookStoreDbContext _context, IMapper _mapper, ILogger<AuthorsController> _logger) : ControllerBase
    {
        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            try
            {
                _logger.LogInformation($"Request to {nameof(GetAuthors)}");
                var authors = await _context.Authors.ToListAsync();
                var authorDtos = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);
                return Ok(authorDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
        {
            try
            {
                _logger.LogInformation($"Request to {nameof(GetAuthor)}");
                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {
                    _logger.LogWarning($"{GetAuthor}: Author with id {id} not found.");
                    return NotFound();
                }

                var authorDto = _mapper.Map<AuthorReadOnlyDto>(author);
                return authorDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthor)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            try
            {
                _logger.LogInformation($"Request to {nameof(PutAuthor)}");
                if (id != authorDto.Id)
                {
                    _logger.LogWarning($"{PutAuthor}: id and dto id mismatch");
                    return BadRequest();
                }

                var author = await _context.Authors.FindAsync(id);
                if (author is null)
                {
                    _logger.LogWarning($"{PutAuthor}: no author found with id {id}");
                    return NotFound();
                }

                _mapper.Map(authorDto, author);
                _context.Entry(author).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(id))
                    {
                        _logger.LogWarning($"{PutAuthor}: Author with {id} not found.");
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
                _logger.LogError(ex, $"Error Performing PUT in {nameof(PutAuthor)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
            try
            {
                _logger.LogInformation($"Request to {nameof(PostAuthor)}");
                var author = _mapper.Map<Author>(authorDto);
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing POST in {nameof(PostAuthor)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                _logger.LogInformation($"Request to {nameof(DeleteAuthor)}");
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing DELETE in {nameof(PostAuthor)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
