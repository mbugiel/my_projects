using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AuthenticationAPI.Migrations
{
    /// <inheritdoc />
    public partial class migracja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "confirmation_codes",
                columns: table => new
                {
                    Email = table.Column<byte[]>(type: "bytea", nullable: false),
                    ConfirmationCode = table.Column<byte[]>(type: "bytea", nullable: false),
                    CodeLifetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Attempts = table.Column<int>(type: "integer", nullable: false),
                    Delay = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_confirmation_codes", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "session_tokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    SessionToken = table.Column<byte[]>(type: "bytea", nullable: false),
                    SessionLifetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session_tokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<byte[]>(type: "bytea", nullable: false),
                    UsernameNormalized = table.Column<byte[]>(type: "bytea", nullable: false),
                    Email = table.Column<byte[]>(type: "bytea", nullable: false),
                    Password = table.Column<byte[]>(type: "bytea", nullable: false),
                    TwoStepLogin = table.Column<bool>(type: "boolean", nullable: false),
                    Key = table.Column<byte[]>(type: "bytea", nullable: false),
                    Iv = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "confirmation_codes");

            migrationBuilder.DropTable(
                name: "session_tokens");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
