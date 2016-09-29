using System;

namespace Blobify.Shared.Helpers
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class OptionAttribute : Attribute
    {
        public OptionAttribute(string token, string valueName, int groupId,
            bool valueRequired, string helpText)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException(nameof(token));

            if (groupId <= 0)
                throw new ArgumentNullException(nameof(groupId));

            if (string.IsNullOrWhiteSpace(helpText))
                throw new ArgumentNullException(nameof(helpText));

            Token = token.ToUpper();
            ValueName = valueName?.ToLower();
            GroupId = groupId;
            HelpText = helpText;

            if (valueRequired)
            {
                if (string.IsNullOrWhiteSpace(valueName))
                    Kind = OptionKind.RequiredKeyOnly;
                else
                    Kind = OptionKind.RequiredKeyValue;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(valueName))
                    Kind = OptionKind.OptionalKeyOnly;
                else
                    Kind = OptionKind.OptionalKeyValue;
            }
        }

        public string Token { get; }
        public int GroupId { get; }
        public string ValueName { get; }
        public OptionKind Kind { get; }
        public string HelpText { get; }
    }
}
