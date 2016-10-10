using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Blobify.Shared.Helpers
{
    public static class StringExtenders
    {
        private static Regex containerNameRegex = new Regex(
            "^[a-z0-9](?:[a-z0-9]|(\\-(?!\\-))){1,61}[a-z0-9]$|^\\$root$",
                RegexOptions.Compiled);

        private static bool isInvalidEmail = false;

        public static bool IsConnString(this string value)
        {
            CloudStorageAccount account;

            return CloudStorageAccount.TryParse(value, out account);
        }

        public static bool IsLocalPath(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (value.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                return false;

            try
            {
                if (Path.IsPathRooted(value))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsRegexPattern(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            try
            {
                Regex.Match("", value);

                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public static bool IsContainerName(this string value) =>
            containerNameRegex.IsMatch(value);

        public static bool IsEmail(this string value)
        {
            isInvalidEmail = false;

            if (string.IsNullOrEmpty(value))
                return false;

            try
            {
                value = Regex.Replace(value, @"(@)(.+)$",
                    DomainMapper, RegexOptions.None,
                    TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (isInvalidEmail)
                return false;

            try
            {
                return Regex.IsMatch(value,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private static string DomainMapper(Match match)
        {
            var idn = new IdnMapping();

            string domainName = match.Groups[2].Value;

            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                isInvalidEmail = true;
            }

            return match.Groups[1].Value + domainName;
        }

        public static string WithSlash(this string value)
        {
            if (!value.EndsWith(Path.DirectorySeparatorChar.ToString()))
                value += Path.DirectorySeparatorChar;

            return value;
        }

        public static string ToSingleLine(this string value, string separator = "; ")
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return string.Join(separator, value.ToLines());
        }

        private static List<string> ToLines(this string value)
        {
            var reader = new StringReader(value);

            var lines = new List<string>();

            string line;

            while ((line = reader.ReadLine()) != null)
                lines.Add(line.Trim());

            return lines;
        }

        public static List<string> Wrap(this string text, int margin)
        {
            int start = 0;

            int end;

            var lines = new List<string>();

            text = text.Trim();

            while ((end = start + margin) < text.Length)
            {
                while ((text[end]) != ' ' && (end > start))
                    end -= 1;

                if (end == start)
                    end = start + margin;

                lines.Add(text.Substring(start, end - start));

                start = end + 1;
            }

            if (start < text.Length)
                lines.Add(text.Substring(start));

            return lines;
        }
        public static bool IsTrimmed(this string value) =>
            value == null || value == value.Trim();

        public static bool IsFileName(this string value, bool mustBeRooted = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                new FileInfo(value);

                if (!mustBeRooted)
                    return true;
                else
                    return Path.IsPathRooted(value);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (PathTooLongException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }

        public static bool IsFolderName(this string value, bool mustBeRooted = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                new DirectoryInfo(value);

                if (!mustBeRooted)
                    return true;
                else
                    return Path.IsPathRooted(value);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (PathTooLongException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }
    }
}
