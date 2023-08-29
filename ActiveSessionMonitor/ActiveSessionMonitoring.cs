using CustomerSupportChatApi.BusinessLayer.Interfaces;
using CustomerSupportChatApi.DataLayer.Models;

namespace ActiveSessionMonitor
{
    public class ActiveSessionMonitoring : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<ActiveSessionMonitoring> _logger;
        private Timer? _timer = null;
        IService<ChatSession> _chatSessionService;

        public ActiveSessionMonitoring(ILogger<ActiveSessionMonitoring> logger, IService<ChatSession> chatSessionService)
        {
            _logger = logger;
            _chatSessionService = chatSessionService;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(CheckForInactiveSessions, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        private void CheckForInactiveSessions(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);

            // code for determining if session is not polled for 3 seconds and rendering it inactive if true
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}