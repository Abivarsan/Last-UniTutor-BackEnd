using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniTutor.Migrations
{
    /// <inheritdoc />
    public partial class menushhaaa333 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Students_studentId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Tutors_tutorId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_studentId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_tutorId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "studentId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "tutorId",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "senderMail",
                table: "Reports",
                newName: "reporterType");

            migrationBuilder.RenameColumn(
                name: "receiverMail",
                table: "Reports",
                newName: "reportedType");

            migrationBuilder.AddColumn<bool>(
                name: "isSuspended",
                table: "Tutors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isSuspended",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "reportedId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "reporterId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isSuspended",
                table: "Tutors");

            migrationBuilder.DropColumn(
                name: "isSuspended",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "reportedId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "reporterId",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "reporterType",
                table: "Reports",
                newName: "senderMail");

            migrationBuilder.RenameColumn(
                name: "reportedType",
                table: "Reports",
                newName: "receiverMail");

            migrationBuilder.AddColumn<int>(
                name: "studentId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tutorId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_studentId",
                table: "Reports",
                column: "studentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_tutorId",
                table: "Reports",
                column: "tutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Students_studentId",
                table: "Reports",
                column: "studentId",
                principalTable: "Students",
                principalColumn: "_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Tutors_tutorId",
                table: "Reports",
                column: "tutorId",
                principalTable: "Tutors",
                principalColumn: "_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
