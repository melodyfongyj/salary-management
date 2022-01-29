using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SalaryManagement.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Please enter your registered email.")]
        [StringLength(100), MaxLength(100)]
        public string Email { get; set; }

        [StringLength(128, ErrorMessage = "Password cannot be more than 128 characters.")]
        public string Password { get; set; }
    }
}
