using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMedia.Services
{
    public class StoriesService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public StoriesService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanupExpiredStories(stoppingToken);

                // Delay for 24 hours before running the task again
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task CleanupExpiredStories(CancellationToken stoppingToken)
        {
            using (var scope = _services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var expiredStories = await dbContext.Stories
                    .Where(s => s.DeleteAt <= DateTime.UtcNow)
                    .ToListAsync(stoppingToken); // Pass stopping token to ToListAsync

                if (expiredStories.Any())
                {
                    dbContext.Stories.RemoveRange(expiredStories);
                    await dbContext.SaveChangesAsync(stoppingToken); // Pass stopping token to SaveChangesAsync
                }
            }
        }
    }
}
