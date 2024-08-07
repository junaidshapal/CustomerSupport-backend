using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerSupportAPI.Data;
using CustomerSupportAPI.Models;

namespace CustomerSupportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketCommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TicketCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TicketComments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketComment>>> GetTicketComment()
        {
          if (_context.TicketComment == null)
          {
              return NotFound();
          }
            return await _context.TicketComment.ToListAsync();
        }

        // GET: api/TicketComments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketComment>> GetTicketComment(int id)
        {
          if (_context.TicketComment == null)
          {
              return NotFound();
          }
            var ticketComment = await _context.TicketComment.FindAsync(id);

            if (ticketComment == null)
            {
                return NotFound();
            }

            return ticketComment;
        }

        // PUT: api/TicketComments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicketComment(int id, TicketComment ticketComment)
        {
            if (id != ticketComment.Id)
            {
                return BadRequest();
            }

            _context.Entry(ticketComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketCommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TicketComments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TicketComment>> PostTicketComment(TicketComment ticketComment)
        {
          if (_context.TicketComment == null)
          {
              return Problem("Entity set 'ApplicationDbContext.TicketComment'  is null.");
          }
            _context.TicketComment.Add(ticketComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicketComment", new { id = ticketComment.Id }, ticketComment);
        }

        // DELETE: api/TicketComments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicketComment(int id)
        {
            if (_context.TicketComment == null)
            {
                return NotFound();
            }
            var ticketComment = await _context.TicketComment.FindAsync(id);
            if (ticketComment == null)
            {
                return NotFound();
            }

            _context.TicketComment.Remove(ticketComment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketCommentExists(int id)
        {
            return (_context.TicketComment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
