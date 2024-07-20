using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmotionDetectionSystem.Migrations
{
    /// <inheritdoc />
    public partial class Initcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    UserType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Lesson",
                columns: table => new
                {
                    LessonId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    LessonName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    TeacherId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    EntryCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Tags = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lesson", x => x.LessonId);
                    table.ForeignKey(
                        name: "FK_Lesson_User_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "User",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EnrollmentSummary",
                columns: table => new
                {
                    StudentId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LessonId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TeacherApproach = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentSummary", x => new { x.LessonId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_EnrollmentSummary_Lesson_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lesson",
                        principalColumn: "LessonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnrollmentSummary_User_StudentId",
                        column: x => x.StudentId,
                        principalTable: "User",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmotionData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Time = table.Column<DateTime>(type: "TEXT", maxLength: 100, nullable: false),
                    WinningEmotion = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Seen = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnrollmentSummaryLessonId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    EnrollmentSummaryStudentId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmotionData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmotionData_EnrollmentSummary_EnrollmentSummaryLessonId_EnrollmentSummaryStudentId",
                        columns: x => new { x.EnrollmentSummaryLessonId, x.EnrollmentSummaryStudentId },
                        principalTable: "EnrollmentSummary",
                        principalColumns: new[] { "LessonId", "StudentId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmotionData_EnrollmentSummaryLessonId_EnrollmentSummaryStudentId",
                table: "EmotionData",
                columns: new[] { "EnrollmentSummaryLessonId", "EnrollmentSummaryStudentId" });

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentSummary_StudentId",
                table: "EnrollmentSummary",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_TeacherId",
                table: "Lesson",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmotionData");

            migrationBuilder.DropTable(
                name: "EnrollmentSummary");

            migrationBuilder.DropTable(
                name: "Lesson");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
