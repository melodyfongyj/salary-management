using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalaryManagement.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessLevel = table.Column<int>(type: "int", nullable: false),
                    PersonalID = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "Date", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DepartmentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(type: "Date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "Date", nullable: false),
                    EmpSuppDocs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true, defaultValue: "5kOIeXHcGKSjzvjntN7lbJDlMkXaf+onA0sCyvhKN4s=")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "BankDetail",
                columns: table => new
                {
                    BankDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNo = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    AccountStatus = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDetail", x => x.BankDetailId);
                    table.ForeignKey(
                        name: "FK_BankDetail_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Salary",
                columns: table => new
                {
                    SalaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Year = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "Date", nullable: false),
                    SalaryAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Bonus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salary", x => x.SalaryId);
                    table.ForeignKey(
                        name: "FK_Salary_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeId", "AccessLevel", "Address", "Age", "DateOfBirth", "DepartmentName", "Email", "EmpSuppDocs", "EndDate", "FirstName", "JobTitle", "LastName", "MobileNo", "PersonalID", "StartDate" },
                values: new object[,]
                {
                    { 1, 2, "123 Paya Lebar Dr 6 Singapore 123123", 28, new DateTime(1993, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "IS", "mich@gmail.com", null, new DateTime(2032, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), "Mich", "System Administrator", "Anderson", "12345678", "S1234567A", new DateTime(2021, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, "321 Paya Lebar Dr 6 Singapore 123321", 31, new DateTime(1990, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marketing", "trish@gmail.com", null, new DateTime(2032, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), "Trish", "Marketing Executive", "Ng", "87654321", "S7654321A", new DateTime(2022, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 0, "103 Toa Payoh Dr 6 Singapore 456103", 36, new DateTime(1986, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR", "jack@gmail.com", null, new DateTime(2032, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), "Jack", "HR Manager", "Lozano", "12457448", "S1265437A", new DateTime(2022, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, "450 Toa Payoh Dr 6 Singapore 456450", 62, new DateTime(1960, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sales", "jill@gmail.com", null, new DateTime(2032, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), "Jill", "Sales Director", "Oconnell", "87742422", "S7657656A", new DateTime(2021, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, "117 Tampines St 11 Singapore 510117", 67, new DateTime(1955, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "IT", "tim@gmail.com", null, new DateTime(2032, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), "Tim", "IT Manager", "Ong", "17422344", "S1254432A", new DateTime(2021, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "BankDetail",
                columns: new[] { "BankDetailId", "AccountNo", "AccountStatus", "BankName", "BranchCode", "EmployeeId" },
                values: new object[,]
                {
                    { 1, "123-45671-9", 0, "OCBC", "123", 1 },
                    { 2, "321-12391-9", 1, "DBS", "321", 1 },
                    { 9, "527-95475-7", 0, "MAYBANK", "527", 5 },
                    { 8, "423-85638-3", 1, "DBS", "423", 5 },
                    { 3, "110-41136-4", 1, "UOB", "110", 2 },
                    { 4, "126-44673-6", 0, "DBS", "126", 2 },
                    { 5, "129-23626-3", 0, "POSB", "129", 3 },
                    { 7, "233-55632-8", 0, "CITI", "233", 4 },
                    { 6, "130-14677-9", 1, "DBS", "130", 4 }
                });

            migrationBuilder.InsertData(
                table: "Salary",
                columns: new[] { "SalaryId", "BasicSalary", "Bonus", "Deduction", "EmployeeId", "InvoiceDate", "ModifiedBy", "Month", "Remarks", "SalaryAmt", "Year" },
                values: new object[,]
                {
                    { 7, 4700m, 0m, 0m, 4, new DateTime(2022, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), 0, "Dec", "", 4700m, "2021" },
                    { 6, 3500m, 0m, 0m, 4, new DateTime(2022, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), 0, "Jan", "", 3500m, "2022" },
                    { 4, 8700m, 0m, 0m, 3, new DateTime(2022, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), 0, "Jan", "", 8700m, "2022" },
                    { 8, 4500m, 0m, 0m, 5, new DateTime(2022, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), 0, "Jan", "", 4500m, "2022" },
                    { 10, 6000m, 0m, 0m, 2, new DateTime(2022, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), 0, "Dec", "", 6000m, "2021" },
                    { 3, 5000m, 0m, 0m, 2, new DateTime(2022, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), 0, "Jan", "", 5000m, "2022" },
                    { 2, 3700m, 0m, 0m, 1, new DateTime(2022, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), 0, "Dec", "", 3700m, "2021" },
                    { 1, 6700m, 300m, 0m, 1, new DateTime(2022, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), 0, "Jan", "Promoted", 7000m, "2022" },
                    { 5, 5000m, 300m, 0m, 3, new DateTime(2022, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), 0, "Dec", "", 5300m, "2021" },
                    { 9, 6000m, 0m, 0m, 5, new DateTime(2022, 1, 29, 0, 0, 0, 0, DateTimeKind.Local), 0, "Dec", "", 6000m, "2021" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankDetail_EmployeeId",
                table: "BankDetail",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Email",
                table: "Employee",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Salary_EmployeeId",
                table: "Salary",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankDetail");

            migrationBuilder.DropTable(
                name: "Salary");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
