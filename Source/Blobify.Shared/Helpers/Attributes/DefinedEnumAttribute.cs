using System;

namespace Blobify.Shared.Helpers
{
    public class DefinedEnumAttribute : StringValidationAttribute
    {
        private Type enumType;

        public DefinedEnumAttribute(Type enumType, string argName = null)
            : base(argName)
        {
            this.enumType = enumType;
        }

        protected override bool IsValid(string value) =>
            Enum.IsDefined(enumType, value);
    }
}
