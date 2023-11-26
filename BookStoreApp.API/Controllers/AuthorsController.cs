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
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            try
            {
                logger.LogInformation($"Request to {nameof(GetAuthors)}");
                var authors = await context.Authors.ToListAsync();
                var authorDtos = mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);
                return Ok(authorDtos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
        {
            try
            {
                logger.LogInformation($"Request to {nameof(GetAuthor)}");
                var author = await context.Authors.FindAsync(id);

                if (author == null)
                {
                    logger.LogWarning($"{GetAuthor}: Author with id {id} not found.");
                    return NotFound();
                }

                var authorDto = mapper.Map<AuthorReadOnlyDto>(author);
                return authorDto;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthor)}");
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
                logger.LogInformation($"Request to {nameof(PutAuthor)}");
                if (id != authorDto.Id)
                {
                    logger.LogWarning($"{PutAuthor}: id and dto id mismatch");
                    return BadRequest();
                }

                var author = await context.Authors.FindAsync(id);
                if (author is null)
                {
                    logger.LogWarning($"{PutAuthor}: no author found with id {id}");
                    return NotFound();
                }

                mapper.Map(authorDto, author);
                context.Entry(author).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(id))
                    {
                        logger.LogWarning($"{PutAuthor}: Author with {id} not found.");
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
                logger.LogError(ex, $"Error Performing PUT in {nameof(PutAuthor)}");
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
                logger.LogInformation($"Request to {nameof(PostAuthor)}");
                var author = mapper.Map<Author>(authorDto);
                context.Authors.Add(author);
                await context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Performing POST in {nameof(PostAuthor)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                logger.LogInformation($"Request to {nameof(DeleteAuthor)}");
                var author = await context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                context.Authors.Remove(author);
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Performing DELETE in {nameof(PostAuthor)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        private bool AuthorExists(int id)
        {
            return context.Authors.Any(e => e.Id == id);
        }
    }
}
