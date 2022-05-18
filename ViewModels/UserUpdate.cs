using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.ViewModels
{
    public class UserUpdate
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }

        [Phone]

        public string MobilePhone { get; set; }
        public int Last4DigitsSSN { get; set; }
        public int MaxLoginAttempt { get; set; }
        public string LastLogin { get; set; }
        public string Status { get; set; }
        public string EnrolledDate { get; set; }
        public string Password { get; set; }
    }
}
