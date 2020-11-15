using Microsoft.EntityFrameworkCore.Migrations;

namespace tpEstudiante.Migrations
{
    public partial class Estudiante_Legajo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "Legajo",
                table: "Estudiantes",
                type: "string",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Legajo",
                table: "Estudiantes");

   
        }
    }
}
