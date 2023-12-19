namespace TaskManagementSystem.Services
{
    public class DailyStatusCheckService : BackgroundService
    {
        private readonly ILogger<DailyStatusCheckService> _logger;
        private readonly IServiceScopeFactory _factory;
        private readonly int _interval;

        public DailyStatusCheckService(ILogger<DailyStatusCheckService> logger, IServiceScopeFactory factory,
            IConfiguration configuration)
        {
            _logger = logger;
            _factory = factory;
            try
            {
                _interval = configuration.GetValue<int>("DailyStatusCheckPeriod");
            }
            catch
            {
                _interval = 30;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(new TimeSpan(0, 0, _interval));
            while (!stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                    StatusCheckService statusCheckService = asyncScope.ServiceProvider.GetRequiredService<StatusCheckService>();
                    statusCheckService.CheckTasksStatuses();
                    _logger.LogInformation("Executed DailyStatusCheckService");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Failed to execute DailyStatusCheckService with exception message {ex.Message}.");
                }
            }
        }
    }
}
