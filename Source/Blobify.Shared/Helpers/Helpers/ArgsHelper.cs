using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Blobify.Shared.Helpers.Helpers
{
    public static class ArgsHelper<O> where O : OptionsBase, new()
    {
        public static ExitCode GetOptions(string[] args, out O options) 
        {
            options = null;

            O parsed = null;

            if (args.Length == 0)
                return ExitCode.NoArgs;

            if (!Safe.Run(() => parsed = ArgsParser<O>.Parse(string.Join(" ", args))))
                return ExitCode.BadArgs;

            if (parsed.ArgsFile != null)
            {
                if (!File.Exists(parsed.ArgsFile))
                    return ExitCode.NoArgsFile;

                if (!Safe.Run(() => parsed = LoadArgsFile(parsed.ArgsFile)))
                    return ExitCode.BadArgsFile;
            }

            if (!Safe.Run(parsed, o =>
                Validator.ValidateObject(o, new ValidationContext(o), true)))
            {
                return ExitCode.BadArgs;
            }

            if (!Safe.Run(parsed, o => o.ValidateDependencies()))
                return ExitCode.BadDependencies;

            options = parsed;

            return ExitCode.Success;
        }

        private static O LoadArgsFile(string argsFile)
        {
            var lines = new List<string>();

            using (var reader = new StreamReader(argsFile))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (!line.StartsWith("//"))
                        lines.Add(line);
                }
            }

            return ArgsParser<O>.Parse(string.Join(" ", lines));
        }
    }
}
