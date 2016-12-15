using System.ComponentModel;

namespace Blobify.Shared.Helpers
{
    public static class EnumHelper
    {
        public static string GetDescription<T>(this T value)
        {
            var type = typeof(T);

            var member = type.GetMember(value.ToString());

            var attributes = member[0].GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            return ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}
