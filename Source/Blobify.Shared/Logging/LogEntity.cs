using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Blobify.Shared.Logging
{
    public class LogEntity : TableEntity
    {
        public string Severity { get; set; }
        public string Message { get; set; }
        public string ErrorJson { get; set; }

        private DateTime LoggedOn => Timestamp ==
            default(DateTimeOffset) ? DateTime.UtcNow : Timestamp.DateTime;

        public override string ToString() =>
            $"{LoggedOn:MM/dd/yyyy HH:mm:ss.fff} {Severity,-8} {Message}";
    }
}
