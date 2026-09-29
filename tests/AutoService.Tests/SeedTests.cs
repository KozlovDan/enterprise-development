using AutoService.Tests.Fixtures;

namespace AutoService.Tests;

/// <summary>Проверяет полноту и согласованность исходных данных лабораторной.</summary>
/// <param name="fixture">Тестовые данные автосервиса.</param>
public class SeedTests(AutoServiceFixture fixture) : IClassFixture<AutoServiceFixture>
{
    /// <summary>В каждой коллекции есть минимум десять объектов с уникальными Id.</summary>
    [Fact]
    public void Seed_ContainsAtLeastTenUniqueEntitiesOfEachType()
    {
        // Arrange
        int[][] identifiers =
        [
            fixture.Clients.Select(client => client.Id).ToArray(),
            fixture.Cars.Select(car => car.Id).ToArray(),
            fixture.Mechanics.Select(mechanic => mechanic.Id).ToArray(),
            fixture.WorkTypes.Select(work => work.Id).ToArray(),
            fixture.Orders.Select(order => order.Id).ToArray()
        ];

        // Act
        var counts = identifiers.Select(ids => (Total: ids.Length, Unique: ids.Distinct().Count())).ToArray();

        // Assert
        Assert.All(counts, count =>
        {
            Assert.True(count.Total >= 10);
            Assert.Equal(count.Total, count.Unique);
        });
    }

    /// <summary>Заказы ссылаются на существующие согласованные объекты и корректные даты.</summary>
    [Fact]
    public void Seed_RelationsAndDatesAreConsistent()
    {
        // Arrange
        var orders = fixture.Orders;

        // Act
        var ownersWithSeveralCars = fixture.Cars
            .GroupBy(car => car.Owner.Id)
            .Where(group => group.Count() > 1)
            .OrderBy(group => group.Key)
            .Select(group => group.Key)
            .ToArray();

        // Assert
        Assert.Equal([1, 2], ownersWithSeveralCars);
        Assert.All(fixture.Cars, car => Assert.Contains(car.Owner, fixture.Clients));
        Assert.All(fixture.WorkTypes, work =>
        {
            Assert.True(work.Cost > 0);
            Assert.True(work.Duration > TimeSpan.Zero);
        });
        Assert.All(orders, order =>
        {
            Assert.Contains(order.Car, fixture.Cars);
            Assert.Contains(order.Client, fixture.Clients);
            Assert.Contains(order.Mechanic, fixture.Mechanics);
            Assert.Same(order.Car.Owner, order.Client);
            Assert.NotEmpty(order.WorkTypes);
            Assert.Equal(order.WorkTypes.Count, order.WorkTypes.DistinctBy(work => work.Id).Count());
            Assert.True(order.ReturnedAt is null || order.ReturnedAt >= order.AcceptedAt);
            Assert.All(order.WorkTypes, work =>
            {
                Assert.Contains(work, fixture.WorkTypes);
                Assert.Equal(order.Mechanic.Specialization, work.Category);
            });
        });
    }
}
