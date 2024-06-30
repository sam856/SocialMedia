using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SocialMedia;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class ExpiredStoryCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ExpiredStoryCleanupService> _logger;

    public ExpiredStoryCleanupService(IServiceProvider serviceProvider, ILogger<ExpiredStoryCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CleanUpExpiredStoriesAsync();
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken); 
        }
    }

    private async Task CleanUpExpiredStoriesAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var expiredStories = dbContext.Stories.Where(s => s.DeleteAt <= DateTime.UtcNow).ToList();
            if (expiredStories.Any())
            {
                dbContext.Stories.RemoveRange(expiredStories);
                await dbContext.SaveChangesAsync();
                _logger.LogInformation($"{expiredStories.Count} expired stories deleted at {DateTime.UtcNow}.");
            }
        }
    }
}
