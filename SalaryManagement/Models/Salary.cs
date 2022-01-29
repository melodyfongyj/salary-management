using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
//using Newtonsoft.Json;

namespace SalaryManagement.Models
{
    public class Salary
    {
        public int SalaryId { get; set; }

        [MaxLength(3)]
        public string Month { get; set; }

        [MaxLength(4)]
        public string Year { get; set; }

        [Column(TypeName = "Date")]
        public DateTime InvoiceDate { get; set; } = DateTime.Now.Date;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SalaryAmt { get; set; }

        public int ModifiedBy { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Bonus { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Deduction { get; set; }

        [MaxLength(255)]
        public string Remarks { get; set; }


        ///* EMP-SAL 1:1 */
        //public Employee Employee { get; set; }

        /* EMP-SAL 1:M */
        public int EmployeeId { get; set; }

        [JsonIgnore]
        public Employee Employee { get; set; }
    }
}
