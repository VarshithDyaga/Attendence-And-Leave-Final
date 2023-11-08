using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Attendence_And_Leave_Final.Model;

namespace Attendence_And_Leave_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendencesController : ControllerBase
    {
        private readonly Attendance_Leave_Context _context;

        public AttendencesController(Attendance_Leave_Context context)
        {
            _context = context;
        }

        // GET: api/Attendences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendence>>> GetAttendenceData()
        {
          if (_context.AttendenceData == null)
          {
              return NotFound();
          }
            return await _context.AttendenceData.ToListAsync();
        }

        // GET: api/Attendences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attendence>> GetAttendence(int id)
        {
          if (_context.AttendenceData == null)
          {
              return NotFound();
          }
            var attendence = await _context.AttendenceData.FindAsync(id);

            if (attendence == null)
            {
                return NotFound();
            }

            return attendence;
        }

        // PUT: api/Attendences/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttendence(int id, Attendence attendence)
        {
            if (id != attendence.AttendenceId)
            {
                return BadRequest();
            }

            _context.Entry(attendence).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendenceExists(id))
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

        // POST: api/Attendences
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Attendence>> PostAttendence(Attendence attendence)
        {
          if (_context.AttendenceData == null)
          {
              return Problem("Entity set 'Attendance_Leave_Context.AttendenceData'  is null.");
          }
            _context.AttendenceData.Add(attendence);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAttendence", new { id = attendence.AttendenceId }, attendence);
        }

        // DELETE: api/Attendences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendence(int id)
        {
            if (_context.AttendenceData == null)
            {
                return NotFound();
            }
            var attendence = await _context.AttendenceData.FindAsync(id);
            if (attendence == null)
            {
                return NotFound();
            }

            _context.AttendenceData.Remove(attendence);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AttendenceExists(int id)
        {
            return (_context.AttendenceData?.Any(e => e.AttendenceId == id)).GetValueOrDefault();
        }
    }
}
