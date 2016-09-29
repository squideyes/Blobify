using Blobify.Shared.Helpers;
using Microsoft.WindowsAzure.Storage;
using System;
using System.IO;

namespace Blobify.Uploader
{
    public class Options : OptionsBase
    {
        [Option("SOURCE", "path", 1, true,
            "The local or UNC path to draw blobifiable files from.")]
        public string Source { get; set; }

        [Option("REGEX", "pattern", 1, false,
            "A regular expression to filter /SOURCE against.  If a regex is supplied, then all of the files found in /SOURCE will be blobified.")]
        public string Regex { get; set; }

        [Option("RECURSE", null, 1, false,
            "If present, the sub-folders under /SOURCE will not searched for blobifyable files.")]
        public bool? Recurse { get; set; }

        [Option("CONTAINER", "name", 1, true,
            "The container within /CONN to upload the blobified files to. A container name must be 3 to 63 characters long, start with a lowercase letter or number, and can only contain lowercase letters, numbers, and the dash (-) character.")]
        public string Container { get; set; }

        [Option("PATH", "folders", 1, false,
            "The \"local-path\" within /CONTAINER to upload the blobified files to.  If ommited then the blobified files will be uploaded to the root of /CONTAINER. NOTE: /PATH will be converted to upper-case, by convention, and will be created on the fly if nescessary.")]
        public string LocalPath { get; set; }

        [Option("CONN", "string", 2, true,
            "The Storage connection-string associated with /CONTAINER. If a /CONN was previously saved to the local machine, it will be overwritten.  In any case, the /CONN will be persisted to the local machine using DPAPI security.")]
        public string ConnString { get; set; }

        [Option("NOCONN", null, 3, true,
            "Indicates that any previously saved /CONN should be deleted.")]
        public bool? NoConnString { get; set; }

        private void AssertOptionNotSet(string optionName, bool isEmpty)
        {
            if (!isEmpty)
            {
                throw new ArgumentException(
                    $"The \"{optionName}\" option wasn't ommited, as expected!");
            }
        }

        private void AssertBlobifyOptionsNotSet()
        {
            AssertOptionNotSet(nameof(Source), Source == null);
            AssertOptionNotSet(nameof(Regex), Regex == null);
            AssertOptionNotSet(nameof(Recurse), !Recurse.HasValue);
            AssertOptionNotSet(nameof(Container), Container == null);
            AssertOptionNotSet(nameof(LocalPath), LocalPath == null);
        }

        protected override void Normalize()
        {
            if (LocalPath != null)
                LocalPath = LocalPath.Replace(@"\", "/");
        }

        public override void Validate()
        {
            if (ParamsFile != null)
            {
                AssertBlobifyOptionsNotSet();
                AssertOptionNotSet(nameof(ConnString), ConnString == null);
                AssertOptionNotSet(nameof(NoConnString), !NoConnString.HasValue);

                if (!File.Exists(ParamsFile))
                {
                    throw new ArgumentException(
                        $"The \"{ParamsFile}\" /PARAMS file could not be found!");
                }
            }
            else if (NoConnString.HasValue)
            {
                AssertBlobifyOptionsNotSet();
                AssertOptionNotSet(nameof(ConnString), ConnString == null);
            }
            else if (ConnString != null)
            {
                AssertBlobifyOptionsNotSet();
                AssertOptionNotSet(nameof(NoConnString), !NoConnString.HasValue);

                CloudStorageAccount account;

                if (!CloudStorageAccount.TryParse(ConnString, out account))
                {
                    throw new ArgumentOutOfRangeException(
                       $"The /CONN is an invalid Azure Storage connection string ({ConnString})!");
                }
            }
            else
            {
                AssertOptionNotSet(nameof(ConnString), ConnString == null);
                AssertOptionNotSet(nameof(NoConnString), !NoConnString.HasValue);

                if (string.IsNullOrWhiteSpace(Source))
                {
                    throw new ArgumentNullException(
                        "A /SOURCE directory must be supplied!");
                }
                else if (Source.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
                {
                    throw new ArgumentNullException(
                        "A valid /SOURCE directory must be supplied!");
                }
                else if (!Directory.Exists(Source))
                {
                    throw new ArgumentException(
                        $"The \"{Source}\" /SOURCE directory could not be found!");
                }

                if (Regex != null && !Regex.IsRegexPattern())
                {
                    throw new ArgumentOutOfRangeException(
                        $"The \"{Regex}\" /REGEX pattern is invalid!");
                }

                if (!Container.IsContainerName())
                {
                    throw new ArgumentOutOfRangeException(
                        $"The \"{Container}\" /CONTAINER name is invalid!");
                }

                if (LocalPath != null && !LocalPath.IsLocalPath())
                {
                    throw new ArgumentOutOfRangeException(
                        $"The \"{LocalPath}\" /PATH is invalid!");
                }
            }
        }
    }
}
