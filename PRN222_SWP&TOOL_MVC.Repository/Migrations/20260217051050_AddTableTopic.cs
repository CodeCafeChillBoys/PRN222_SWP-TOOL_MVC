using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PRN222_SWP_TOOL_MVC.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddTableTopic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    TopicID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TopicName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Requirement = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SemesterID = table.Column<int>(type: "integer", nullable: false),
                    TeacherID = table.Column<int>(type: "integer", nullable: false),
                    MaxGroupCount = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.TopicID);
                    table.ForeignKey(
                        name: "FK_Topics_Semesters_SemesterID",
                        column: x => x.SemesterID,
                        principalTable: "Semesters",
                        principalColumn: "SemesterID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Topics_Teachers_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "Teachers",
                        principalColumn: "TeacherID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopicRegisters",
                columns: table => new
                {
                    RegistrationID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TopicID = table.Column<int>(type: "integer", nullable: false),
                    GroupID = table.Column<int>(type: "integer", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicRegisters", x => x.RegistrationID);
                    table.ForeignKey(
                        name: "FK_TopicRegisters_StudentGroups_GroupID",
                        column: x => x.GroupID,
                        principalTable: "StudentGroups",
                        principalColumn: "GroupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopicRegisters_Topics_TopicID",
                        column: x => x.TopicID,
                        principalTable: "Topics",
                        principalColumn: "TopicID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopicRegisters_GroupID",
                table: "TopicRegisters",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_TopicRegisters_TopicID",
                table: "TopicRegisters",
                column: "TopicID");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_SemesterID",
                table: "Topics",
                column: "SemesterID");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_TeacherID",
                table: "Topics",
                column: "TeacherID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopicRegisters");

            migrationBuilder.DropTable(
                name: "Topics");
        }
    }
}
