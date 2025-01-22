using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEMANTH.HOME_EXPENCE.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "tblexpence_details",
                schema: "dbo",
                columns: table => new
                {
                    expd_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    expd_expe_id = table.Column<int>(type: "int", nullable: false),
                    expd_us_id = table.Column<int>(type: "int", nullable: false),
                    expd_exp_price = table.Column<double>(type: "float", nullable: false),
                    expd_fam_id = table.Column<int>(type: "int", nullable: false),
                    expd_entry_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblexpence_details", x => x.expd_id);
                });

            migrationBuilder.CreateTable(
                name: "tblexpencecategory",
                schema: "dbo",
                columns: table => new
                {
                    exp_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    exp_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    exp_desc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblexpencecategory", x => x.exp_id);
                });

            migrationBuilder.CreateTable(
                name: "tblexpencemaster",
                schema: "dbo",
                columns: table => new
                {
                    expense_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    exp_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    e_price = table.Column<double>(type: "float", nullable: false),
                    e_catergory_id = table.Column<int>(type: "int", nullable: false),
                    e_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    e_us_id = table.Column<int>(type: "int", nullable: false),
                    e_fam_id = table.Column<int>(type: "int", nullable: false),
                    e_desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    e_entry_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblexpencemaster", x => x.expense_id);
                });

            migrationBuilder.CreateTable(
                name: "tblfamily",
                schema: "dbo",
                columns: table => new
                {
                    fam_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fam_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fam_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fam_entry_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fam_home_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fam_home_location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fam_home_photo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fam_electric_bill_number = table.Column<int>(type: "int", nullable: false),
                    fam_location_map = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fam_home_owner_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fam_home_owner_mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fam_door_no = table.Column<int>(type: "int", nullable: false),
                    fam_floor_no = table.Column<int>(type: "int", nullable: false),
                    fam_ref_key = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblfamily", x => x.fam_id);
                });

            migrationBuilder.CreateTable(
                name: "tbluser",
                schema: "dbo",
                columns: table => new
                {
                    us_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    us_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    us_pswd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    us_phone_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    us_email_add = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    us_is_admin = table.Column<int>(type: "int", nullable: false),
                    us_approve_status = table.Column<int>(type: "int", nullable: false),
                    us_family_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbluser", x => x.us_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblexpence_details",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblexpencecategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblexpencemaster",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblfamily",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tbluser",
                schema: "dbo");
        }
    }
}
