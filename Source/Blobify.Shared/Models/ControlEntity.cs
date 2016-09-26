using Microsoft.WindowsAzure.Storage.Table;

namespace Blobify.Shared.Models
{
    public class ControlEntity : TableEntity
    {
        public string BlobName { get; set; }
        public int Status { get; set; }
        public string AlertTos { get; set; }

        public override string ToString() =>
            $"{RowKey} on {PartitionKey} (Blob: {BlobName})";
    }
}
