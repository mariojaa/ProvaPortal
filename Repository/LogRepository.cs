using ProvaPortal.Repository.Interface;

namespace ProvaPortal.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly ILogger<LogRepository> _logger;

        public LogRepository(ILogger<LogRepository> logger)
        {
            _logger = logger;
        }

        public void LogDatabaseAction(string message)
        {
            _logger.LogInformation($"[Database Action] {message}");
        }
    }
}
