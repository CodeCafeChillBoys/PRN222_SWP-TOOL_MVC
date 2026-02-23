using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PRN222_SWP_TOOL_MVC.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddQnAModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_questions_StudentGroups_GroupID",
                table: "questions");

            migrationBuilder.DropForeignKey(
                name: "FK_questions_Students_StudentID",
                table: "questions");

            migrationBuilder.DropForeignKey(
                name: "FK_questions_Topics_TopicID",
                table: "questions");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicRegisters_StudentGroups_GroupID",
                table: "TopicRegisters");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicRegisters_Topics_TopicID",
                table: "TopicRegisters");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Teachers_TeacherID",
                table: "Topics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_questions",
                table: "questions");

            migrationBuilder.DropIndex(
                name: "IX_questions_GroupID",
                table: "questions");

            migrationBuilder.DropIndex(
                name: "IX_questions_TopicID",
                table: "questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicRegisters",
                table: "TopicRegisters");

            migrationBuilder.RenameTable(
                name: "questions",
                newName: "Questions");

            migrationBuilder.RenameTable(
                name: "TopicRegisters",
                newName: "TopicRegistrations");

            migrationBuilder.RenameIndex(
                name: "IX_questions_StudentID",
                table: "Questions",
                newName: "IX_Questions_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_TopicRegisters_TopicID",
                table: "TopicRegistrations",
                newName: "IX_TopicRegistrations_TopicID");

            migrationBuilder.RenameIndex(
                name: "IX_TopicRegisters_GroupID",
                table: "TopicRegistrations",
                newName: "IX_TopicRegistrations_GroupID");

            migrationBuilder.AddColumn<int>(
                name: "LastRepliedByUserId",
                table: "Questions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastReplyAt",
                table: "Questions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Questions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Questions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicRegistrations",
                table: "TopicRegistrations",
                column: "RegistrationID");

            migrationBuilder.CreateTable(
                name: "QuestionMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    SenderUserId = table.Column<int>(type: "integer", nullable: false),
                    SenderRole = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionMessages_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionMessages_Users_SenderUserId",
                        column: x => x.SenderUserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_GroupID_CreatedAt",
                table: "Questions",
                columns: new[] { "GroupID", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_LastRepliedByUserId",
                table: "Questions",
                column: "LastRepliedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TopicID_Status_LastReplyAt",
                table: "Questions",
                columns: new[] { "TopicID", "Status", "LastReplyAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_GroupID_StudentID",
                table: "GroupMembers",
                columns: new[] { "GroupID", "StudentID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionMessages_QuestionId",
                table: "QuestionMessages",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionMessages_SenderUserId",
                table: "QuestionMessages",
                column: "SenderUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_StudentGroups_GroupID",
                table: "Questions",
                column: "GroupID",
                principalTable: "StudentGroups",
                principalColumn: "GroupID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Students_StudentID",
                table: "Questions",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "StudentID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Topics_TopicID",
                table: "Questions",
                column: "TopicID",
                principalTable: "Topics",
                principalColumn: "TopicID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Users_LastRepliedByUserId",
                table: "Questions",
                column: "LastRepliedByUserId",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicRegistrations_StudentGroups_GroupID",
                table: "TopicRegistrations",
                column: "GroupID",
                principalTable: "StudentGroups",
                principalColumn: "GroupID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicRegistrations_Topics_TopicID",
                table: "TopicRegistrations",
                column: "TopicID",
                principalTable: "Topics",
                principalColumn: "TopicID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Teachers_TeacherID",
                table: "Topics",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_StudentGroups_GroupID",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Students_StudentID",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Topics_TopicID",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_LastRepliedByUserId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicRegistrations_StudentGroups_GroupID",
                table: "TopicRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicRegistrations_Topics_TopicID",
                table: "TopicRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Teachers_TeacherID",
                table: "Topics");

            migrationBuilder.DropTable(
                name: "QuestionMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_GroupID_CreatedAt",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_LastRepliedByUserId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TopicID_Status_LastReplyAt",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_GroupID_StudentID",
                table: "GroupMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicRegistrations",
                table: "TopicRegistrations");

            migrationBuilder.DropColumn(
                name: "LastRepliedByUserId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "LastReplyAt",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Questions");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "questions");

            migrationBuilder.RenameTable(
                name: "TopicRegistrations",
                newName: "TopicRegisters");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_StudentID",
                table: "questions",
                newName: "IX_questions_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_TopicRegistrations_TopicID",
                table: "TopicRegisters",
                newName: "IX_TopicRegisters_TopicID");

            migrationBuilder.RenameIndex(
                name: "IX_TopicRegistrations_GroupID",
                table: "TopicRegisters",
                newName: "IX_TopicRegisters_GroupID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_questions",
                table: "questions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicRegisters",
                table: "TopicRegisters",
                column: "RegistrationID");

            migrationBuilder.CreateIndex(
                name: "IX_questions_GroupID",
                table: "questions",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_questions_TopicID",
                table: "questions",
                column: "TopicID");

            migrationBuilder.AddForeignKey(
                name: "FK_questions_StudentGroups_GroupID",
                table: "questions",
                column: "GroupID",
                principalTable: "StudentGroups",
                principalColumn: "GroupID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_questions_Students_StudentID",
                table: "questions",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "StudentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_questions_Topics_TopicID",
                table: "questions",
                column: "TopicID",
                principalTable: "Topics",
                principalColumn: "TopicID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicRegisters_StudentGroups_GroupID",
                table: "TopicRegisters",
                column: "GroupID",
                principalTable: "StudentGroups",
                principalColumn: "GroupID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicRegisters_Topics_TopicID",
                table: "TopicRegisters",
                column: "TopicID",
                principalTable: "Topics",
                principalColumn: "TopicID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Teachers_TeacherID",
                table: "Topics",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
