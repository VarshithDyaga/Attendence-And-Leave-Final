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
    public class EmployeesController : ControllerBase
    {
        private readonly Attendance_Leave_Context _context;

        public EmployeesController(Attendance_Leave_Context context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employees>>> GetEmployeeData()
        {
          if (_context.EmployeeData == null)
          {
              return NotFound();
          }
            return await _context.EmployeeData.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employees>> GetEmployees(int id)
        {
          if (_context.EmployeeData == null)
          {
              return NotFound();
          }
            var employees = await _context.EmployeeData.FindAsync(id);

            if (employees == null)
            {
                return NotFound();
            }

            return employees;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployees(int id, Employees employees)
        {
            if (id != employees.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(employees).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeesExists(id))
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

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employees>> PostEmployees(Employees employees)
        {
          if (_context.EmployeeData == null)
          {
              return Problem("Entity set 'Attendance_Leave_Context.EmployeeData'  is null.");
          }
            _context.EmployeeData.Add(employees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployees", new { id = employees.EmployeeId }, employees);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployees(int id)
        {
            if (_context.EmployeeData == null)
            {
                return NotFound();
            }
            var employees = await _context.EmployeeData.FindAsync(id);
            if (employees == null)
            {
                return NotFound();
            }

            _context.EmployeeData.Remove(employees);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeesExists(int id)
        {
            return (_context.EmployeeData?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
        [HttpPost]
        [Route("Login")]
        public ActionResult Login([FromBody] EmployeeLogin employeeLogin)
        {
            var currentAdmin = _context.EmployeeData.FirstOrDefault(x => x.UserName == employeeLogin.UserName && x.Password == employeeLogin.Password && x.designation==employeeLogin.designation);
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
