using System.ComponentModel;

namespace Blobify.Shared
{
    public enum ExitCode
    {
        [Description("Files were copied without error.")]
        Success = 0,

        [Description("No files were found to copy.")]
        NoFiles = 1,

        [Description("The user pressed CTRL+C to terminate the blobify process.")]
        Cancelled = 2,

        [Description("Initialization error occurred (i.e. you eentered invalid syntax on the command line.)")]
        InitError = 3,

        [Description("Blobfication error occurred.")]
        ProcessingError = 4
    }
}
