using AutoService.Domain.Entities;
using AutoService.Domain.Enums;

namespace AutoService.Tests.Fixtures;

/// <summary>
/// Фиксированные учебные данные для LINQ-запросов. Все персональные сведения вымышлены.
/// </summary>
public class AutoServiceFixture
{
    /// <summary>Клиенты автосервиса.</summary>
    public IReadOnlyList<Client> Clients { get; }

    /// <summary>Автомобили; у клиентов 1 и 2 по два автомобиля.</summary>
    public IReadOnlyList<Car> Cars { get; }

    /// <summary>Механики; специалистов по шиномонтажу в наборе нет.</summary>
    public IReadOnlyList<Mechanic> Mechanics { get; }

    /// <summary>Прейскурант, включая два пока не заказанных вида работ.</summary>
    public IReadOnlyList<WorkType> WorkTypes { get; }

    /// <summary>История заказов с повторными визитами и границами отчётного периода.</summary>
    public IReadOnlyList<Order> Orders { get; }

    /// <summary>Создаёт связанный набор данных один раз для тестового класса.</summary>
    public AutoServiceFixture()
    {
        Clients =
        [
            new() { Id = 1, FullName = "Соколов Иван Сергеевич", Phone = "+79000000001" },
            new() { Id = 2, FullName = "Андреев Павел Игоревич", Phone = "+79000000002" },
            new() { Id = 3, FullName = "Козлов Алексей Олегович", Phone = "+79000000003" },
            new() { Id = 4, FullName = "Белова Анна Викторовна", Phone = "+79000000004" },
            new() { Id = 5, FullName = "Яковлев Дмитрий Андреевич", Phone = "+79000000005" },
            new() { Id = 6, FullName = "Васильева Мария Алексеевна", Phone = "+79000000006" },
            new() { Id = 7, FullName = "Дмитриев Олег Павлович", Phone = "+79000000007" },
            new() { Id = 8, FullName = "Иванова Елена Сергеевна", Phone = "+79000000008" },
            new() { Id = 9, FullName = "Петров Сергей Николаевич", Phone = "+79000000009" },
            new() { Id = 10, FullName = "Смирнова Ольга Ильинична", Phone = "+79000000010" }
        ];

        Cars =
        [
            new()
            {
                Id = 1,
                RegistrationNumber = "А101АА163",
                Brand = "Lada",
                Model = "Vesta",
                ManufactureYear = 2020,
                Owner = Clients[0]
            },
            new()
            {
                Id = 2,
                RegistrationNumber = "В202ВВ163",
                Brand = "Toyota",
                Model = "Corolla",
                ManufactureYear = 2018,
                Owner = Clients[1]
            },
            new()
            {
                Id = 3,
                RegistrationNumber = "Е303ЕЕ163",
                Brand = "Kia",
                Model = "Rio",
                ManufactureYear = 2019,
                Owner = Clients[2]
            },
            new()
            {
                Id = 4,
                RegistrationNumber = "К404КК163",
                Brand = "Hyundai",
                Model = "Solaris",
                ManufactureYear = 2021,
                Owner = Clients[3]
            },
            new()
            {
                Id = 5,
                RegistrationNumber = "М505ММ163",
                Brand = "Renault",
                Model = "Logan",
                ManufactureYear = 2017,
                Owner = Clients[4]
            },
            new()
            {
                Id = 6,
                RegistrationNumber = "Н606НН163",
                Brand = "Skoda",
                Model = "Octavia",
                ManufactureYear = 2022,
                Owner = Clients[5]
            },
            new()
            {
                Id = 7,
                RegistrationNumber = "О707ОО163",
                Brand = "Volkswagen",
                Model = "Polo",
                ManufactureYear = 2016,
                Owner = Clients[6]
            },
            new()
            {
                Id = 8,
                RegistrationNumber = "Р808РР163",
                Brand = "Nissan",
                Model = "Qashqai",
                ManufactureYear = 2023,
                Owner = Clients[7]
            },
            new()
            {
                Id = 9,
                RegistrationNumber = "С909СС163",
                Brand = "Ford",
                Model = "Focus",
                ManufactureYear = 2015,
                Owner = Clients[8]
            },
            new()
            {
                Id = 10,
                RegistrationNumber = "Т010ТТ163",
                Brand = "Mazda",
                Model = "3",
                ManufactureYear = 2020,
                Owner = Clients[9]
            },
            new()
            {
                Id = 11,
                RegistrationNumber = "У111УУ163",
                Brand = "Lada",
                Model = "Granta",
                ManufactureYear = 2024,
                Owner = Clients[0]
            },
            new()
            {
                Id = 12,
                RegistrationNumber = "Х212ХХ163",
                Brand = "Toyota",
                Model = "Camry",
                ManufactureYear = 2021,
                Owner = Clients[1]
            }
        ];

        Mechanics =
        [
            new()
            {
                Id = 1,
                FullName = "Орлов Андрей Петрович",
                PassportNumber = "0000 000001",
                Specialization = Specialization.Maintenance,
                ExperienceYears = 12
            },
            new()
            {
                Id = 2,
                FullName = "Громов Виктор Сергеевич",
                PassportNumber = "0000 000002",
                Specialization = Specialization.Maintenance,
                ExperienceYears = 5
            },
            new()
            {
                Id = 3,
                FullName = "Зайцев Николай Иванович",
                PassportNumber = "0000 000003",
                Specialization = Specialization.Engine,
                ExperienceYears = 15
            },
            new()
            {
                Id = 4,
                FullName = "Егоров Михаил Олегович",
                PassportNumber = "0000 000004",
                Specialization = Specialization.Engine,
                ExperienceYears = 7
            },
            new()
            {
                Id = 5,
                FullName = "Крылов Денис Андреевич",
                PassportNumber = "0000 000005",
                Specialization = Specialization.Transmission,
                ExperienceYears = 10
            },
            new()
            {
                Id = 6,
                FullName = "Фролов Артём Павлович",
                PassportNumber = "0000 000006",
                Specialization = Specialization.Transmission,
                ExperienceYears = 4
            },
            new()
            {
                Id = 7,
                FullName = "Лебедев Роман Ильич",
                PassportNumber = "0000 000007",
                Specialization = Specialization.Electrical,
                ExperienceYears = 8
            },
            new()
            {
                Id = 8,
                FullName = "Волков Игорь Викторович",
                PassportNumber = "0000 000008",
                Specialization = Specialization.Electrical,
                ExperienceYears = 3
            },
            new()
            {
                Id = 9,
                FullName = "Морозов Алексей Юрьевич",
                PassportNumber = "0000 000009",
                Specialization = Specialization.Body,
                ExperienceYears = 11
            },
            new()
            {
                Id = 10,
                FullName = "Новиков Павел Борисович",
                PassportNumber = "0000 000010",
                Specialization = Specialization.Body,
                ExperienceYears = 6
            }
        ];

        WorkTypes =
        [
            new()
            {
                Id = 1,
                Name = "Замена моторного масла",
                Category = Specialization.Maintenance,
                Cost = 1200.50m,
                Duration = TimeSpan.FromMinutes(40)
            },
            new()
            {
                Id = 2,
                Name = "Замена масляного фильтра",
                Category = Specialization.Maintenance,
                Cost = 400.25m,
                Duration = TimeSpan.FromMinutes(15)
            },
            new()
            {
                Id = 3,
                Name = "Диагностика двигателя",
                Category = Specialization.Engine,
                Cost = 3500m,
                Duration = TimeSpan.FromMinutes(60)
            },
            new()
            {
                Id = 4,
                Name = "Замена свечей зажигания",
                Category = Specialization.Engine,
                Cost = 1800m,
                Duration = TimeSpan.FromMinutes(45)
            },
            new()
            {
                Id = 5,
                Name = "Замена масла в коробке передач",
                Category = Specialization.Transmission,
                Cost = 2500m,
                Duration = TimeSpan.FromMinutes(60)
            },
            new()
            {
                Id = 6,
                Name = "Замена сцепления",
                Category = Specialization.Transmission,
                Cost = 6000m,
                Duration = TimeSpan.FromMinutes(180)
            },
            new()
            {
                Id = 7,
                Name = "Диагностика электрооборудования",
                Category = Specialization.Electrical,
                Cost = 900m,
                Duration = TimeSpan.FromMinutes(30)
            },
            new()
            {
                Id = 8,
                Name = "Замена генератора",
                Category = Specialization.Electrical,
                Cost = 1400m,
                Duration = TimeSpan.FromMinutes(60)
            },
            new()
            {
                Id = 9,
                Name = "Ремонт крыла",
                Category = Specialization.Body,
                Cost = 7000m,
                Duration = TimeSpan.FromMinutes(180)
            },
            new()
            {
                Id = 10,
                Name = "Покраска бампера",
                Category = Specialization.Body,
                Cost = 5000m,
                Duration = TimeSpan.FromMinutes(120)
            },
            new()
            {
                Id = 11,
                Name = "Шиномонтаж",
                Category = Specialization.Tires,
                Cost = 2000m,
                Duration = TimeSpan.FromMinutes(45)
            },
            new()
            {
                Id = 12,
                Name = "Балансировка колёс",
                Category = Specialization.Tires,
                Cost = 1500m,
                Duration = TimeSpan.FromMinutes(30)
            }
        ];

        Orders =
        [
            CreateOrder(1, 1, 1, new(2026, 6, 1, 12, 0, 0, TimeSpan.Zero), [1, 2]),
            CreateOrder(2, 2, 3, new(2026, 8, 1, 12, 0, 0, TimeSpan.Zero), [3, 4]),
            CreateOrder(3, 3, 5, new(2026, 8, 26, 12, 0, 0, TimeSpan.Zero), [5]),
            CreateOrder(4, 4, 7, new(2026, 8, 27, 11, 59, 59, TimeSpan.Zero), [7]),
            CreateOrder(5, 1, 2, new(2026, 8, 27, 12, 0, 0, TimeSpan.Zero), [1]),
            CreateOrder(6, 2, 4, new(2026, 8, 27, 11, 59, 59, TimeSpan.Zero), [3]),
            CreateOrder(7, 11, 1, new(2026, 9, 1, 12, 0, 0, TimeSpan.Zero), [1, 2]),
            CreateOrder(8, 2, 3, new(2026, 9, 2, 12, 0, 0, TimeSpan.Zero), [3, 4]),
            CreateOrder(9, 3, 6, new(2026, 9, 3, 12, 0, 0, TimeSpan.Zero), [5, 6]),
            CreateOrder(10, 4, 8, new(2026, 9, 4, 12, 0, 0, TimeSpan.Zero), [7, 8]),
            CreateOrder(11, 5, 9, new(2026, 9, 5, 12, 0, 0, TimeSpan.Zero), [9]),
            CreateOrder(12, 6, 1, new(2026, 9, 6, 12, 0, 0, TimeSpan.Zero), [1, 2]),
            CreateOrder(13, 7, 3, new(2026, 9, 7, 12, 0, 0, TimeSpan.Zero), [3]),
            CreateOrder(14, 8, 5, new(2026, 9, 8, 12, 0, 0, TimeSpan.Zero), [5]),
            CreateOrder(15, 9, 7, new(2026, 9, 9, 12, 0, 0, TimeSpan.Zero), [7]),
            CreateOrder(16, 10, 9, new(2026, 9, 10, 12, 0, 0, TimeSpan.Zero), [9, 10]),
            CreateOrder(17, 1, 1, new(2026, 9, 11, 12, 0, 0, TimeSpan.Zero), [1, 2]),
            CreateOrder(18, 2, 4, new(2026, 9, 12, 12, 0, 0, TimeSpan.Zero), [3]),
            CreateOrder(19, 12, 3, new(2026, 9, 13, 12, 0, 0, TimeSpan.Zero), [3, 4]),
            CreateOrder(20, 3, 5, new(2026, 9, 14, 12, 0, 0, TimeSpan.Zero), [5]),
            CreateOrder(21, 4, 7, new(2026, 9, 15, 12, 0, 0, TimeSpan.Zero), [7]),
            CreateOrder(22, 5, 10, new(2026, 9, 16, 12, 0, 0, TimeSpan.Zero), [9]),
            CreateOrder(23, 6, 2, new(2026, 9, 17, 12, 0, 0, TimeSpan.Zero), [1]),
            CreateOrder(24, 7, 4, new(2026, 9, 18, 12, 0, 0, TimeSpan.Zero), [3, 4]),
            CreateOrder(25, 8, 6, new(2026, 9, 19, 12, 0, 0, TimeSpan.Zero), [5, 6]),
            CreateOrder(26, 1, 1, new(2026, 9, 27, 11, 59, 59, TimeSpan.Zero), [1], completed: false),
            CreateOrder(27, 2, 3, new(2026, 9, 27, 12, 0, 0, TimeSpan.Zero), [3], completed: false),
            CreateOrder(28, 3, 5, new(2026, 9, 28, 12, 0, 0, TimeSpan.Zero), [5], completed: false)
        ];
    }

    private Order CreateOrder(
        int id,
        int carId,
        int mechanicId,
        DateTimeOffset acceptedAt,
        int[] workTypeIds,
        bool completed = true)
    {
        var car = Cars.Single(car => car.Id == carId);
        var workTypes = workTypeIds.Select(id => WorkTypes.Single(work => work.Id == id)).ToArray();
        var totalDuration = TimeSpan.FromTicks(workTypes.Sum(work => work.Duration.Ticks));

        return new()
        {
            Id = id,
            Car = car,
            Client = car.Owner,
            Mechanic = Mechanics.Single(mechanic => mechanic.Id == mechanicId),
            WorkTypes = workTypes,
            AcceptedAt = acceptedAt,
            ReturnedAt = completed ? acceptedAt + totalDuration : null
        };
    }
}
