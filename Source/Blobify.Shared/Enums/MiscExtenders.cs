using System.IO;

namespace Blobify.Shared.Helpers
{
    public static class MiscExtenders
    {
        public static void EnsurePathExists(this string filePath)
        {
            var path = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
