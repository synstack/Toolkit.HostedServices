using System.Diagnostics;
using Cronos;
using Microsoft.Extensions.Hosting;
using Timer = System.Timers.Timer;

namespace NekoSpace.Toolkit.HostedServices;

public abstract class CronScheduledService : IHostedService, IDisposable
{
    private readonly Timer _timer = new();
    private CancellationTokenSource? _stoppingCts;
    private CronExpression? _cronExpr;

    protected abstract string CronExpression { get; }

    protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

    protected CronScheduledService()
    {
        _timer.Elapsed += async (_, _) =>
        {
            RefreshTimer();
            await ExecuteAsync(_stoppingCts?.Token ?? default);
        };
    }

    private void RefreshTimer()
    {
        _cronExpr ??= Cronos.CronExpression.Parse(CronExpression);

        _timer.Stop();

        var now = DateTime.UtcNow;
        var nextOccurrence = _cronExpr.GetNextOccurrence(now);
        if (nextOccurrence is not { } next)
        {
            Debug.WriteLine("No next occurrence.");
            return;
        }

        var delay = next - now;

        Debug.WriteLine($"Delay {delay}, next occurrence: {next.ToLocalTime()}");

        _timer.Interval = delay.TotalMilliseconds;
        _timer.Start();
    }

    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        _stoppingCts = new CancellationTokenSource();
        RefreshTimer();
        return Task.CompletedTask;
    }

    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        _stoppingCts?.Cancel();
        return Task.CompletedTask;
    }

    public virtual void Dispose()
    {
        _timer.Dispose();
        GC.SuppressFinalize(this);
    }
}