using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerSupportAPI.Data;
using CustomerSupportAPI.Models;
using CustomerSupportAPI.DataTransferObjects;
using CustomerSupportAPI.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CustomerSupportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
          if (_context.Tickets == null)
          {
              return NotFound();
          }
            var list = await _context.Tickets.ToListAsync();

            return list;

        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
          if (_context.Tickets == null)
          {
              return NotFound();
          }

            var ticket = await _context.Tickets.Include(t => t.Comments).FirstOrDefaultAsync(t => t.Id == id);

            //var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        //public async Task<ActionResult<IEnumerable<Ticket>>> SearchTickets(string name)
        //{
        //    if(string.IsNullOrWhiteSpace(name))
        //    {
        //        return NotFound();
        //    }         

        //    var tickets = await _context.Tickets.Where(t => t.Title.Contains(name)).ToListAsync();

        //    return Ok(tickets);
        //}

        // PUT: api/Tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            var existingTicket = await _context.Tickets.FindAsync(id);

            if (existingTicket == null)
            {
                return NotFound();
            }
            //if (id != ticket.Id)
            //{
            //    return BadRequest();
            //}

            //var changes = new Dictionary<string, string>();

            //if (existingTicket.Title != ticket.Title)
            //{
            //    changes.Add("Title", ticket.Title);
            //}

            //if (existingTicket.Description != ticket.Description)
            //{
            //    changes.Add("Description", ticket.Description);
            //}

            //_context.Entry(ticket).State = EntityState.Modified;
            //await _context.SaveChangesAsync();

            existingTicket.Title = ticket.Title;
            existingTicket.Description = ticket.Description;
            existingTicket.AssignedTo = ticket.AssignedTo;
            existingTicket.Status = ticket.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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

        // POST: api/Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
          if (_context.Tickets == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
          }

            //if(ticket.Status == Enums.TicketStatus.Unknown)
            //  {
            //      ticket.Status = Enums.TicketStatus.InProgress;
            //      }

            //ticket.Status = (int)Enums.TicketStatus.InProgress;
            //ticket.Status = (int)Enums.TicketStatus.Completed;

            ticket.Status = ticket.Status;
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            if (_context.Tickets == null)
            {
                return NotFound();
            }
            //var ticket = await _context.Tickets.FindAsync(id);
            var ticket = await _context.Tickets.Include(t => t.Comments).FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(int id)
        {
            return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
