namespace BuildingBlocks.Logging
{
    public class LogEntry
    {
        public string RequestPath { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSuccess { get; set; }
        public string ResponseStatus { get; set; }
        public string ErrorMessage { get; set; }
    }
}
