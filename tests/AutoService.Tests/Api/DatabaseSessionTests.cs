using System.Data.Common;
using AutoService.Application.Services;
using AutoService.Domain.Entities;
using AutoService.Infrastructure.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AutoService.Tests.Api;

/// <summary>Чтение не открывает транзакций, а запись сохраняется или откатывается целиком.</summary>
public class DatabaseSessionTests : ApiTestBase
{
    /// <summary>Все методы чтения сервисов, включая аналитику и связанные данные, обходятся без транзакций.</summary>
    [Fact]
    public async Task ReadServices_DoNotStartTransactions()
    {
        var transactions = new TransactionCounter();
        await using var context = Database.CreateContext(transactions);
        var session = new EfAutoServiceSession(context);
        var clients = new ClientService(session);
        var cars = new CarService(session, TimeProvider.System);
        var mechanics = new MechanicService(session);
        var works = new WorkTypeService(session);
        var orders = new OrderService(session);
        var analytics = new AnalyticsService(session);
        Func<Task>[] queries =
        [
            () => clients.GetAllAsync(), () => clients.GetAsync(1),
            () => clients.GetCarsAsync(1), () => clients.GetOrdersAsync(1),
            () => cars.GetAllAsync(), () => cars.GetAsync(1),
            () => cars.GetOwnerAsync(1), () => cars.GetOrdersAsync(1),
            () => mechanics.GetAllAsync(), () => mechanics.GetAsync(1),
            () => mechanics.GetClientsAsync(1), () => mechanics.GetOrdersAsync(1),
            () => works.GetAllAsync(), () => works.GetAsync(1),
            () => works.GetOrdersAsync(1), () => works.GetMechanicsAsync(1),
            () => orders.GetAllAsync(), () => orders.GetAsync(1),
            () => orders.GetCarAsync(1), () => orders.GetClientAsync(1),
            () => orders.GetMechanicAsync(1), () => orders.GetWorkTypesAsync(1),
            () => orders.GetTotalCostAsync(1), () => analytics.GetMostFrequentWorkTypesAsync(),
            () => analytics.GetRepeatVisitsAsync(new DateTimeOffset(2026, 9, 27, 12, 0, 0, TimeSpan.Zero))
        ];

        foreach (var query in queries)
        {
            await query();
        }

        Assert.Equal(0, transactions.Started);
    }

    /// <summary>Успешная запись коммитится один раз и доступна из другого контекста.</summary>
    [Fact]
    public async Task ExecuteWriteAsync_CommitsCreatedClient()
    {
        var transactions = new TransactionCounter();
        await using var context = Database.CreateContext(transactions);
        var session = new EfAutoServiceSession(context);

        var client = await session.ExecuteWriteAsync(() => session.Clients.AddAsync(new Client
        {
            Id = 0,
            FullName = "Клиент после коммита",
            Phone = "+79990000001"
        }));

        Assert.Equal(1, transactions.Started);
        Assert.Null(context.Database.CurrentTransaction);
        await using var fresh = Database.CreateContext();
        Assert.NotNull(await fresh.Clients.FindAsync([client.Id], TestContext.Current.CancellationToken));
    }

    /// <summary>Исключение после SaveChanges откатывает запись и освобождает транзакцию.</summary>
    [Fact]
    public async Task ExecuteWriteAsync_RollsBackSavedClientAfterFailure()
    {
        var transactions = new TransactionCounter();
        await using var context = Database.CreateContext(transactions);
        var session = new EfAutoServiceSession(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => session.ExecuteWriteAsync(async () =>
        {
            await session.Clients.AddAsync(new Client
            {
                Id = 0,
                FullName = "Клиент из отменённой операции",
                Phone = "+79990000002"
            });
            throw new InvalidOperationException("Отмена операции после сохранения.");
        }));

        Assert.Equal(1, transactions.Started);
        Assert.Null(context.Database.CurrentTransaction);
        Assert.Empty(context.ChangeTracker.Entries());
        await using var fresh = Database.CreateContext();
        Assert.Equal(10, await fresh.Clients.CountAsync(TestContext.Current.CancellationToken));
    }

    private sealed class TransactionCounter : DbTransactionInterceptor
    {
        public int Started { get; private set; }

        public override ValueTask<DbTransaction> TransactionStartedAsync(
            DbConnection connection, TransactionEndEventData eventData, DbTransaction result,
            CancellationToken cancellationToken = default)
        {
            Started++;
            return ValueTask.FromResult(result);
        }
    }
}
