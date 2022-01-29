using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaryManagement.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public int AccessLevel { get; set; } = 1;

        [MaxLength(9)]
        public string PersonalID { get; set; }

        [Required(ErrorMessage = "Please enter First Name.")]
        [MaxLength(40)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name.")]
        [MaxLength(40)]
        public string LastName { get; set; }

        public int Age { get; set; }

        [Column(TypeName = "Date")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(15)]
        public string MobileNo { get; set; }

        public string Address { get; set; }

        [MaxLength(100)]
        public string JobTitle { get; set; }

        [MaxLength(100)]
        public string DepartmentName { get; set; }

        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;

        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; } = DateTime.Now.AddYears(10).Date;

        public string EmpSuppDocs { get; set; }

        //https://stackoverflow.com/questions/1199190/what-is-the-optimal-length-for-an-email-address-in-a-database
        [Required(ErrorMessage = "Please enter your registered email.")]
        [StringLength(100), MaxLength(100)]
        public string Email { get; set; }

        [StringLength(128, ErrorMessage = "Password cannot be more than 128 characters.")]
        public string Password { get; set; }

        ///* EMP-SAL 1:1 */
        //public int SalaryId { get; set; } //FK
        //public Salary Salary { get; set; }

        /* EMP-SAL 1:M */
        public ICollection<Salary> Salary { get; set; }

        /* EMP-BANK 1:M */
        public ICollection<BankDetail> BankDetail { get; set; }

    }

}
