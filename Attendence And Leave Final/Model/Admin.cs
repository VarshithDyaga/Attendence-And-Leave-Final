using System.ComponentModel.DataAnnotations;

namespace Attendence_And_Leave_Final.Model
{
    public class Admin : AdminLoginModel
    {
        [Key]
        public int Id { get; set; }

       
        public string UserName { get; set; }

        public string? Name { get; set; }


        public string Email { get; set; } 
       
        public string Password { get; set; }
    }

    public class AdminLoginModel
    {
        
        public string UserName { get; set; }
       

        public string Password { get; set; }
    }
}
