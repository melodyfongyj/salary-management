using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaryManagement.Models
{
    public class DisplayEmpListVM
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please enter your registered email.")]
        [StringLength(100), MaxLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter First Name.")]
        [MaxLength(40)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name.")]
        [MaxLength(40)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string JobTitle { get; set; }

        [MaxLength(100)]
        public string DepartmentName { get; set; }

        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;

        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }

        [MaxLength(1)]
        public int AccessLevel { get; set; }

    }
}
