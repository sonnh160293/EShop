namespace BuildingBlocks.Logging
{
    public interface ILogService
    {
        Task LogRequestAsync(LogEntry logEntry);
    }
}
