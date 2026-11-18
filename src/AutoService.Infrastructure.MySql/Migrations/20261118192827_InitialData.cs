using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AutoService.Infrastructure.MySql.Migrations;

/// <inheritdoc />
public partial class InitialData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Clients",
            columns: new[] { "Id", "FullName", "Phone" },
            values: new object[,]
            {
                { 1, "Соколов Иван Сергеевич", "+79000000001" },
                { 2, "Андреев Павел Игоревич", "+79000000002" },
                { 3, "Козлов Алексей Олегович", "+79000000003" },
                { 4, "Белова Анна Викторовна", "+79000000004" },
                { 5, "Яковлев Дмитрий Андреевич", "+79000000005" },
                { 6, "Васильева Мария Алексеевна", "+79000000006" },
                { 7, "Дмитриев Олег Павлович", "+79000000007" },
                { 8, "Иванова Елена Сергеевна", "+79000000008" },
                { 9, "Петров Сергей Николаевич", "+79000000009" },
                { 10, "Смирнова Ольга Ильинична", "+79000000010" }
            });

        migrationBuilder.InsertData(
            table: "Mechanics",
            columns: new[] { "Id", "ExperienceYears", "FullName", "PassportNumber", "Specialization" },
            values: new object[,]
            {
                { 1, 12, "Орлов Андрей Петрович", "0000 000001", 1 },
                { 2, 5, "Громов Виктор Сергеевич", "0000 000002", 1 },
                { 3, 15, "Зайцев Николай Иванович", "0000 000003", 2 },
                { 4, 7, "Егоров Михаил Олегович", "0000 000004", 2 },
                { 5, 10, "Крылов Денис Андреевич", "0000 000005", 3 },
                { 6, 4, "Фролов Артём Павлович", "0000 000006", 3 },
                { 7, 8, "Лебедев Роман Ильич", "0000 000007", 4 },
                { 8, 3, "Волков Игорь Викторович", "0000 000008", 4 },
                { 9, 11, "Морозов Алексей Юрьевич", "0000 000009", 5 },
                { 10, 6, "Новиков Павел Борисович", "0000 000010", 5 }
            });

        migrationBuilder.InsertData(
            table: "WorkTypes",
            columns: new[] { "Id", "Category", "Cost", "Duration", "Name" },
            values: new object[,]
            {
                { 1, 1, 1200.50m, 24000000000L, "Замена моторного масла" },
                { 2, 1, 400.25m, 9000000000L, "Замена масляного фильтра" },
                { 3, 2, 3500m, 36000000000L, "Диагностика двигателя" },
                { 4, 2, 1800m, 27000000000L, "Замена свечей зажигания" },
                { 5, 3, 2500m, 36000000000L, "Замена масла в коробке передач" },
                { 6, 3, 6000m, 108000000000L, "Замена сцепления" },
                { 7, 4, 900m, 18000000000L, "Диагностика электрооборудования" },
                { 8, 4, 1400m, 36000000000L, "Замена генератора" },
                { 9, 5, 7000m, 108000000000L, "Ремонт крыла" },
                { 10, 5, 5000m, 72000000000L, "Покраска бампера" },
                { 11, 6, 2000m, 27000000000L, "Шиномонтаж" },
                { 12, 6, 1500m, 18000000000L, "Балансировка колёс" }
            });

        migrationBuilder.InsertData(
            table: "Cars",
            columns: new[] { "Id", "Brand", "ManufactureYear", "Model", "OwnerId", "RegistrationNumber" },
            values: new object[,]
            {
                { 1, "Lada", 2020, "Vesta", 1, "А101АА163" },
                { 2, "Toyota", 2018, "Corolla", 2, "В202ВВ163" },
                { 3, "Kia", 2019, "Rio", 3, "Е303ЕЕ163" },
                { 4, "Hyundai", 2021, "Solaris", 4, "К404КК163" },
                { 5, "Renault", 2017, "Logan", 5, "М505ММ163" },
                { 6, "Skoda", 2022, "Octavia", 6, "Н606НН163" },
                { 7, "Volkswagen", 2016, "Polo", 7, "О707ОО163" },
                { 8, "Nissan", 2023, "Qashqai", 8, "Р808РР163" },
                { 9, "Ford", 2015, "Focus", 9, "С909СС163" },
                { 10, "Mazda", 2020, "3", 10, "Т010ТТ163" },
                { 11, "Lada", 2024, "Granta", 1, "У111УУ163" },
                { 12, "Toyota", 2021, "Camry", 2, "Х212ХХ163" }
            });

        migrationBuilder.InsertData(
            table: "Orders",
            columns: new[] { "Id", "AcceptedAt", "CarId", "ClientId", "MechanicId", "ReturnedAt" },
            values: new object[,]
            {
                { 1, new DateTime(2026, 6, 1, 12, 0, 0, 0, DateTimeKind.Utc), 1, 1, 1, new DateTime(2026, 6, 1, 12, 55, 0, 0, DateTimeKind.Utc) },
                { 2, new DateTime(2026, 8, 1, 12, 0, 0, 0, DateTimeKind.Utc), 2, 2, 3, new DateTime(2026, 8, 1, 13, 45, 0, 0, DateTimeKind.Utc) },
                { 3, new DateTime(2026, 8, 26, 12, 0, 0, 0, DateTimeKind.Utc), 3, 3, 5, new DateTime(2026, 8, 26, 13, 0, 0, 0, DateTimeKind.Utc) },
                { 4, new DateTime(2026, 8, 27, 11, 59, 59, 0, DateTimeKind.Utc), 4, 4, 7, new DateTime(2026, 8, 27, 12, 29, 59, 0, DateTimeKind.Utc) },
                { 5, new DateTime(2026, 8, 27, 12, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, new DateTime(2026, 8, 27, 12, 40, 0, 0, DateTimeKind.Utc) },
                { 6, new DateTime(2026, 8, 27, 11, 59, 59, 0, DateTimeKind.Utc), 2, 2, 4, new DateTime(2026, 8, 27, 12, 59, 59, 0, DateTimeKind.Utc) },
                { 7, new DateTime(2026, 9, 1, 12, 0, 0, 0, DateTimeKind.Utc), 11, 1, 1, new DateTime(2026, 9, 1, 12, 55, 0, 0, DateTimeKind.Utc) },
                { 8, new DateTime(2026, 9, 2, 12, 0, 0, 0, DateTimeKind.Utc), 2, 2, 3, new DateTime(2026, 9, 2, 13, 45, 0, 0, DateTimeKind.Utc) },
                { 9, new DateTime(2026, 9, 3, 12, 0, 0, 0, DateTimeKind.Utc), 3, 3, 6, new DateTime(2026, 9, 3, 16, 0, 0, 0, DateTimeKind.Utc) },
                { 10, new DateTime(2026, 9, 4, 12, 0, 0, 0, DateTimeKind.Utc), 4, 4, 8, new DateTime(2026, 9, 4, 13, 30, 0, 0, DateTimeKind.Utc) },
                { 11, new DateTime(2026, 9, 5, 12, 0, 0, 0, DateTimeKind.Utc), 5, 5, 9, new DateTime(2026, 9, 5, 15, 0, 0, 0, DateTimeKind.Utc) },
                { 12, new DateTime(2026, 9, 6, 12, 0, 0, 0, DateTimeKind.Utc), 6, 6, 1, new DateTime(2026, 9, 6, 12, 55, 0, 0, DateTimeKind.Utc) },
                { 13, new DateTime(2026, 9, 7, 12, 0, 0, 0, DateTimeKind.Utc), 7, 7, 3, new DateTime(2026, 9, 7, 13, 0, 0, 0, DateTimeKind.Utc) },
                { 14, new DateTime(2026, 9, 8, 12, 0, 0, 0, DateTimeKind.Utc), 8, 8, 5, new DateTime(2026, 9, 8, 13, 0, 0, 0, DateTimeKind.Utc) },
                { 15, new DateTime(2026, 9, 9, 12, 0, 0, 0, DateTimeKind.Utc), 9, 9, 7, new DateTime(2026, 9, 9, 12, 30, 0, 0, DateTimeKind.Utc) },
                { 16, new DateTime(2026, 9, 10, 12, 0, 0, 0, DateTimeKind.Utc), 10, 10, 9, new DateTime(2026, 9, 10, 17, 0, 0, 0, DateTimeKind.Utc) },
                { 17, new DateTime(2026, 9, 11, 12, 0, 0, 0, DateTimeKind.Utc), 1, 1, 1, new DateTime(2026, 9, 11, 12, 55, 0, 0, DateTimeKind.Utc) },
                { 18, new DateTime(2026, 9, 12, 12, 0, 0, 0, DateTimeKind.Utc), 2, 2, 4, new DateTime(2026, 9, 12, 13, 0, 0, 0, DateTimeKind.Utc) },
                { 19, new DateTime(2026, 9, 13, 12, 0, 0, 0, DateTimeKind.Utc), 12, 2, 3, new DateTime(2026, 9, 13, 13, 45, 0, 0, DateTimeKind.Utc) },
                { 20, new DateTime(2026, 9, 14, 12, 0, 0, 0, DateTimeKind.Utc), 3, 3, 5, new DateTime(2026, 9, 14, 13, 0, 0, 0, DateTimeKind.Utc) },
                { 21, new DateTime(2026, 9, 15, 12, 0, 0, 0, DateTimeKind.Utc), 4, 4, 7, new DateTime(2026, 9, 15, 12, 30, 0, 0, DateTimeKind.Utc) },
                { 22, new DateTime(2026, 9, 16, 12, 0, 0, 0, DateTimeKind.Utc), 5, 5, 10, new DateTime(2026, 9, 16, 15, 0, 0, 0, DateTimeKind.Utc) },
                { 23, new DateTime(2026, 9, 17, 12, 0, 0, 0, DateTimeKind.Utc), 6, 6, 2, new DateTime(2026, 9, 17, 12, 40, 0, 0, DateTimeKind.Utc) },
                { 24, new DateTime(2026, 9, 18, 12, 0, 0, 0, DateTimeKind.Utc), 7, 7, 4, new DateTime(2026, 9, 18, 13, 45, 0, 0, DateTimeKind.Utc) },
                { 25, new DateTime(2026, 9, 19, 12, 0, 0, 0, DateTimeKind.Utc), 8, 8, 6, new DateTime(2026, 9, 19, 16, 0, 0, 0, DateTimeKind.Utc) },
                { 26, new DateTime(2026, 9, 27, 11, 59, 59, 0, DateTimeKind.Utc), 1, 1, 1, null },
                { 27, new DateTime(2026, 9, 27, 12, 0, 0, 0, DateTimeKind.Utc), 2, 2, 3, null },
                { 28, new DateTime(2026, 9, 28, 12, 0, 0, 0, DateTimeKind.Utc), 3, 3, 5, null }
            });

        migrationBuilder.InsertData(
            table: "OrderWorks",
            columns: new[] { "OrderId", "WorkTypeId" },
            values: new object[,]
            {
                { 1, 1 },
                { 1, 2 },
                { 2, 3 },
                { 2, 4 },
                { 3, 5 },
                { 4, 7 },
                { 5, 1 },
                { 6, 3 },
                { 7, 1 },
                { 7, 2 },
                { 8, 3 },
                { 8, 4 },
                { 9, 5 },
                { 9, 6 },
                { 10, 7 },
                { 10, 8 },
                { 11, 9 },
                { 12, 1 },
                { 12, 2 },
                { 13, 3 },
                { 14, 5 },
                { 15, 7 },
                { 16, 9 },
                { 16, 10 },
                { 17, 1 },
                { 17, 2 },
                { 18, 3 },
                { 19, 3 },
                { 19, 4 },
                { 20, 5 },
                { 21, 7 },
                { 22, 9 },
                { 23, 1 },
                { 24, 3 },
                { 24, 4 },
                { 25, 5 },
                { 25, 6 },
                { 26, 1 },
                { 27, 3 },
                { 28, 5 }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 1, 1 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 1, 2 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 2, 3 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 2, 4 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 3, 5 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 4, 7 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 5, 1 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 6, 3 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 7, 1 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 7, 2 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 8, 3 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 8, 4 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 9, 5 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 9, 6 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 10, 7 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 10, 8 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 11, 9 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 12, 1 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 12, 2 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 13, 3 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 14, 5 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 15, 7 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 16, 9 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 16, 10 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 17, 1 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 17, 2 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 18, 3 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 19, 3 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 19, 4 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 20, 5 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 21, 7 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 22, 9 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 23, 1 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 24, 3 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 24, 4 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 25, 5 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 25, 6 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 26, 1 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 27, 3 });

        migrationBuilder.DeleteData(
            table: "OrderWorks",
            keyColumns: new[] { "OrderId", "WorkTypeId" },
            keyValues: new object[] { 28, 5 });

        migrationBuilder.DeleteData(
            table: "WorkTypes",
            keyColumn: "Id",
            keyValue: 11);

        migrationBuilder.DeleteData(
            table: "WorkTypes",
            keyColumn: "Id",
            keyValue: 12);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 3);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 5);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 7);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 8);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 9);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 10);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 11);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 12);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 13);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 14);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 15);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 16);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 17);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 18);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 19);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 20);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 21);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 22);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 23);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 24);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 25);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 26);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 27);

        migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "Id",
            keyValue: 28);

        migrationBuilder.DeleteData(
            table: "WorkTypes",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "WorkTypes",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "WorkTypes",
            keyColumn: "Id",
            keyValue: 3);

        migrationBuilder.DeleteData(
            table: "WorkTypes",
            keyColumn: "Id",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "WorkTypes",
            keyColumn: "Id",
            keyValue: 5);

        migrationBuilder.DeleteData(
            table: "WorkTypes",
            keyColumn: "Id",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "WorkTypes",
            keyColumn: "Id",
            keyValue: 7);

        migrationBuilder.DeleteData(
            table: "WorkTypes",
            keyColumn: "Id",
            keyValue: 8);

        migrationBuilder.DeleteData(
            table: "WorkTypes",
            keyColumn: "Id",
            keyValue: 9);

        migrationBuilder.DeleteData(
            table: "WorkTypes",
            keyColumn: "Id",
            keyValue: 10);

        migrationBuilder.DeleteData(
            table: "Cars",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "Cars",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "Cars",
            keyColumn: "Id",
            keyValue: 3);

        migrationBuilder.DeleteData(
            table: "Cars",
            keyColumn: "Id",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "Cars",
            keyColumn: "Id",
            keyValue: 5);

        migrationBuilder.DeleteData(
            table: "Cars",
            keyColumn: "Id",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "Cars",
            keyColumn: "Id",
            keyValue: 7);

        migrationBuilder.DeleteData(
            table: "Cars",
            keyColumn: "Id",
            keyValue: 8);

        migrationBuilder.DeleteData(
            table: "Cars",
            keyColumn: "Id",
            keyValue: 9);

        migrationBuilder.DeleteData(
            table: "Cars",
            keyColumn: "Id",
            keyValue: 10);

        migrationBuilder.DeleteData(
            table: "Cars",
            keyColumn: "Id",
            keyValue: 11);

        migrationBuilder.DeleteData(
            table: "Cars",
            keyColumn: "Id",
            keyValue: 12);

        migrationBuilder.DeleteData(
            table: "Mechanics",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "Mechanics",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "Mechanics",
            keyColumn: "Id",
            keyValue: 3);

        migrationBuilder.DeleteData(
            table: "Mechanics",
            keyColumn: "Id",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "Mechanics",
            keyColumn: "Id",
            keyValue: 5);

        migrationBuilder.DeleteData(
            table: "Mechanics",
            keyColumn: "Id",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "Mechanics",
            keyColumn: "Id",
            keyValue: 7);

        migrationBuilder.DeleteData(
            table: "Mechanics",
            keyColumn: "Id",
            keyValue: 8);

        migrationBuilder.DeleteData(
            table: "Mechanics",
            keyColumn: "Id",
            keyValue: 9);

        migrationBuilder.DeleteData(
            table: "Mechanics",
            keyColumn: "Id",
            keyValue: 10);

        migrationBuilder.DeleteData(
            table: "Clients",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "Clients",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "Clients",
            keyColumn: "Id",
            keyValue: 3);

        migrationBuilder.DeleteData(
            table: "Clients",
            keyColumn: "Id",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "Clients",
            keyColumn: "Id",
            keyValue: 5);

        migrationBuilder.DeleteData(
            table: "Clients",
            keyColumn: "Id",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "Clients",
            keyColumn: "Id",
            keyValue: 7);

        migrationBuilder.DeleteData(
            table: "Clients",
            keyColumn: "Id",
            keyValue: 8);

        migrationBuilder.DeleteData(
            table: "Clients",
            keyColumn: "Id",
            keyValue: 9);

        migrationBuilder.DeleteData(
            table: "Clients",
            keyColumn: "Id",
            keyValue: 10);
    }
}
