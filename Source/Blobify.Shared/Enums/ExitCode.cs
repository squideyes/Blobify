using System.ComponentModel;

namespace Blobify.Shared
{
    public enum ExitCode
    {
        [Description("Files were copied without error.")]
        Success = 0,

        [Description("The program was invoked without command-line arguments.")]
        NoArgs = 1,

        [Description("One or more of the command-line arguments are invalid.")]
        BadArgs = 2,

        [Description("The arguments-file could not be found!")]
        NoArgsFile = 3,

        [Description("The arguments-file has invalid data or cannot be parsed.")]
        BadArgsFile = 4,

        [Description("One or more of the required dependencies could not be resolved.")]
        BadDependencies = 5,







        //[Description("No files were found to copy.")]
        //NoFiles = 3,

        //[Description("The user pressed CTRL+C to terminate the blobify process.")]
        //Cancelled = 4,

        //[Description("An initialization error occurred (i.e. invalid syntax was entered on the command line.)")]
        //InitError = 5,

        //[Description("Blobfication error occurred.")]
        //ProcessingError = 6
    }
}
