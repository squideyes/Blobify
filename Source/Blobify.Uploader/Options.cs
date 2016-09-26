using Blobify.Shared.Helpers;
using Microsoft.WindowsAzure.Storage;
using System.IO;

namespace Blobify.Uploader
{
    public class Options : IOptions
    {
        [Option("CONN", "string", 1, false,
            "The Storage connection-string associated with /CONTAINER. If a /CONN was previously saved to the local machine, it will be overwritten.  In any case, the /CONN will be persisted to the local machine using DPAPI security.")]
        public string ConnString { get; set; }

        [Option("NOCONN", null, 2, true,
            "Indicates that any previously saved /CONN should be deleted.")]
        public bool? NoConnString { get; set; }

        [Option("SOURCE", "path", 3, true,
            "The local or UNC path to draw blobifiable files from.")]
        public string Source { get; set; }

        [Option("FILTER", "regex", 3, false,
            "A regular expression to filter /SOURCE against.  If a filter is not supplied, then all of the files found in /SOURCE will be blobified.")]
        public string Filter { get; set; }

        [Option("NOSUBFOLDERS", null, 3, false,
            "If present, the sub-folders under /SOURCE will not searched for blobifyable files.")]
        public bool? NoSubFolders { get; set; }

        [Option("CONTAINER", "name", 3, true,
            "The container within /CONN to upload the blobified files to.")]
        public string Container { get; set; }

        [Option("LOCALPATH", "path", 3, false,
            "The \"local-path\" within /CONTAINER to upload the blobified files to.  If ommited then the blobified files will be uploaded to the root of /CONTAINER. NOTE: /LOCALPATH will be converted to upper-case, by convention.")]
        public string LocalPath { get; set; }

        [Option("PARAMS", "file", 4, true,
            "A file that contains one or more of the above parameters.  Whitespace, such as spaces and newlines, will be ignored, as will per-line comments prefixed by a double-slash.")]
        public string ParamsFile { get; set; }

        [Option("LOGLEVEL", "level", 0, false,
            "The minimum logging level (Trace, Debug, Info, Warn, Error or Fatal); by default: Info.")]
        public LogLevel? LogLevel { get; set; }

        private bool BlobifyFieldsEmpty()
        {
            if (!string.IsNullOrWhiteSpace(Source))
                return false;

            if (!string.IsNullOrWhiteSpace(Filter))
                return false;

            if (NoSubFolders.HasValue)
                return false;

            if (!string.IsNullOrWhiteSpace(Container))
                return false;

            if (!string.IsNullOrWhiteSpace(LocalPath))
                return false;

            if (LogLevel.HasValue)
                return false;

            return true;
        }

        private bool IsConnString()
        {
            CloudStorageAccount account;

            return CloudStorageAccount.TryParse(ConnString, out account);
        }

        public bool GetIsValid()
        {
            if (string.IsNullOrWhiteSpace(ParamsFile))
            {
                if (!BlobifyFieldsEmpty())
                    return false;

                if (!string.IsNullOrWhiteSpace(ConnString))
                    return false;

                if (NoConnString.HasValue)
                    return false;
            }
            else if (NoConnString.HasValue)
            {
                if (!BlobifyFieldsEmpty())
                    return false;

                if (!string.IsNullOrWhiteSpace(ConnString))
                    return false;
            }
            else if (!string.IsNullOrWhiteSpace(ConnString))
            {
                if (!BlobifyFieldsEmpty())
                    return false;

                if (NoConnString.HasValue)
                    return false;

                if (!IsConnString())
                    return false;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Source))
                    return false;
                else if (Source.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                    return false;

                if (NoConnString.HasValue)
                    return false;

                if (!string.IsNullOrWhiteSpace(ConnString))
                    return false;
            }

            return true;
        }
    }
}
