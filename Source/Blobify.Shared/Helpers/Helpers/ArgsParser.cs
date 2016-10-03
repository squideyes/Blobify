using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Blobify.Shared.Helpers
{
    public class ArgsParser<O> where O : OptionsBase, new()
    {
        public class Spec
        {
            public PropertyInfo Property { get; set; }
            public Type Type { get; set; }
            public string HelpText { get; set; }
            public OptionKind Kind { get; set; }
        }

        private const string LOGLEVELHELPTEXT =
            "The minimum logging level (Trace, Debug, Info, Warn, Error or Fatal); by default: Info.";

        private const string PARAMSHELPTEXT =
            "A file that contains one or more of the above parameters.  Whitespace, such as spaces and newlines, will be ignored, as will per-line comments prefixed by a double-slash.";

        private static Regex chunksRegex =
            new Regex(@"(?<=/).+?:?.*?(?=\s+?/)", RegexOptions.Compiled);

        private const int WIDTH = 78;

        public static void ShowHelp(Exception error = null)
        {
            var options = new List<OptionAttribute>();

            foreach (var property in typeof(O).GetProperties())
            {
                var option = property.GetCustomAttribute<OptionAttribute>();

                if (option == null)
                    continue;

                options.Add(option);
            }

            var appInfo = new AppInfo(typeof(O).Assembly);

            var sb = new StringBuilder();

            if (error != null)
            {
                sb.AppendLine(new string('=', WIDTH));
                sb.AppendLine("ERROR:");

                foreach (var line in error.Message.ToSingleLine().Wrap(WIDTH))
                    sb.AppendLine(line);

                sb.AppendLine(new string('=', WIDTH));
            }

            sb.AppendLine(new string('=', WIDTH));
            sb.AppendLine($"{appInfo.Title}. {appInfo.Copyright}");
            sb.AppendLine(new string('=', WIDTH));
            sb.AppendLine();

            var groups = new SortedDictionary<int, List<OptionAttribute>>();

            foreach (var option in options)
            {
                if (!groups.ContainsKey(option.GroupId))
                    groups.Add(option.GroupId, new List<OptionAttribute>());

                groups.First(g => g.Key == option.GroupId).Value.Add(option);
            }

            foreach (var groupId in groups.Keys)
                groups.First(g => g.Key == groupId).Value.Add(GetLogLevel(groupId));

            groups.Add(groups.Count + 1, new List<OptionAttribute>());

            groups.Last().Value.Add(new OptionAttribute(
                "PARAMS", "file", groups.Keys.Count + 1, true, PARAMSHELPTEXT));

            var cmd = new StringBuilder();

            cmd.Append(appInfo.Product + " ");

            var firstGroup = true;

            foreach (var groupId in groups.Keys)
            {
                if (!firstGroup)
                    cmd.Append(" or ");

                firstGroup = false;

                if (groups.First(g => g.Key == groupId).Value.Count > 1)
                    cmd.Append("[");

                var firstOption = true;

                foreach (var option in groups.First(g => g.Key == groupId).Value)
                {
                    if (!firstOption)
                        cmd.Append(' ');

                    firstOption = false;

                    cmd.Append(GetWrappedToken(option));
                }

                if (groups.First(g => g.Key == groupId).Value.Count > 1)
                    cmd.Append("]");
            }

            var cmdString = cmd.ToString();

            var firstLine = cmdString.Wrap(WIDTH)[0];

            sb.AppendLine(firstLine);

            foreach (var line in cmdString
                .Substring(firstLine.Length).Trim().Wrap(WIDTH - 4))
            {
                sb.AppendLine("    " + line);
            }

            sb.AppendLine();

            foreach (var groupId in groups.Keys)
            {
                sb.AppendLine(new string('-', WIDTH));
                sb.AppendLine();

                firstGroup = false;

                foreach (var option in groups.First(g => g.Key == groupId).Value)
                    ShowOptionHelp(sb, option);
            }

            sb.AppendLine(new string('=', WIDTH));
            sb.AppendLine();

            Console.Write(sb.ToString());
        }

        private static OptionAttribute GetLogLevel(int groupId)
        {
            return new OptionAttribute(
                "LOGLEVEL", "level", groupId, false, LOGLEVELHELPTEXT);
        }

        private static string GetWrappedToken(OptionAttribute option)
        {
            switch (option.Kind)
            {
                case OptionKind.OptionalKeyOnly:
                    return $"{{/{option.Token}}}";
                case OptionKind.OptionalKeyValue:
                    return $"{{/{option.Token}:}}";
                case OptionKind.RequiredKeyOnly:
                    return $"</{option.Token}>";
                case OptionKind.RequiredKeyValue:
                    return $"</{option.Token}:>";
                default:
                    throw new ArgumentOutOfRangeException(nameof(option.Kind));
            }
        }

        private static void ShowOptionHelp(StringBuilder sb, OptionAttribute option)
        {
            var text = option.HelpText;

            var lines = text.Wrap(60);

            for (int i = 0; i < lines.Count; i++)
            {
                if (i == 0)
                {
                    var slug = $"/{option.Token}";

                    if (!string.IsNullOrWhiteSpace(option.ValueName))
                        slug += $":<{option.ValueName}>";

                    sb.Append(slug.PadRight(18));
                }
                else
                {
                    sb.Append(new string(' ', 18));
                }

                sb.AppendLine(lines[i]);
            }

            sb.AppendLine();
        }

        public static O Parse(string commands)
        {
            if (string.IsNullOrWhiteSpace(commands))
                throw new ArgumentNullException(nameof(commands));

            commands = commands.Trim() + " /IGNORE";

            var specs = new Dictionary<string, Spec>();

            var options = new O();

            var properties = typeof(O).GetProperties(
                BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in typeof(O).
                GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                string token;
                OptionKind kind;
                string helpText;

                if (property.Name == nameof(options.LogLevel))
                {
                    token = "LOGLEVEL";
                    kind = OptionKind.OptionalKeyValue;
                    helpText = LOGLEVELHELPTEXT;
                }
                else if (property.Name == nameof(options.ParamsFile))
                {
                    token = "PARAMS";
                    kind = OptionKind.OptionalKeyValue;
                    helpText = PARAMSHELPTEXT;
                }
                else
                {
                    var attrib = property.GetCustomAttribute<OptionAttribute>();

                    if (attrib == null)
                        continue;

                    if (attrib.Token == null)
                        throw new ArgumentNullException(nameof(attrib.Token));

                    token = attrib.Token.ToUpper();
                    kind = attrib.Kind;
                    helpText = attrib.HelpText;
                }

                var spec = new Spec()
                {
                    Property = property,
                    Type = property.PropertyType,
                    HelpText = helpText,
                    Kind = kind
                };

                specs.Add(token, spec);
            }

            var chunks = GetChunks(commands);

            foreach (var chunk in chunks)
            {
                var tv = new TokenValue(chunk);

                Spec spec;

                if (!specs.TryGetValue(tv.Token, out spec))
                    continue;

                if (!tv.HasValue)
                {
                    SetValue(spec.Property, options, true);
                }
                else if (spec.Type.IsEnum)
                {
                    if (Enum.GetNames(spec.Type).Any(
                        e => e.ToUpper() == tv.Value.ToUpper()))
                    {
                        var value = Enum.Parse(spec.Type, tv.Value, true);

                        SetValue(spec.Property, options, value);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(
                            $"\"{tv.Value}\" is an invalid {spec.Type} value.");
                    }
                }
                else if (spec.Type.GetInterface(typeof(IConvertible).FullName) != null)
                {
                    var value = Convert.ChangeType(tv.Value, spec.Type);

                    SetValue(spec.Property, options, value);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(
                        $"\"{tv.Value}\" could not be assigned to the \"{tv.Token}\" option.");
                }
            }

            options.Validate();

            return options;
        }

        private static void SetValue(PropertyInfo info, object instance, object value)
        {
            var targetType = info.PropertyType.IsNullableType()
                 ? Nullable.GetUnderlyingType(info.PropertyType)
                 : info.PropertyType;

            var convertedValue = Convert.ChangeType(value, targetType);

            info.SetValue(instance, convertedValue, null);
        }

        private static List<string> GetChunks(string cmd) =>
            chunksRegex.Matches(cmd).Cast<Match>().Select(m => m.Value).ToList();
    }
}
