using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
//using Newtonsoft.Json;

namespace SalaryManagement.Models
{
    public class BankDetail
    {
        public int BankDetailId { get; set; }

        [MaxLength(255)]
        public string BankName { get; set; }

        public string BranchCode { get; set; }

        [MaxLength(18)]
        public string AccountNo { get; set; }

        public int AccountStatus { get; set; } = 0;


        /* EMP-BANK 1:M */
        public int EmployeeId { get; set; }

        [JsonIgnore]
        public Employee Employee { get; set; }
    }
}
