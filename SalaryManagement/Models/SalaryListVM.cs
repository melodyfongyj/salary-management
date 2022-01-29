using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaryManagement.Models
{
    public class SalaryVM
    {
        public int SalaryId { get; set; }

        public int EmployeeId { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SalaryAmt { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Bonus { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Deduction { get; set; }

        [MaxLength(255)]
        public string Remarks { get; set; }

        public int ModifiedBy { get; set; }

        [MaxLength(3)]
        public string Month { get; set; }

        [MaxLength(4)]
        public string Year { get; set; }

        [Column(TypeName = "Date")]
        public DateTime InvoiceDate { get; set; } = DateTime.Now.Date;


    }
}
