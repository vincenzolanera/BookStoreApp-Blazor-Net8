using BookStoreApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsOdataController(BookStoreDbContext _context) : ODataController
    {

        [EnableQuery, HttpGet]
        public ActionResult<IEnumerable<Author>> Get()
        {
            return Ok(_context.Authors);
        }
    }
}
