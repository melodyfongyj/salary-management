using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalaryManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;

namespace SalaryManagement.Controllers
{
    [EnableCors]
    [Route("api/[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly SalaryMgtContext _context;

        public AdminController(SalaryMgtContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult<List<DisplayEmpListVM>> Home()
        {
            try
            {
                string session = HttpContext.Session.GetString("AdminSession");

                if (!String.IsNullOrEmpty(session))
                {
                    var employees = (from e in _context.Employees
                                     select new DisplayEmpListVM
                                     {
                                         EmployeeId = e.EmployeeId,
                                         FirstName = e.FirstName,
                                         Email = e.Email,
                                         LastName = e.LastName,
                                         JobTitle = e.JobTitle,
                                         DepartmentName = e.DepartmentName,
                                         StartDate = e.StartDate,
                                         EndDate = e.EndDate
                                     }).ToList();

                    return Ok(new { status = "Ok", message = "Employee list loaded successfully.", obj = employees });
                }
                else
                {
                    return Ok(new { status = "NotFound", message = "Please log in." });
                }
            }
            catch (Exception e)
            {
                return Ok(new { status = "Error", message = e.Message });
            }

        }

        [HttpPost]
        public ActionResult SalaryList()
        {
            try
            {
                //HttpContext.Session.SetString("AdminSession", "mich@gmail.com");
                string session = HttpContext.Session.GetString("AdminSession");

                if (!String.IsNullOrEmpty(session))
                {
                    var employees = _context.Employees;
                    var salaries = _context.Salaries;
                    var display = from emp in employees
                                  join sal in salaries on emp.EmployeeId equals sal.EmployeeId
                                  select new SalaryVM
                                  {
                                      Name = emp.FirstName + " " + emp.LastName,
                                      EmployeeId = sal.EmployeeId,
                                      SalaryId = sal.SalaryId,
                                      BasicSalary = sal.BasicSalary,
                                      Bonus = sal.Bonus,
                                      Deduction = sal.Deduction,
                                      Remarks = sal.Remarks,
                                      SalaryAmt = sal.SalaryAmt,
                                      Month = sal.Month,
                                      Year = sal.Year,
                                      InvoiceDate = sal.InvoiceDate,
                                      ModifiedBy = sal.ModifiedBy
                                  };

                    return Ok(new { status = "Ok", message = "Salary list loaded successfully.", obj = display });
                }
                else
                {
                    return Ok(new { status = "NotFound", message = "Please log in." });
                }
            }
            catch (Exception e)
            {
                return Ok(new { status = "Error", message = e.Message });
            }

        }

        [HttpPost]
        public ActionResult UpdateSalary([FromBody] SalaryVM entry)
        {
            try
            {
                string session = HttpContext.Session.GetString("AdminSession");
                var admin = _context.Employees.FirstOrDefault(a => a.Email == session);

                if (!String.IsNullOrEmpty(session))
                {
                    var salRecords = _context.Salaries.Where(s => s.EmployeeId == entry.EmployeeId).ToList(); //grab all the salaries records by empid
                    var nullRecord = salRecords.Find(s => (s.Month == null) && (s.Year == null));
                    var record = salRecords.Find(s => (s.Month == entry.Month) && (s.Year == s.Year));

                    //if null record exists, overwrite. null only happens for new account
                    if(nullRecord != null)
                    {
                        nullRecord.Month = entry.Month;
                        nullRecord.Year = entry.Year;
                        nullRecord.ModifiedBy = admin.EmployeeId;
                        nullRecord.SalaryAmt = Convert.ToDecimal(entry.SalaryAmt);
                        nullRecord.BasicSalary = Convert.ToDecimal(entry.BasicSalary);
                        nullRecord.Bonus = Convert.ToDecimal(entry.Bonus);
                        nullRecord.Deduction = Convert.ToDecimal(entry.Deduction);
                        nullRecord.Remarks = entry.Remarks;
                        nullRecord.InvoiceDate = entry.InvoiceDate;
                        _context.SaveChanges();
                    }
                    
                    //if matching record found
                    else if(record != null)
                    {
                        //add
                        record.ModifiedBy = admin.EmployeeId;
                        record.SalaryAmt = Convert.ToDecimal(entry.SalaryAmt);
                        record.BasicSalary = Convert.ToDecimal(entry.BasicSalary);
                        record.Bonus = Convert.ToDecimal(entry.Bonus);
                        record.Deduction = Convert.ToDecimal(entry.Deduction);
                        record.Remarks = entry.Remarks;
                        record.InvoiceDate = entry.InvoiceDate;
                        _context.SaveChanges();
                    }
                    else
                    {
                        Salary newSal = new Salary()
                        {
                            SalaryAmt = entry.SalaryAmt,
                            ModifiedBy = admin.EmployeeId,
                            BasicSalary = entry.BasicSalary,
                            Bonus = entry.Bonus,
                            Deduction = entry.Deduction,
                            Remarks = entry.Remarks,
                            Month = entry.Month,
                            Year = entry.Year,
                            InvoiceDate = entry.InvoiceDate,
                            EmployeeId = entry.EmployeeId
                        };
                        _context.Salaries.Add(newSal);
                        _context.SaveChanges();
                    }

                    return Ok(new { status = "Ok", message = "Saved successfully.", obj = salRecords });
                }
                else
                {
                    return Ok(new { status = "NotFound", message = "Please log in." });
                }
            }
            catch (Exception e)
            {
                return Ok(new { status = "Error", message = e.Message });
            }

        }
    }
}
