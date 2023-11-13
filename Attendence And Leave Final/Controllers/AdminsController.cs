using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Attendence_And_Leave_Final.Model;
using System.Security.Policy;

namespace Attendence_And_Leave_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly Attendance_Leave_Context _context;

        public AdminsController(Attendance_Leave_Context context)
        {
            _context = context;
        }

        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdminData()
        {
            if (_context.AdminData == null)
            {
                return NotFound();
            }
            return await _context.AdminData.ToListAsync();
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
            if (_context.AdminData == null)
            {
                return NotFound();
            }
            var admin = await _context.AdminData.FindAsync(id);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        // PUT: api/Admins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmin(int id, Admin admin)
        {
            if (id != admin.Id)
            {
                return BadRequest();
            }

            _context.Entry(admin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
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

        // POST: api/Admins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Admin>> PostAdmin(Admin admin)
        {
            if (_context.AdminData == null)
            {
                return Problem("Entity set 'Attendance_Leave_Context.AdminData'  is null.");
            }
            _context.AdminData.Add(admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdmin", new { id = admin.Id }, admin);
        }

        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            if (_context.AdminData == null)
            {
                return NotFound();
            }
            var admin = await _context.AdminData.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            _context.AdminData.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdminExists(int id)
        {
            return (_context.AdminData?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpPost]
        [Route("Login")]
        public ActionResult Login([FromBody] AdminLoginModel adminLogin)
        {
            var currentAdmin = _context.AdminData.FirstOrDefault(x => x.UserName == adminLogin.UserName && x.Password == adminLogin.Password);
            if (currentAdmin == null)
            {
                return NotFound("Invalid Username or password");
            }
            var token = currentAdmin;
            if (token == null)
            {
                return NotFound("Invalid Credentials");
            }
            return Ok(token);
        }
    }
}

