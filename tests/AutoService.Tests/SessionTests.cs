using AutoService.Infrastructure.InMemory;

namespace AutoService.Tests;

/// <summary>Сеанс сохраняет согласованность данных при асинхронных операциях.</summary>
public class SessionTests
{
    /// <summary>Следующая операция ждёт, пока первая завершит асинхронную работу.</summary>
    [Fact]
    public async Task ExecuteWriteAsync_SerializesOperationsAcrossAwait()
    {
        var session = new AutoServiceSession();
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var secondStarted = false;
        var first = session.ExecuteWriteAsync(async () =>
        {
            await release.Task;
            return 42;
        });
        var second = session.ExecuteWriteAsync(() =>
        {
            secondStarted = true;
            return Task.CompletedTask;
        });

        try
        {
            Assert.False(secondStarted);
        }
        finally
        {
            release.SetResult();
        }

        Assert.Equal(42, await first.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken));
        await second.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
        Assert.True(secondStarted);
    }

    /// <summary>Исключение в любой перегрузке не блокирует последующие операции.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ExecuteWriteAsync_ReleasesGateAfterFailure(bool returnsValue)
    {
        var session = new AutoServiceSession();

        if (returnsValue)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => session.ExecuteWriteAsync<int>(async () =>
            {
                await Task.Yield();
                throw new InvalidOperationException();
            }));
        }
        else
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => session.ExecuteWriteAsync(async () =>
            {
                await Task.Yield();
                throw new InvalidOperationException();
            }));
        }

        var result = await session.ExecuteWriteAsync(() => Task.FromResult(42))
            .WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
        Assert.Equal(42, result);
    }
}
