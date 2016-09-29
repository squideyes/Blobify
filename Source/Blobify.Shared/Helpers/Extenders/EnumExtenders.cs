using System;

namespace Blobify.Shared.Helpers
{
    public static class EnumExtenders
    {
        public static bool IsDefinedEnumValue(this Enum enumeration) =>
            Enum.IsDefined(enumeration.GetType(), enumeration);

        public static T ToEnum<T>(this string value) =>
            (T)Enum.Parse(typeof(T), value, true);
    }
}
