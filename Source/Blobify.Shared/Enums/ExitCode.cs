using System.ComponentModel;

namespace Blobify.Shared
{
    public enum ExitCode
    {
        [Description("Files were copied without error.")]
        Success = 0,

        [Description("The program was submitted without command-line arguments.")]
        NoArgs = 1,

        [Description("No files were found to copy.")]
        NoFiles = 2,

        [Description("The user pressed CTRL+C to terminate the blobify process.")]
        Cancelled = 3,

        [Description("Initialization error occurred (i.e. you eentered invalid syntax on the command line.)")]
        InitError = 4,

        [Description("Blobfication error occurred.")]
        ProcessingError = 5
    }
}
