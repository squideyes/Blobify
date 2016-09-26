using Blobify.Shared.Constants;
using Blobify.Shared.Helpers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blobify.Shared.Logging
{
    public class Logger
    {
        private const string CREATING =
            "Creating the \"{0}\" {1}, if it doesn't already exist.";

        private Severity minSeverity;
        private CloudTable table;
        private string partitionKey;
        private CloudTableClient client;
        private CancellationTokenSource cancellationTokenSource;

        public Logger(Type type, CloudStorageAccount account,
            CancellationTokenSource cancellationTokenSource,
            Severity minSeverity = Severity.Info)
        {
            partitionKey = $"{type.Namespace}_{Environment.MachineName}";

            client = account.CreateCloudTableClient();

            this.cancellationTokenSource = cancellationTokenSource;

            this.minSeverity = minSeverity;
        }

        public async Task Init()
        { 
            LogToConsole(Severity.Debug,
                CREATING, WellKnown.LogTableName, "table");

            table = client.GetTableReference(WellKnown.LogTableName);

            var wasCreated = await table.CreateIfNotExistsAsync();

            await Log(wasCreated ? Severity.Info : Severity.Debug,
                "The \"{0}\" {1} {2}.", WellKnown.LogTableName,
                "table", wasCreated ? "was created" : "already exists");
        }

        public async Task Log(
            Severity severity, string format, params object[] args)
        {
            await Log(null, severity, format, args);
        }

        public async Task Log(Exception error)
        {
            await Log(JsonConvert.SerializeObject(error, Formatting.None),
                Severity.Error, error.Message.ToSingleLine());
        }

        public void LogToConsole(
            Severity severity, string format, params object[] args)
        {
            Console.WriteLine(GetEntity(severity, format, args));
        }

        private LogEntity GetEntity(
            Severity severity, string format, params object[] args)
        {
            return new LogEntity()
            {
                PartitionKey = partitionKey,
                RowKey = $"{DateTime.UtcNow:yyyyMMdd_HHmmssfff}_{Guid.NewGuid():N}",
                Severity = severity.ToString(),
                Message = string.Format(format, args)
            };
        }

        private async Task Log(string errorJson,
            Severity severity, string format, params object[] args)
        {
            if (cancellationTokenSource.IsCancellationRequested)
                return;

            var entity = GetEntity(severity, format, args);

            entity.ErrorJson = errorJson;

            if (table != null && severity >= minSeverity)
                await table.ExecuteAsync(TableOperation.Insert(entity));

            Console.WriteLine(entity);
        }
    }
}
