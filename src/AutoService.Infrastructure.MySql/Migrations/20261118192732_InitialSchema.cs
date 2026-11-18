using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace AutoService.Infrastructure.MySql.Migrations;

/// <inheritdoc />
public partial class InitialSchema : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "Clients",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                Phone = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                FullName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clients", x => x.Id);
            })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "Mechanics",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                PassportNumber = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
                Specialization = table.Column<int>(type: "int", nullable: false),
                ExperienceYears = table.Column<int>(type: "int", nullable: false),
                FullName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Mechanics", x => x.Id);
                table.CheckConstraint("CK_Mechanics_ExperienceYears", "`ExperienceYears` BETWEEN 0 AND 80");
                table.CheckConstraint("CK_Mechanics_Specialization", "`Specialization` BETWEEN 1 AND 6");
            })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "WorkTypes",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                Category = table.Column<int>(type: "int", nullable: false),
                Cost = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                Duration = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WorkTypes", x => x.Id);
                table.CheckConstraint("CK_WorkTypes_Category", "`Category` BETWEEN 1 AND 6");
                table.CheckConstraint("CK_WorkTypes_Cost", "`Cost` > 0 AND `Cost` <= 1000000000");
                table.CheckConstraint("CK_WorkTypes_Duration", "`Duration` BETWEEN 600000000 AND 315360000000000");
            })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "Cars",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                RegistrationNumber = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                Brand = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                Model = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                ManufactureYear = table.Column<int>(type: "int", nullable: false),
                OwnerId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Cars", x => x.Id);
                table.CheckConstraint("CK_Cars_ManufactureYear", "`ManufactureYear` BETWEEN 1886 AND 9999");
                table.ForeignKey(
                    name: "FK_Cars_Clients_OwnerId",
                    column: x => x.OwnerId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "Orders",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                CarId = table.Column<int>(type: "int", nullable: false),
                ClientId = table.Column<int>(type: "int", nullable: false),
                MechanicId = table.Column<int>(type: "int", nullable: false),
                AcceptedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                ReturnedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Orders", x => x.Id);
                table.CheckConstraint("CK_Orders_Dates", "`ReturnedAt` IS NULL OR `ReturnedAt` >= `AcceptedAt`");
                table.ForeignKey(
                    name: "FK_Orders_Cars_CarId",
                    column: x => x.CarId,
                    principalTable: "Cars",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Orders_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Orders_Mechanics_MechanicId",
                    column: x => x.MechanicId,
                    principalTable: "Mechanics",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "OrderWorks",
            columns: table => new
            {
                OrderId = table.Column<int>(type: "int", nullable: false),
                WorkTypeId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OrderWorks", x => new { x.OrderId, x.WorkTypeId });
                table.ForeignKey(
                    name: "FK_OrderWorks_Orders_OrderId",
                    column: x => x.OrderId,
                    principalTable: "Orders",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_OrderWorks_WorkTypes_WorkTypeId",
                    column: x => x.WorkTypeId,
                    principalTable: "WorkTypes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateIndex(
            name: "IX_Cars_OwnerId",
            table: "Cars",
            column: "OwnerId");

        migrationBuilder.CreateIndex(
            name: "IX_Cars_RegistrationNumber",
            table: "Cars",
            column: "RegistrationNumber",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Mechanics_PassportNumber",
            table: "Mechanics",
            column: "PassportNumber",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Orders_CarId",
            table: "Orders",
            column: "CarId");

        migrationBuilder.CreateIndex(
            name: "IX_Orders_ClientId_AcceptedAt_Id",
            table: "Orders",
            columns: new[] { "ClientId", "AcceptedAt", "Id" });

        migrationBuilder.CreateIndex(
            name: "IX_Orders_MechanicId",
            table: "Orders",
            column: "MechanicId");

        migrationBuilder.CreateIndex(
            name: "IX_OrderWorks_WorkTypeId",
            table: "OrderWorks",
            column: "WorkTypeId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "OrderWorks");

        migrationBuilder.DropTable(
            name: "Orders");

        migrationBuilder.DropTable(
            name: "WorkTypes");

        migrationBuilder.DropTable(
            name: "Cars");

        migrationBuilder.DropTable(
            name: "Mechanics");

        migrationBuilder.DropTable(
            name: "Clients");
    }
}
