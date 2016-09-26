using System;
using System.Reflection;

namespace Blobify.Shared.Helpers
{
    public static partial class AssemblyExtenders
    {
        public static T GetAttribute<T>(this Assembly callingAssembly)
            where T : Attribute
        {
            T result = null;

            var configAttributes = Attribute.
                GetCustomAttributes(callingAssembly, typeof(T), false);

            if (configAttributes != null)
                result = (T)configAttributes[0];

            return result;
        }
    }
}
