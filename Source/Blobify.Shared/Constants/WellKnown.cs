namespace Blobify.Shared.Constants
{
    public static class WellKnown
    {
        public const int KB = 1024;
        public const int MB = KB * 1024;
        public const long GB = MB * 1024;

        public const int MaxBytesPerBlock = 4 * MB;

        public const string ControlTableName = "Control";
        public const string LogTableName = "Log";

        public const string AlertQueueName = "alerts";

        public const string ConnStringName = "ConnString";
    }
}
