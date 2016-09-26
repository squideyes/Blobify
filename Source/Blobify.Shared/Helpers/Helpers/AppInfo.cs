using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Text;

namespace Blobify.Shared.Helpers
{
    public class AppInfo
    { 
        public AppInfo(Assembly assembly)
        {
            Contract.Requires(assembly != null, nameof(assembly));

            Product = GetProduct(assembly);
            Version = assembly.GetName().Version;
            Copyright = GetCopyright(assembly).Replace("©", "(c)");
        }

        public string Product { get; private set; }
        public Version Version { get; private set; }
        public string Copyright { get; private set; }

        public string Title
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append(Product);

                sb.Append(" v");
                sb.Append(Version.Major);
                sb.Append('.');
                sb.Append(Version.Minor);

                if ((Version.Build != 0) || (Version.Revision != 0))
                {
                    sb.Append('.');
                    sb.Append(Version.Build);
                }

                if (Version.Revision != 0)
                {
                    sb.Append('.');
                    sb.Append(Version.Revision);
                }

                return sb.ToString();
            }
        }

        private static string GetCopyright(Assembly assembly)
        {
            return assembly.GetAttribute<AssemblyCopyrightAttribute>().Copyright;
        }

        private static string GetProduct(Assembly assembly)
        {
            return assembly.GetAttribute<AssemblyProductAttribute>().Product;
        }
    }
}
