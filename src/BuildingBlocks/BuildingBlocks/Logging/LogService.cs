using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BuildingBlocks.Logging
{
    public class LogService : ILogService
    {
        private readonly IMongoCollection<LogEntry> _logs;

        public LogService(IConfiguration configuration)
        {
            // Truy cập các giá trị cấu hình trực tiếp từ IConfiguration
            var connectionString = configuration["MongoSettings:ConnectionString"];
            var databaseName = configuration["MongoSettings:DatabaseName"];
            var logsCollectionName = configuration["MongoSettings:LogsCollectionName"];

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("EShop_Logging");
            _logs = database.GetCollection<LogEntry>("Log");
        }


        public async Task LogRequestAsync(LogEntry logEntry)
        {
            await _logs.InsertOneAsync(logEntry);
        }
    }
}
