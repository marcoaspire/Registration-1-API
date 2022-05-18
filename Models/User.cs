using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistrationAPI.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]

        public string MiddleName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        [Phone]

        public string MobilePhone { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public int Last4DigitsSSN { get; set; }
        [Required]
        public bool TermsandConditions { get; set; }
        
        public int MaxLoginAttempt { get; set; }
        public string LastLogin { get; set; }
        public string Status { get; set; }
        public string EnrolledDate { get; set; }
        public string Password { get; set; }

        public virtual Employee Employee { get; set; }



    }
}
