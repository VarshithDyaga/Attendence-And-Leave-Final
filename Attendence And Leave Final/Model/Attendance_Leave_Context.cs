using Microsoft.EntityFrameworkCore;

namespace Attendence_And_Leave_Final.Model
{
    public class Attendance_Leave_Context : DbContext
    {
        public Attendance_Leave_Context(DbContextOptions<Attendance_Leave_Context> options) : base(options)
        {

        }
        public virtual DbSet<Attendence> AttendenceData { get; set; }

        public virtual DbSet<Employees> EmployeeData { get; set; }

        public virtual DbSet<Leaves> LeaveData { get; set; } 

        

        public virtual DbSet<Project> ProjectData { get; set; }

        public virtual DbSet<Admin> AdminData { get; set; }
    }
}
