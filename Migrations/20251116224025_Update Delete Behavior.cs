using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practice_On_EFCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Students_StdId",
                table: "Courses");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Students_StdId",
                table: "Courses",
                column: "StdId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Students_StdId",
                table: "Courses");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Students_StdId",
                table: "Courses",
                column: "StdId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
