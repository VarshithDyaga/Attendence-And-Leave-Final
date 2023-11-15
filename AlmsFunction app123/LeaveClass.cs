using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmsFunction_app123
{
    public class Leaves
    {
        public int LeaveId { get; set; }

        public int EmployeeId { get; set; }




        public int ProjectCode { get; set; }



        public string EmployeeName { get; set; } = null!;

        public string EmployeeRole { get; set; } = null!;

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string LeaveDescription { get; set; }

        public string LeaveStatus { get; set; } = null!;
    }
}
