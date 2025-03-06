using dt191g_projekt.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    [Route("api/sanitation")]
    [ApiController]
    public class SanitationApiController : ControllerBase
    {
        //Databas anslutning
        private readonly ApplicationDbContext _context;

        //Konstruktur
        public SanitationApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET api/sanitation
        [HttpGet]
        public async Task<IActionResult> GetSanitations() {
            //Kontroll om _context är null
            if(_context.Sanitations == null) {
                return NotFound();
            }

            return Ok(await _context.Sanitations.ToListAsync());
        }
    }
}
