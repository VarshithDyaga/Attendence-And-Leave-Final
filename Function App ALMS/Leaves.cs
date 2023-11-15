
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace Attendence_And_Leave_Final.Model

{

    public class Leaves

    {

        public int LeaveId { get; set; }

        public int? EmployeeId { get; set; }

        public int ProjectCode { get; set; }

        public string EmployeeName { get; set; } = null!;


        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string? LeaveDescription { get; set; }

        public string LeaveStatus { get; set; } = null!;

    }

}
