using Blobify.Shared.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System;
using System.IO;

namespace Blobify.Uploader
{
    [CustomValidation(typeof(Options), nameof(FinalCheck))]
    public class Options : OptionsBase
    {
        [Option("SOURCE", "path", 0, true,
            "The local or UNC path to draw blobifiable files from.")]
        [FolderName]
        public string Source { get; set; }

        [Option("REGEX", "pattern", 0, false,
            "A regular expression to filter /SOURCE against.  If a regex is supplied, then all of the files found in /SOURCE will be blobified.")]
        [RegexPattern]
        public string Regex { get; set; }

        [Option("RECURSE", null, 0, false,
            "If present, the sub-folders under /SOURCE will be recursively searched for blobifyable files.")]
        public bool? Recurse { get; set; }

        [Option("CONTAINER", "name", 0, true,
            "The container within /CONN to upload the blobified files to. A container name must be 3 to 63 characters long, start with a lowercase letter or number, and can only contain lowercase letters, numbers, and the dash (-) character.")]
        [ContainerName]
        public string Container { get; set; }

        [Option("PATH", "folders", 0, false,
            "The \"local-path\" within /CONTAINER to upload the blobified files to.  If ommited then the blobified files will be uploaded to the root of /CONTAINER. NOTE: /PATH will be converted to upper-case, by convention, and created on the fly if nescessary.")]
        [LocalPath("PATH")]
        public string LocalPath { get; set; }

        [Option("CONN", "string", 1, true,
            "The Storage connection-string associated with /CONTAINER. If a /CONN was previously saved to the local machine, it will be overwritten.  In any case, the /CONN will be persisted to the local machine using DPAPI security.")]
        [ConnString("CONN")]
        public string ConnString { get; set; }

        [Option("NOCONN", null, 2, true,
            "Indicates that any previously saved /CONN should be deleted.")]
        public bool? NoConnString { get; set; }

        private static ValidationResult NotOmmited(string optionName)
        {
            return new ValidationResult(
                $"The \"{optionName}\" option wasn't ommited, as expected!");
        }

        private static ValidationResult NotSupplied(string optionName)
        {
            return new ValidationResult(
                $"The \"{optionName}\" option wasn't supplied, as expected!");
        }

        protected override void Normalize()
        {
            if (LocalPath != null)
                LocalPath = LocalPath.Replace(@"\", "/");
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Source != null)
                sb.Append($"/SOURCE:{Source} ");

            if (Regex != null)
                sb.Append($"/REGEX:{Regex} ");

            if (Recurse.HasValue)
                sb.Append("/RECURSE ");

            if (Container != null)
                sb.Append($"/CONTAINER:{Container} ");

            if (LocalPath != null)
                sb.Append($"/PATH:{LocalPath} ");

            if (ConnString != null)
                sb.Append($"/CONN:{ConnString}");

            if (NoConnString.HasValue)
                sb.Append($"/NOCONN ");

            return sb.ToString();
        }

        private ValidationResult ValidateBlobifyOptionsNotSet()
        {
            if (Source != null)
                return NotOmmited(nameof(Source));

            if (Regex != null)
                return NotOmmited(nameof(Regex));

            if (Recurse.HasValue)
                return NotOmmited(nameof(Recurse));

            if (Container != null)
                return NotOmmited(nameof(Container));

            if (LocalPath != null)
                return NotOmmited(nameof(LocalPath));

            return ValidationResult.Success;
        }

        public override void ValidateDependencies()
        {
            if ((ParamsFile != null) && !File.Exists(ParamsFile))
            {
                throw new FileNotFoundException(
                    "The \"PARAMS\" file could not be found!");
            }

            if ((Source != null) && !Directory.Exists(Source))
            {
                throw new DirectoryNotFoundException(
                    "The \"SOURCE\" directory could not be found!");
            }
        }

        public static ValidationResult FinalCheck(
            Options options, ValidationContext context)
        {
            if (options.ParamsFile != null)
            {
                var result = options.ValidateBlobifyOptionsNotSet();

                if (result != ValidationResult.Success)
                    return result;

                if (options.ConnString != null)
                    return NotOmmited(nameof(ConnString));

                if (options.NoConnString.HasValue)
                    return NotOmmited(nameof(NoConnString));
            }
            else if (options.NoConnString.HasValue)
            {
                var result = options.ValidateBlobifyOptionsNotSet();

                if (result != ValidationResult.Success)
                    return result;

                if (options.ConnString != null)
                    return NotOmmited(nameof(ConnString));
            }
            else if (options.ConnString != null)
            {
                var result = options.ValidateBlobifyOptionsNotSet();

                if (result != ValidationResult.Success)
                    return result;

                if (options.NoConnString.HasValue)
                    return NotOmmited(nameof(NoConnString));
            }
            else
            {
                if (options.ConnString != null)
                    return NotOmmited(nameof(ConnString));

                if (options.NoConnString.HasValue)
                    return NotOmmited(nameof(NoConnString));

                if (options.Source == null)
                    return NotSupplied(nameof(Source));

                if (options.Container == null)
                    return NotSupplied(nameof(Container));
            }

            return ValidationResult.Success;
        }
    }
}
