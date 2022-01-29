using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace SalaryManagement.Models
{
    public class SalaryMgtContext: DbContext
    {
        public SalaryMgtContext(DbContextOptions<SalaryMgtContext> options): base(options)
        {
        }

        public DbSet<BankDetail> BankDetails { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Salary> Salaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* Set properties */
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            //modelBuilder.Entity<Employee>()
            //    .Property(e => e.Age)
            //    .HasComputedColumnSql("CAST(FLOOR(CAST(CONVERT(DATETIME, GetDate()) - CONVERT(DATETIME, DateOfBirth) AS INTEGER) / 365) AS INTEGER)")
            //    .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Password)
                .HasDefaultValue(HashPassword("123"))
                .ValueGeneratedOnAdd();

            /* Create table without plural */
            modelBuilder.Entity<BankDetail>().ToTable("BankDetail");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Salary>().ToTable("Salary");


            /* Populate table with seed data */
            modelBuilder.Entity<Employee>().HasData(
                new Employee { EmployeeId = 1, FirstName = "Mich", LastName = "Anderson", Age = 28, PersonalID = "S1234567A", DateOfBirth = new DateTime(1993, 04, 21), MobileNo = "12345678", Email = "mich@gmail.com", Address = "123 Paya Lebar Dr 6 Singapore 123123", JobTitle = "System Administrator", DepartmentName = "IS", StartDate = new DateTime(2021, 10, 03), AccessLevel = 2 },
                new Employee { EmployeeId = 2, FirstName = "Trish", LastName = "Ng", Age = 31, PersonalID = "S7654321A", DateOfBirth = new DateTime(1990, 07, 12), MobileNo = "87654321", Email = "trish@gmail.com", Address = "321 Paya Lebar Dr 6 Singapore 123321", JobTitle = "Marketing Executive", DepartmentName = "Marketing", StartDate = new DateTime(2022, 01, 13) },
                new Employee { EmployeeId = 3, FirstName = "Jack", LastName = "Lozano", Age = 36, PersonalID = "S1265437A", DateOfBirth = new DateTime(1986, 01, 10), MobileNo = "12457448", Email = "jack@gmail.com", Address = "103 Toa Payoh Dr 6 Singapore 456103", JobTitle = "HR Manager", DepartmentName = "HR", StartDate = new DateTime(2022, 01, 07), AccessLevel = 0 },
                new Employee { EmployeeId = 4, FirstName = "Jill", LastName = "Oconnell", Age = 62, PersonalID = "S7657656A", DateOfBirth = new DateTime(1960, 07, 29), MobileNo = "87742422", Email = "jill@gmail.com", Address = "450 Toa Payoh Dr 6 Singapore 456450", JobTitle = "Sales Director", DepartmentName = "Sales", StartDate = new DateTime(2021, 01, 17) },
                new Employee { EmployeeId = 5, FirstName = "Tim", LastName = "Ong", Age = 67, PersonalID = "S1254432A", DateOfBirth = new DateTime(1955, 09, 11), MobileNo = "17422344", Email = "tim@gmail.com", Address = "117 Tampines St 11 Singapore 510117", JobTitle = "IT Manager", DepartmentName = "IT", StartDate = new DateTime(2021, 03, 27) }

            );
            modelBuilder.Entity<BankDetail>().HasData(
                new BankDetail { BankDetailId = 1, EmployeeId = 1, AccountNo = "123-45671-9", BranchCode = "123", BankName = "OCBC" },
                new BankDetail { BankDetailId = 2, EmployeeId = 1, AccountNo = "321-12391-9", BranchCode = "321", BankName = "DBS", AccountStatus = 1 },
                new BankDetail { BankDetailId = 3, EmployeeId = 2, AccountNo = "110-41136-4", BranchCode = "110", BankName = "UOB", AccountStatus = 1 },
                new BankDetail { BankDetailId = 4, EmployeeId = 2, AccountNo = "126-44673-6", BranchCode = "126", BankName = "DBS" },
                new BankDetail { BankDetailId = 5, EmployeeId = 3, AccountNo = "129-23626-3", BranchCode = "129", BankName = "POSB" },
                new BankDetail { BankDetailId = 6, EmployeeId = 4, AccountNo = "130-14677-9", BranchCode = "130", BankName = "DBS", AccountStatus = 1 },
                new BankDetail { BankDetailId = 7, EmployeeId = 4, AccountNo = "233-55632-8", BranchCode = "233", BankName = "CITI" },
                new BankDetail { BankDetailId = 8, EmployeeId = 5, AccountNo = "423-85638-3", BranchCode = "423", BankName = "DBS", AccountStatus = 1 },
                new BankDetail { BankDetailId = 9, EmployeeId = 5, AccountNo = "527-95475-7", BranchCode = "527", BankName = "MAYBANK" }
            );
            modelBuilder.Entity<Salary>().HasData(
               new Salary { SalaryId = 1, EmployeeId = 1, Month = "Jan", Year = "2022", BasicSalary = 6700, Bonus = 300, Deduction = 0, SalaryAmt = 7000, Remarks = "Promoted" },
               new Salary { SalaryId = 2, EmployeeId = 1, Month = "Dec", Year = "2021", BasicSalary = 3700, Bonus = 0, Deduction = 0, SalaryAmt = 3700, Remarks = "" },
               new Salary { SalaryId = 3, EmployeeId = 2, Month = "Jan", Year = "2022", BasicSalary = 5000, Bonus = 0, Deduction = 0, SalaryAmt = 5000, Remarks = "" },
               new Salary { SalaryId = 4, EmployeeId = 3, Month = "Jan", Year = "2022", BasicSalary = 8700, Bonus = 0, Deduction = 0, SalaryAmt = 8700, Remarks = "" },
               new Salary { SalaryId = 5, EmployeeId = 3, Month = "Dec", Year = "2021", BasicSalary = 5000, Bonus = 300, Deduction = 0, SalaryAmt = 5300, Remarks = "" },
               new Salary { SalaryId = 6, EmployeeId = 4, Month = "Jan", Year = "2022", BasicSalary = 3500, Bonus = 0, Deduction = 0, SalaryAmt = 3500, Remarks = "" },
               new Salary { SalaryId = 7, EmployeeId = 4, Month = "Dec", Year = "2021", BasicSalary = 4700, Bonus = 0, Deduction = 0, SalaryAmt = 4700, Remarks = "" },
               new Salary { SalaryId = 8, EmployeeId = 5, Month = "Jan", Year = "2022", BasicSalary = 4500, Bonus = 0, Deduction = 0, SalaryAmt = 4500, Remarks = "" },
               new Salary { SalaryId = 9, EmployeeId = 5, Month = "Dec", Year = "2021", BasicSalary = 6000, Bonus = 0, Deduction = 0, SalaryAmt = 6000, Remarks = "" },
               new Salary { SalaryId = 10, EmployeeId = 2, Month = "Dec", Year = "2021", BasicSalary = 6000, Bonus = 0, Deduction = 0, SalaryAmt = 6000, Remarks = "" }
           );

        }

        public string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            //using (var rngCrypt = new RNGCryptoServiceProvider())
            //{
            //    rngCrypt.GetNonZeroBytes(salt);
            //}            

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            ));

            return hashed;
        }
    }
}
