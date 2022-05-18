using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        public string Role { get; set; }
        public double Salary { get; set; }
        [ForeignKey("User")]
        [Required]

        public int UserID { get; set; }

        public virtual User User { get; set; }

    }
         
         
}
