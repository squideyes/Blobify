using Blobify.Shared.Helpers;
using Microsoft.WindowsAzure.Storage;
using System.IO;

namespace Blobify.Uploader
{
    public class Options : OptionsBase
    {
        [Option("SOURCE", "path", 1, true, false,
            "The local or UNC path to draw blobifiable files from.")]
        public string Source { get; set; }

        [Option("REGEX", "pattern", 1, false, false,
            "A regular expression to filter /SOURCE against.  If a regex is supplied, then all of the files found in /SOURCE will be blobified.")]
        public string Filter { get; set; }

        [Option("RECURSE", null, 1, false, false,
            "If present, the sub-folders under /SOURCE will not searched for blobifyable files.")]
        public bool? NoSubFolders { get; set; }

        [Option("CONTAINER", "name", 1, true, false,
            "The container within /CONN to upload the blobified files to. A container name must be 3 to 63 characters long, start with a lowercase letter or number, and can only contain lowercase letters, numbers, and the dash (-) character.")]
        public string Container { get; set; }

        [Option("PATH", "folders", 1, false, false,
            "The \"local-path\" within /CONTAINER to upload the blobified files to.  If ommited then the blobified files will be uploaded to the root of /CONTAINER. NOTE: /LOCALPATH will be converted to upper-case, by convention.")]
        public string LocalPath { get; set; }

        [Option("CONN", "string", 2, true, false,
            "The Storage connection-string associated with /CONTAINER. If a /CONN was previously saved to the local machine, it will be overwritten.  In any case, the /CONN will be persisted to the local machine using DPAPI security.")]
        public string ConnString { get; set; }

        [Option("NOCONN", null, 3, true, false,
            "Indicates that any previously saved /CONN should be deleted.")]
        public bool? NoConnString { get; set; }

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

            if (LogLevel != null)
                return false;

            return true;
        }

        private bool IsConnString()
        {
            CloudStorageAccount account;

            return CloudStorageAccount.TryParse(ConnString, out account);
        }

        protected override bool GetIsValid()
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
