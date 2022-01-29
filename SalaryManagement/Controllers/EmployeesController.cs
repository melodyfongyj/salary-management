using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalaryManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore.Storage;

namespace SalaryManagement.Controllers
{   
    
    [EnableCors]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly SalaryMgtContext _context;

        public EmployeesController(SalaryMgtContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult Login(LoginVM employee)
        {
            try
            {
                var employees = _context.Employees;
                var pass = _context.HashPassword(employee.Password);
                if (employees.Any(e => e.Email == employee.Email)) //email exist
                {
                    var emp = employees.FirstOrDefault(e => e.Email == employee.Email);
                    if (emp.Email == employee.Email) //get emp by email
                    {
                        if (emp.Password == pass) //hash pw match
                        {
                            if (emp.AccessLevel == 2)
                            {
                                HttpContext.Session.SetString("AdminSession", emp.Email);
                                return Ok(new { status = 1, message = "Admin log-in successful." });
                            }
                            else if (emp.AccessLevel == 1)
                            {
                                HttpContext.Session.SetString("EmpSession", emp.Email);
                                return Ok(new { status = 2, message = "Employee log-in successful." });
                            }
                            else
                            {
                                return Ok(new { status = "Deactivated", message = "Account has been deactivated. Please contact the Administrator." });
                            }
                        }
                    }

                }
                return Ok(new { status = "NotFound", message = "Account does not exist. Please contact the Administrator." });
            }
            catch(Exception e)
            {
                return Ok(new { status = "Error", message = e.Message });
            }
            
        }

        [HttpPost]
        public ActionResult Logout(LoginVM employee)
        {
            
            try
            {
                string empSess = HttpContext.Session.GetString("EmpSession");
                string adminSess = HttpContext.Session.GetString("AdminSession");
                if(employee.Email == adminSess)
                {
                    HttpContext.Session.Remove("AdminSession");
                    return Ok(new { status = "Out", message = "Admin log-out successful." });
                }
                else if(employee.Email == empSess)
                {
                    HttpContext.Session.Remove("EmpSession");
                    return Ok(new { status = "Out", message = "Admin log-out successful." });
                }
                else
                {
                    return Ok(new { status = "Out", message = "No session found." });
                }
                
            }
            catch(Exception e)
            {
                return Ok(new { status = "Error", message = e.Message });
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(LoginVM employee)
        {
            try
            {
                var employees = (from e in _context.Employees
                                 select e).ToList();
                var newPass = _context.HashPassword(employee.Password);
                if (employees.Any(e => e.Email == employee.Email)) //email exist
                {
                    var emp = employees.FirstOrDefault(e => e.Email == employee.Email);
                    if (emp.Email == employee.Email) //find emp based on email
                    {
                        if (emp.AccessLevel == 0)
                        {
                            return Ok(new { status = "Deactivated", message = "Account has been deactivated. Please contact the Administrator." });
                        }
                        else
                        {
                            emp.Password = newPass;
                            _context.SaveChanges();
                            return Ok(new { status = "Ok", message = "Password saved successfully." });
                        }
                    }
                }
                return Ok(new { status = "NotFound", message = "Please contact the Administrator." });
            }
            catch(Exception e)
            {
                return Ok(new { status = "Error", message = e.Message });
            }
            
        }

        //[Route("~/api/Admin")]
        //[HttpPost]
        //public ActionResult<List<DisplayEmpListVM>> Admin()
        //{
        //    try
        //    {
        //        string session = HttpContext.Session.GetString("AdminSession");

        //        if (!String.IsNullOrEmpty(session))
        //        {
        //            var employees = (from e in _context.Employees
        //                             select new DisplayEmpListVM
        //                             {
        //                                 EmployeeId = e.EmployeeId,
        //                                 FirstName = e.FirstName,
        //                                 Email = e.Email,
        //                                 LastName = e.LastName,
        //                                 JobTitle = e.JobTitle,
        //                                 DepartmentName = e.DepartmentName,
        //                                 StartDate = e.StartDate,
        //                                 EndDate = e.EndDate
        //                             }).ToList();

        //            return Ok(new { status = "Ok", message = "Employee list loaded successfully.", obj = employees });
        //        }
        //        else
        //        {
        //            return Ok(new { status = "NotFound", message = "Please log in." });
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        return Ok(new { status = "Error", message = e.Message });
        //    }
            
        //}

        [HttpPost]
        public ActionResult<DisplayEmpVM> Profile(LoginVM emp)
        {
            try
            {
                string adminSess = HttpContext.Session.GetString("AdminSession");
                string empSess = HttpContext.Session.GetString("EmpSession");

                if (!String.IsNullOrEmpty(adminSess) || !String.IsNullOrEmpty(empSess))
                {
                    var employee = from e in _context.Employees
                                   where e.Email == emp.Email
                                   select new DisplayEmpVM
                                   {
                                       EmployeeId = e.EmployeeId,
                                       AccessLevel = e.AccessLevel,
                                       PersonalID = e.PersonalID,
                                       FirstName = e.FirstName,
                                       LastName = e.LastName,
                                       DateOfBirth = e.DateOfBirth,
                                       Age = e.Age,
                                       MobileNo = e.MobileNo,
                                       Address = e.Address,
                                       JobTitle = e.JobTitle,
                                       DepartmentName = e.DepartmentName,
                                       StartDate = e.StartDate,
                                       EndDate = e.EndDate,
                                       EmpSuppDocs = e.EmpSuppDocs,
                                       Email = e.Email,
                                       BankDetail = _context.BankDetails.Where(b => b.EmployeeId == e.EmployeeId).ToList(),
                                       Salary = _context.Salaries.Where(s => s.EmployeeId == e.EmployeeId).ToList()
                                                    //(from e in _context.Employees
                                                    // join b in _context.BankDetails on e.EmployeeId equals b.EmployeeId
                                                    // select b).ToList()
                                   };

                    return Ok(new { status = "Ok", message = "Employee profile loaded successfully.", obj = employee });
                }
                else
                {
                    return Ok(new { status = "NotFound", message = "Please log in." });
                }
            }
            catch(Exception e)
            {
                return Ok(new { status = "Error", message = e.Message });
            }

            
        }

        [HttpPost]
        public ActionResult Add(DisplayEmpVM emp)
        {
            try
            {
                string session = HttpContext.Session.GetString("AdminSession");
                
                if (!String.IsNullOrEmpty(session)) //user is admin
                {
                    var employees = _context.Employees;
                    var salaries = _context.Salaries;
                    var dob = DateTime.ParseExact(emp.DateOfBirth.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    var admin = _context.Employees.FirstOrDefault(a => a.Email == session);

                    if (!employees.Any(e => e.Email == emp.Email)) //email does not exist
                    {
                        using(IDbContextTransaction transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                /* Create record in Employee table*/
                                Employee newEmp = new Employee()
                                {
                                    AccessLevel = emp.AccessLevel,
                                    FirstName = emp.FirstName,
                                    LastName = emp.LastName,
                                    Email = emp.Email,
                                    PersonalID = emp.PersonalID,
                                    Address = emp.Address,
                                    MobileNo = emp.MobileNo,
                                    DateOfBirth = dob,
                                    Age = DateTime.Today.Year - dob.Year,
                                    JobTitle = emp.JobTitle,
                                    DepartmentName = emp.DepartmentName,
                                    StartDate = DateTime.ParseExact(emp.StartDate.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                    EndDate = DateTime.ParseExact(emp.StartDate.AddYears(10).Date.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                    EmpSuppDocs = emp.EmpSuppDocs
                                }; 
                                employees.Add(newEmp);
                                _context.SaveChanges(); //after save, will generate identity

                                /* Create record in BankDetail table */
                                var empId = _context.Employees.FirstOrDefault(e => e.Email == emp.Email);
                                foreach (var item in emp.BankDetail)
                                {
                                    BankDetail newBank = new BankDetail
                                    {
                                        BankName = item.BankName,
                                        BranchCode = item.BranchCode,
                                        AccountNo = item.AccountNo,
                                        EmployeeId = empId.EmployeeId
                                    };
                                    _context.BankDetails.Add(newBank);
                                    _context.SaveChanges();
                                }

                                /* Create record in Salary table */
                                foreach(var item in emp.Salary)
                                {
                                    Salary newSal = new Salary()
                                    {
                                        SalaryAmt = item.SalaryAmt,
                                        ModifiedBy = admin.EmployeeId,
                                        BasicSalary = item.BasicSalary,
                                        Bonus = item.Bonus,
                                        Deduction = item.Deduction,
                                        Remarks = item.Remarks,
                                        Month = item.Month,
                                        Year = item.Year,
                                        InvoiceDate = item.InvoiceDate,
                                        EmployeeId = empId.EmployeeId
                                    };
                                    salaries.Add(newSal);
                                    _context.SaveChanges();
                                }


                                /* Commit transaction */
                                transaction.Commit();

                                return Ok(new { status = "Ok", message = "New employee added successfully.", obj = newEmp });
                            }
                            catch(Exception e)
                            {
                                transaction.Rollback();
                                return Ok(new { status = "Transaction Error", message = e.Message });
                            }
                        }
 
                    }
                }
                return Ok(new { status = "NotFound", message = "Please contact the Administrator." });
            }
            catch(Exception e)
            {
                return Ok(new { status = "Error", message = e.Message });
            }
            
        }

        [HttpPost]
        public ActionResult UpdateEmployee(DisplayEmpVM emp) //after click on button
        {
            try
            {
                string empSession = HttpContext.Session.GetString("EmpSession"),
                        adminSession = HttpContext.Session.GetString("AdminSession"), 
                        bName, accNo, bCode;
                int dbCount, newCount;
                /* EMPLOYEE
                 * Can edit Personal and Bank Details
                 */
                if (!String.IsNullOrEmpty(empSession)) 
                {
                    var employee = _context.Employees.FirstOrDefault(e => e.Email == emp.Email);
                    var bankDetail = _context.BankDetails.Where(b => b.EmployeeId == emp.EmployeeId).ToList();
                    dbCount = bankDetail.Count;
                    newCount = emp.BankDetail.Count;

                    //Personal
                    employee.FirstName = emp.FirstName;
                    employee.LastName = emp.LastName;
                    employee.PersonalID = emp.PersonalID;
                    employee.Address = emp.Address;
                    employee.MobileNo = emp.MobileNo;
                    employee.DateOfBirth = DateTime.ParseExact(
                                            emp.DateOfBirth.ToString("yyyy-MM-dd"),
                                            "yyyy-MM-dd",
                                            CultureInfo.InvariantCulture);
                    employee.Age = emp.Age;
                    employee.EmpSuppDocs = emp.EmpSuppDocs;
                    _context.SaveChanges();

                    //Bank Details
                    /* If record exists in db and same number, overwrite */
                    if ((dbCount > 0) && (dbCount == newCount))
                    {
                        using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                for (int i = 0; i < dbCount; i++)
                                {
                                    bankDetail[i].BankName = emp.BankDetail[i].BankName;
                                    bankDetail[i].BranchCode = emp.BankDetail[i].BranchCode;
                                    bankDetail[i].AccountNo = emp.BankDetail[i].AccountNo;
                                    bankDetail[i].AccountStatus = emp.BankDetail[i].AccountStatus;
                                    _context.SaveChanges();
                                }
                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                return Ok(new { status = "Txn Error: Failed to overwrite existing Bank Details.", message = e.Message });
                            }
                        }

                    }
                    /* db record exists and less than new, will never be > newCount due to delete api */
                    else if ((dbCount > 0) && (dbCount < newCount))
                    {
                        using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                foreach (var item in emp.BankDetail)
                                {
                                    var recordExists = bankDetail.Find(b => b.BankDetailId == item.BankDetailId);
                                    if (recordExists != null)
                                    {
                                        recordExists.BankName = item.BankName;
                                        recordExists.BranchCode = item.BranchCode;
                                        recordExists.AccountNo = item.AccountNo;
                                        recordExists.AccountStatus = item.AccountStatus;
                                        _context.SaveChanges();
                                    }
                                    if (recordExists == null)
                                    {
                                        BankDetail addBank = new BankDetail
                                        {
                                            BankName = item.BankName,
                                            BranchCode = item.BranchCode,
                                            AccountNo = item.AccountNo,
                                            EmployeeId = employee.EmployeeId,
                                            AccountStatus = item.AccountStatus
                                        };
                                        _context.BankDetails.Add(addBank);
                                        _context.SaveChanges();
                                    }
                                }
                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                return Ok(new { status = "Txn Error: Failed to overwrite existing Bank Details.", message = e.Message });
                            }
                        }

                    }
                    else
                    {
                        var empId = _context.Employees.FirstOrDefault(e => e.Email == emp.Email);
                        using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                foreach (var item in emp.BankDetail)
                                {
                                    BankDetail addBank = new BankDetail
                                    {
                                        BankName = item.BankName,
                                        BranchCode = item.BranchCode,
                                        AccountNo = item.AccountNo,
                                        EmployeeId = empId.EmployeeId
                                    };
                                    _context.BankDetails.Add(addBank);
                                    _context.SaveChanges();
                                }
                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                return Ok(new { status = "Txn Error: Failed to overwrite existing Bank Details.", message = e.Message });
                            }
                        }

                    }

                    return Ok(new { status = "Ok", message = "Employee profile saved successfully.", obj = employee });
                }

                /* ADMIN
                 * Can edit Personal, Work and Bank details
                 */
                else if (!String.IsNullOrEmpty(adminSession)) 
                {
                    var employee = _context.Employees.FirstOrDefault(e => e.Email == emp.Email);
                    var bankDetail = (from b in _context.BankDetails
                                      where b.EmployeeId == emp.EmployeeId
                                      select b).ToList();
                    dbCount = bankDetail.Count;
                    newCount = emp.BankDetail.Count;

                    employee.FirstName = emp.FirstName;
                    employee.LastName = emp.LastName;
                    employee.PersonalID = emp.PersonalID;
                    employee.Address = emp.Address;
                    employee.MobileNo = emp.MobileNo;
                    employee.DateOfBirth = DateTime.ParseExact(
                                            emp.DateOfBirth.ToString("yyyy-MM-dd"),
                                            "yyyy-MM-dd",
                                            CultureInfo.InvariantCulture);
                    employee.Age = emp.Age;
                    employee.EmpSuppDocs = emp.EmpSuppDocs;
                    employee.AccessLevel = emp.AccessLevel;
                    employee.JobTitle = emp.JobTitle;
                    employee.DepartmentName = emp.DepartmentName;
                    employee.StartDate = DateTime.ParseExact(
                                            emp.StartDate.ToString("yyyy-MM-dd"),
                                            "yyyy-MM-dd",
                                            CultureInfo.InvariantCulture);
                    employee.EndDate = DateTime.ParseExact(
                                            emp.EndDate.ToString("yyyy-MM-dd"),
                                            "yyyy-MM-dd",
                                            CultureInfo.InvariantCulture);
                    employee.Email = emp.Email;

                    _context.SaveChanges();

                    /* If record exists in db and same number, overwrite */
                    if ((dbCount > 0) && (dbCount == newCount))
                    {
                        using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                for (int i = 0; i < dbCount; i++)
                                {
                                    bankDetail[i].BankName = emp.BankDetail[i].BankName;
                                    bankDetail[i].BranchCode = emp.BankDetail[i].BranchCode;
                                    bankDetail[i].AccountNo = emp.BankDetail[i].AccountNo;
                                    bankDetail[i].AccountStatus = emp.BankDetail[i].AccountStatus;
                                    _context.SaveChanges();
                                }
                                transaction.Commit();
                            }
                            catch(Exception e)
                            {
                                transaction.Rollback();
                                return Ok(new { status = "Txn Error: Failed to overwrite existing Bank Details.", message = e.Message });
                            }
                        }
                        
                    }
                    /* db record exists and less than new, will never be > newCount due to delete api */
                    else if((dbCount > 0) && (dbCount < newCount))
                    {
                        using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                foreach (var item in emp.BankDetail)
                                {
                                    var recordExists = bankDetail.Find(b => b.BankDetailId == item.BankDetailId);
                                    if (recordExists != null)
                                    {
                                        recordExists.BankName = item.BankName;
                                        recordExists.BranchCode = item.BranchCode;
                                        recordExists.AccountNo = item.AccountNo;
                                        recordExists.AccountStatus = item.AccountStatus;
                                        _context.SaveChanges();
                                    }
                                    if (recordExists == null)
                                    {
                                        BankDetail addBank = new BankDetail
                                        {
                                            BankName = item.BankName,
                                            BranchCode = item.BranchCode,
                                            AccountNo = item.AccountNo,
                                            EmployeeId = employee.EmployeeId,
                                            AccountStatus = item.AccountStatus
                                        };
                                        _context.BankDetails.Add(addBank);
                                        _context.SaveChanges();
                                    }
                                }
                                transaction.Commit();
                            }
                            catch(Exception e)
                            {
                                transaction.Rollback();
                                return Ok(new { status = "Txn Error: Failed to overwrite existing Bank Details.", message = e.Message });
                            }
                        }
                        
                    }
                    else
                    {
                        var empId = _context.Employees.FirstOrDefault(e => e.Email == emp.Email);
                        using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                foreach (var item in emp.BankDetail)
                                {
                                    BankDetail addBank = new BankDetail
                                    {
                                        BankName = item.BankName,
                                        BranchCode = item.BranchCode,
                                        AccountNo = item.AccountNo,
                                        EmployeeId = empId.EmployeeId
                                    };
                                    _context.BankDetails.Add(addBank);
                                    _context.SaveChanges();
                                }
                                transaction.Commit();
                            }
                            catch(Exception e)
                            {
                                transaction.Rollback();
                                return Ok(new { status = "Txn Error: Failed to overwrite existing Bank Details.", message = e.Message });
                            }
                        }
                        
                    }

                    return Ok(new { status = "Ok", message = "Employee profile saved successfully.", obj = employee });
                }
                else
                {
                    return Ok(new { status = "NotFound", message = "Please log in." });
                }
            } 
            catch(Exception e)
            {
                return Ok(new { status = "Error", message = e.Message });
            }
            
        }

        //[Route("~/api/Admin/Salary")]
        //[HttpPost]
        //public ActionResult SalaryList()
        //{
        //    try
        //    {
        //        //HttpContext.Session.SetString("AdminSession", "mich@gmail.com");
        //        string session = HttpContext.Session.GetString("AdminSession");

        //        if (!String.IsNullOrEmpty(session))
        //        {
        //            var employees = _context.Employees;
        //            var salaries = _context.Salaries;
        //            var display = from emp in employees
        //                          join sal in salaries on emp.SalaryId equals sal.SalaryId
        //                          select new
        //                          {
        //                              Name = emp.FirstName + " " + emp.LastName,
        //                              emp.EmployeeId,
        //                              sal.SalaryId,
        //                              sal.BasicSalary,
        //                              sal.Bonus,
        //                              sal.Deduction,
        //                              sal.Remarks,
        //                              sal.SalaryAmt
        //                          };

        //            return Ok(new { status = "Ok", message = "Salary list loaded successfully.", obj = display });
        //        }
        //        else
        //        {
        //            return Ok(new { status = "NotFound", message = "Please log in." });
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        return Ok(new { status = "Error", message = e.Message });
        //    }
            
        //}

        //[Route("~/api/Admin/UpdateSalaryList")]
        //[HttpPost]
        //public ActionResult UpdateSalaryList(SalaryListVM list)
        //{
        //    try
        //    {
        //        string session = HttpContext.Session.GetString("AdminSession");
        //        var admin = _context.Employees.FirstOrDefault(a => a.Email == session);
        //        var employees = (from e in _context.Employees
        //                        select e).ToList();
        //        var salaries = (from s in _context.Salaries
        //                        select s).ToList();

        //        if (!String.IsNullOrEmpty(session))
        //        {
        //            foreach (var e in employees)
        //            {
        //                if (e.EmployeeId == list.EmployeeId) //EmpID matches
        //                {
        //                    var salary = salaries.FirstOrDefault(s => s.SalaryId == e.SalaryId);
        //                    salary.SalaryAmt = Convert.ToDecimal(list.SalaryAmt);
        //                    salary.ModifiedBy = admin.EmployeeId;
        //                    salary.BasicSalary = Convert.ToDecimal(list.BasicSalary);
        //                    salary.Bonus = Convert.ToDecimal(list.Bonus);
        //                    salary.Deduction = Convert.ToDecimal(list.Deduction);
        //                    salary.Remarks = list.Remarks;

        //                    _context.SaveChanges();

        //                }
                        
        //            }
        //            return Ok(new { status = "Ok", message = "Saved successfully.", obj = salaries });
        //        }
        //        else
        //        {
        //            return Ok(new { status = "NotFound", message = "Please log in." });
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        return Ok(new { status = "Error", message = e.Message });
        //    }
            
        //}
        
        [HttpPost]
        public ActionResult DeleteBankDetail(int id)
        {
            string empSession = HttpContext.Session.GetString("EmpSession"),
                   adminSession = HttpContext.Session.GetString("AdminSession");
            
            //var admin = _context.Employees.FirstOrDefault(a => a.Email == session);

            if (!String.IsNullOrEmpty(empSession) || !String.IsNullOrEmpty(adminSession))
            {
                try
                {
                    var record = _context.BankDetails.Find(id);
                    if(record != null)
                    {
                        _context.BankDetails.Remove(record);
                        _context.SaveChanges();
                    }
                    
                }
                catch(Exception e)
                {
                    return Ok(new { status = "Error", message = e.Message });
                }
                
                return Ok(new { status = "Ok", message = "Employee profile deleted." });
            }
            else
            {
                return Ok(new { status = "NotFound", message = "Please log in." });
            }
            //return Ok();
        }

    }
}
