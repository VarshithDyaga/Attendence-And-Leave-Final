using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Attendence_And_Leave_Final.Model
{
    
        public class Employees : EmployeeLogin
        {

            [Key]
            public int EmployeeId { get; set; }


            [ForeignKey(nameof(project))]
            public int? ProjectCode { get; set; }

            [JsonIgnore]
            public virtual Project? project { get; set; }

             public string UserName { get; set; }

            public string firstName { get; set; }

            public string lastName { get; set; }

            public string Password { get; set; }

            public string email { get; set; } = null!;

           

            public string designation {  get; set; }

        }
        public class EmployeeLogin
        {
           
            public string UserName { get; set; }
           
            public string Password { get; set; }
            public string designation { get; set; }
        }
    }

