using System;
using System.Collections.Generic;

namespace Blobify.Shared.Models
{
    public class AlertInfo
    {
        public DateTime PostedOn { get; set; }
        public string ServerName { get; set; }
        public AppId AppId { get; set; }
        public string BlobName { get; set; }
        public List<string> AlertTos { get; set; }
        public Exception Error { get; set; }

        public Outcome Outcome => Error != null ?
            Outcome.Error : Outcome.Success;

        public override string ToString() =>
            $"{ServerName}/{AppId} {Outcome}";
    }
}
