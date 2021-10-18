using Microsoft.EntityFrameworkCore.Migrations;

namespace iTransitionCourse.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Task_TaskId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TaskId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaskId",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TaskId",
                table: "AspNetUsers",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Task_TaskId",
                table: "AspNetUsers",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
