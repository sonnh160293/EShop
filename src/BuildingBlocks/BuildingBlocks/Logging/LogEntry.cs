namespace BuildingBlocks.Logging
{
    public class LogEntry
    {
        public string RequestPath { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSuccess { get; set; }
        public string ResponseStatus { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
    }
}
