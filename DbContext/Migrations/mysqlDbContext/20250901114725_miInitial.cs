using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContext.Migrations.mysqlDbContext
{
    /// <inheritdoc />
    public partial class miInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreditCards",
                columns: table => new
                {
                    CreditCardId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Issuer = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpirationYear = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpirationMonth = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CardHolderName = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EncryptedToken = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Seeded = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCards", x => x.CreditCardId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditCards");
        }
    }
}
