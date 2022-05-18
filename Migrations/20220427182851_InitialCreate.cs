using Microsoft.EntityFrameworkCore.Migrations;

namespace RegistrationAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: false),
                    MiddleName = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: false),
                    EmailAddress = table.Column<string>(type: "varchar(50)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobilePhone = table.Column<string>(type: "varchar(20)", nullable: false),
                    Last4DigitsSSN = table.Column<string>(type: "varchar(20)", nullable: false),
                    TermsandConditions = table.Column<bool>(type: "bit", nullable: false),
                    MaxLoginAttempt = table.Column<int>(type: "int", nullable: false),
                    LastLogin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnrolledDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.UniqueConstraint("AK_UQ_Email", x => x.EmailAddress); // <-- Add unique constraint
                });

            migrationBuilder.CreateTable(
                name: "tbl_employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "varchar(50)", nullable: false),
                    Salary = table.Column<int>(type: "float", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeID);
                    table.UniqueConstraint("AK_UQ_User", x => x.UserID); // <-- Add unique constraint
                    table.ForeignKey(
                        name: "FK_tbl_employees_tbl_users_UserID",
                        column: x => x.UserID,
                        principalTable: "tbl_users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);

                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_employee");
            migrationBuilder.DropTable(
                name: "tbl_users");
        }
    }
}
