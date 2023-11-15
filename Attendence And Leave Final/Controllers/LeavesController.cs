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
    public class LeavesController : ControllerBase
    {
        private readonly Attendance_Leave_Context _context;
        private readonly IConfiguration config;

        public LeavesController(Attendance_Leave_Context context, IConfiguration config)
        {
            _context = context;
            this.config = config;
        }

        // GET: api/Leaves
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leaves>>> GetLeaveData()
        {
          if (_context.LeaveData == null)
          {
              return NotFound();
          }
            return await _context.LeaveData.ToListAsync();
        }

        // GET: api/Leaves/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Leaves>> GetLeaves(int id)
        {
          if (_context.LeaveData == null)
          {
              return NotFound();
          }
            var leaves = await _context.LeaveData.FindAsync(id);

            if (leaves == null)
            {
                return NotFound();
            }

            return leaves;
        }

        // PUT: api/Leaves/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaves(int id, Leaves leaves)
        {
            if (id != leaves.LeaveId)
            {
                return BadRequest();
            }

            _context.Entry(leaves).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeavesExists(id))
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

        // POST: api/Leaves
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Leaves>> PostLeaves(Leaves leaves)
        {
            if (_context.LeaveData == null)
            {
                return Problem("Entity set 'Attendance_Leave_Context.LeaveData'  is null.");
            }
            _context.LeaveData.Add(leaves);
            await _context.SaveChangesAsync();

            bool _isuploaded = await Helper.UploadBlob(config, leaves);

            if (_isuploaded)
            {
                return CreatedAtAction("GetLeaves", new { id = leaves.LeaveId }, leaves);
            }
            return StatusCode(200);
        }

        // DELETE: api/Leaves/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaves(int id)
        {
            if (_context.LeaveData == null)
            {
                return NotFound();
            }
            var leaves = await _context.LeaveData.FindAsync(id);
            if (leaves == null)
            {
                return NotFound();
            }

            _context.LeaveData.Remove(leaves);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPatch("ApproveLeave/{id}")]
        public async Task<IActionResult> ApproveLeave(int id)
        {
            var leave = await _context.LeaveData.FindAsync(id);

            if (leave == null)
            {
                return NotFound();
            }

            if (leave.LeaveStatus != "pending")
            {
                // Leave is not in a pending state, cannot be approved
                return BadRequest("Leave is not in a pending state and cannot be approved.");
            }

            leave.LeaveStatus = "Approved";

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeavesExists(id))
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

        [HttpPatch("RejectLeave/{id}")]
        public async Task<IActionResult> RejectLeave(int id)
        {
            var leave = await _context.LeaveData.FindAsync(id);

            if (leave == null)
            { 
                return NotFound();
            }

            if (leave.LeaveStatus != "pending")
            {
                // Leave is not in a pending state, cannot be rejected
                return BadRequest("Leave is not in a pending state and cannot be rejected.");
            }

            leave.LeaveStatus = "Rejected";
             
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeavesExists(id))
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



        // GET: api/Leaves/employee/{employeeId}

        [HttpGet("employee/{employeeId}")]

        public IActionResult GetLeaveByEmployeeId(int employeeId)

        {

            try

            {

                var leaveDetails = _context.LeaveData.Where(l => l.EmployeeId == employeeId).ToList();

                if (leaveDetails == null || leaveDetails.Count == 0)

                {

                    return NotFound($"No leave details found for employee with ID {employeeId}");

                }

                return Ok(leaveDetails);

            }

            catch (Exception ex)

            {

                return StatusCode(500, $"Internal server error: {ex.Message}");

            }

        }

        private bool LeavesExists(int id)

        {

            return (_context.LeaveData?.Any(e => e.LeaveId == id)).GetValueOrDefault();
        }

    }
    
}

