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

        private const int WIDTH = 78;

        public void ShowHelp()
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

            sb.AppendLine(new string('=', WIDTH));
            sb.AppendLine($"{appInfo.Title}. {appInfo.Copyright}");
            sb.AppendLine(new string('=', WIDTH));
            sb.AppendLine();

            var groups = new SortedDictionary<int, List<OptionAttribute>>();

            foreach (var option in options)
            {
                if (!groups.ContainsKey(option.GroupId))
                    groups.Add(option.GroupId, new List<OptionAttribute>());

                groups[option.GroupId].Add(option);
            }

            foreach (var groupId in groups.Keys)
                groups[groupId].Add(GetLogLevel(groupId));

            groups.Add(groups.Keys.Count + 1, new List<OptionAttribute>());

            groups[groups.Keys.Last()].Add(new OptionAttribute("PARAMS", "file", 6, true, false,
                "A file that contains one or more of the above parameters.  Whitespace, such as spaces and newlines, will be ignored, as will per-line comments prefixed by a double-slash."));

            var cmd = new StringBuilder();

            cmd.Append(appInfo.Product + " ");

            var firstGroup = true;

            foreach (var groupId in groups.Keys)
            {
                if (!firstGroup)
                    cmd.Append(" or ");

                firstGroup = false;

                if (groups[groupId].Count > 1)
                    cmd.Append("[");

                var firstOption = true;

                foreach (var option in groups[groupId])
                {
                    if (!firstOption)
                        cmd.Append(' ');

                    firstOption = false;

                    cmd.Append(GetWrappedToken(option));
                }

                if (groups[groupId].Count > 1)
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

                foreach (var option in groups[groupId])
                    ShowOptionHelp(sb, option);
            }

            sb.AppendLine(new string('=', WIDTH));
            sb.AppendLine();

            Console.Write(sb.ToString());
        }

        private static OptionAttribute GetLogLevel(int groupId)
        {
            return new OptionAttribute("LOGLEVEL", "level", groupId, false, false,
                "The minimum logging level (Trace, Debug, Info, Warn, Error or Fatal); by default: Info.");
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

        public O Parse(string[] args)
        {
            var cmd = " " + string.Join(" ", args) + " ";

            var specs = new Dictionary<string, Spec>();

            var options = new O();

            var properties = typeof(O).GetProperties(
                BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in typeof(O).
                GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var attrib = property.GetCustomAttribute<OptionAttribute>();

                if (attrib == null)
                    continue;

                if (attrib.Token == null)
                    throw new ArgumentNullException(nameof(attrib.Token));

                var token = attrib.Token.ToUpper();

                var spec = new Spec()
                {
                    Property = property,
                    Type = property.PropertyType,
                    HelpText = attrib.HelpText,
                    Kind = attrib.Kind
                };

                specs.Add(token, spec);
            }

            var chunks = GetChunks(cmd);

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
                }
                else
                {
                    if (spec.Type.GetInterface(typeof(IConvertible).FullName) != null)
                    {
                        var value = Convert.ChangeType(tv.Value, spec.Type);

                        SetValue(spec.Property, options, value);
                    }
                    else
                    {
                        if (spec.Type == typeof(List<string>))
                        {
                            var value = (string)Convert.ChangeType(tv.Value, typeof(string));

                            var items = value.Split(new char[] { ',', ';' })
                                .Select(item => item.Trim()).ToList();

                            SetValue(spec.Property, options, items);
                        }
                        else
                        {
                            return default(O);
                        }
                    }
                }
            }

            if (!options.IsValid)
                return default(O);

            return options;
        }

        private void SetValue(PropertyInfo info, object instance, object value)
        {
            var targetType = info.PropertyType.IsNullableType()
                 ? Nullable.GetUnderlyingType(info.PropertyType)
                 : info.PropertyType;

            var convertedValue = Convert.ChangeType(value, targetType);

            info.SetValue(instance, convertedValue, null);
        }

        private List<string> GetChunks(string cmd)
        {
            var regex = new Regex(
                @"(?<=\s*?/)[A-Za-z]*?[A-Za-z0-9]\s*?(:|\s)");

            cmd = " " + cmd + " ";

            var chunks = new List<string>();

            var matches = regex.Matches(cmd).Cast<Match>().ToList();

            for (int i = 0; i < matches.Count - 1; i++)
            {
                var chunk = cmd.Substring(matches[i].Index,
                    matches[i + 1].Index - matches[i].Index - 1).Trim();

                chunks.Add(chunk);
            }

            if (matches.Count >= 1)
            {
                chunks.Add(cmd.Substring(
                    matches[matches.Count - 1].Index).Trim());
            }

            return chunks;
        }
    }
}
