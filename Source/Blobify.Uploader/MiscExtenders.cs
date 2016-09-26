using Blobify.Shared.Constants;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blobify.Uploader
{
    public static class MiscExtenders
    {
        private static Regex containerNameRegex = new Regex(
            @"^[a-z0-9](?:[a-z0-9]|(\-(?!\-))){1,61}[a-z0-9]$", RegexOptions.Compiled);

        public static string WithSlash(this string value)
        {
            value = value.Trim();

            if (!value.EndsWith(Path.DirectorySeparatorChar.ToString()))
                value += Path.DirectorySeparatorChar;

            return value;
        }

        public static double ToMB(this long value, int digits = 2)
        {
            return Math.Round((double)value / WellKnown.MB, digits);
        }

        public static double ToGB(this long value, int digits = 2)
        {
            return Math.Round((double)value / WellKnown.GB, digits);
        }

        public static bool IsContainerName(this string value)
        {
            return containerNameRegex.IsMatch(value);
        }

        public static bool InRange<T>(this T value, T minValue, T maxValue)
            where T : IComparable<T>
        {
            return (value.CompareTo(minValue) >= 0) && (value.CompareTo(maxValue) <= 0);
        }

        public static async Task<byte[]> GetFileContentAsync(
            this FileInfo file, long offset, int length)
        {
            using (var stream = file.OpenRead())
            {
                stream.Seek(offset, SeekOrigin.Begin);

                var contents = new byte[length];

                var len = await stream.ReadAsync(contents, 0, contents.Length);

                if (len == length)
                    return contents;

                var result = new byte[len];

                Array.Copy(contents, result, len);

                return result;
            }
        }

        public static Task ForEachAsync<T>(
            this IEnumerable<T> source, int parallelUploads, Func<T, Task> body)
        {
            return Task.WhenAll(Partitioner
                .Create(source)
                .GetPartitions(parallelUploads)
                .Select(partition => Task.Run(async () =>
                {
                    using (partition)
                    {
                        while (partition.MoveNext())
                            await body(partition.Current);
                    }
                })));
        }
    }
}
