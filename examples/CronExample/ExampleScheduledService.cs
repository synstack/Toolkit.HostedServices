using Microsoft.Extensions.Logging;
using NekoSpace.Toolkit.HostedServices;

namespace CronExample;

class ExampleScheduledService : CronScheduledService
{
    private readonly ILogger<ExampleScheduledService> _logger;

    /// <summary>
    /// Executes every 5 minutes.
    /// </summary>
    protected override string CronExpression => "*/5 * * * *";

    public ExampleScheduledService(ILogger<ExampleScheduledService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Doing works...");

        // Simulate some works
        await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

        _logger.LogInformation("Work done!");
    }
}